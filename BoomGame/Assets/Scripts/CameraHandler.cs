using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private static readonly float panSpeed = 50f;
    private static readonly float zoomSpeedTouch = 0.1f;
    private static readonly float zoomSpeedMouse = 15f;

    private static readonly float[] boundsXClosest = new float[] { -35f, 35f };
    private static readonly float[] boundsYClosest = new float[] { -10f, 15f };
    private static readonly float[] camSizeBounds = new float[] { 25f, 45f };
    public float defaultCameraSize = 45f;

    private Camera cam;
    private GameController gameController;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    private bool allowCameraMovement = true;

    private Vector3 cameraDefaultPos;

    //double click auto zoom
    private int clicked = 0;
    private float clickTime = 0;
    private float clickDelay = 0.5f;
    private float zoomCameraSize;
    private float origCameraSize;
    private Vector3 origCameraPos;
    private float smoothCameraAlpha = 0;
    private bool autoZoomCamera = false;
    private Vector3 zoomCameraPos;

    void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cameraDefaultPos = cam.transform.position;
        cam.orthographicSize = defaultCameraSize;

        gameController = GameObject.FindObjectOfType<GameController>();

        if (gameController == null)
            Debug.LogError("GameController not found in the scene for the IngameHUD");

    }

    // Update is called once per frame
    void Update()
    {

        //Smooth automatic camera zooming and positioning
        if (autoZoomCamera)
        {
            smoothCameraAlpha += Time.deltaTime;
            if (smoothCameraAlpha > 1f)
                smoothCameraAlpha = 1f;

            var nextCamSize = Mathf.SmoothStep(origCameraSize, zoomCameraSize, smoothCameraAlpha);
            cam.orthographicSize = nextCamSize;
            //print(nextCamSize);

            var camPosition = Vector3.Lerp(origCameraPos, zoomCameraPos, smoothCameraAlpha);
            camPosition.z = transform.position.z;
            transform.position = camPosition;

            if (smoothCameraAlpha >= 1f)
            {
                smoothCameraAlpha = 0;
                autoZoomCamera = false;
                allowCameraMovement = true;
            }
        }

        if (!allowCameraMovement || gameController.bombUnderMouse != null)
        {
            return;
        }
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }
    }

    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        //TODO Check that no bomb on cursor
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;

            //DoubleClick();

        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, zoomSpeedMouse);
    }

    void HandleTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DoubleClick();
        }

        switch (Input.touchCount)
        {

            case 1: // Panning
                wasZoomingLastFrame = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
                break;

            case 2: // Zooming
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the 
                    // distance between the previous positions.
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, zoomSpeedTouch);

                    lastZoomPositions = newPositions;
                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    void DoubleClick()
    {

        //TODO Some issues here
        //Double click
        clicked++;
        if (clicked == 1) clickTime = Time.time;

        if (clicked > 1 && Time.time - clickTime < clickDelay)
        {
            clicked = 0;
            clickTime = 0;
            print("double click");
            AutoZoom();
        }
        else if (clicked > 2 || Time.time - clickTime > 1) clicked = 0;
    }

    void AutoZoom()
    {
        allowCameraMovement = false;
        autoZoomCamera = true;
        origCameraSize = cam.orthographicSize;
        origCameraPos = cam.transform.position;
        lastPanPosition = origCameraPos;
        zoomCameraPos = cameraDefaultPos;

        //print(cam.orthographicSize);
        if (cam.orthographicSize < defaultCameraSize)
        {
            zoomCameraSize = camSizeBounds[1];
        }
        else
        {
            zoomCameraSize = camSizeBounds[0];
        }
    }

    public void ZoomToSize(float size, Vector3 position)
    {
        allowCameraMovement = false;
        autoZoomCamera = true;
        origCameraSize = cam.orthographicSize;
        origCameraPos = cam.transform.position;
        lastPanPosition = origCameraPos;
        zoomCameraPos = position;
        zoomCameraSize = size;
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * panSpeed, offset.y * panSpeed, 0);

        // Perform the movement
        transform.Translate(move, Space.World);


        //Calculate the alpha for the min and max pan position from the current camera size
        //If camera is zoomed in more we can move closer to the edge of the scene
        //If we are in the max camera size we dont allow panning the camera at all 
        var panAlpha = (cam.orthographicSize - camSizeBounds[0]) / (camSizeBounds[1] - camSizeBounds[0]);
        //print(panAlpha);
        var minPanX = Mathf.Lerp(boundsXClosest[0], cameraDefaultPos.x, panAlpha);
        var maxPanX = Mathf.Lerp(boundsXClosest[1], cameraDefaultPos.x, panAlpha);

        var minPanY = Mathf.Lerp(boundsYClosest[0], cameraDefaultPos.y, panAlpha);
        var maxPanY = Mathf.Lerp(boundsYClosest[1], cameraDefaultPos.y, panAlpha);


        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, minPanX, maxPanX);
        pos.y = Mathf.Clamp(transform.position.y, minPanY, maxPanY);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (offset * speed), camSizeBounds[0], camSizeBounds[1]);

        //Pan camera also when zooming in and out
        PanCamera(Input.mousePosition);
    }

}

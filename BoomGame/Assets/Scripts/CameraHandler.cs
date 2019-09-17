using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private static readonly float panSpeed = 50f;
    private static readonly float zoomSpeedTouch = 0.1f;
    private static readonly float zoomSpeedMouse = 15f;

    private static readonly float[] boundsXClosest = new float[] { -30f, 30f };
    private static readonly float[] boundsYClosest = new float[] { -5f, 15f };
    private static readonly float[] zoomBounds = new float[] { 25f, 45f };

    private Camera cam;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    private bool allowCameraMovement = true;

    private Vector3 cameraDefaultPos;
    private int clicked = 0;
    private float clickTime = 0;
    private float clickDelay = 0.3f;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cameraDefaultPos = cam.transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!allowCameraMovement)
        {
            return;
        }
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            //HandleTouch();
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


            //Double click
            clicked++;
            if (clicked == 1) clickTime = Time.time;

            if (clicked > 1 && Time.time - clickTime < clickDelay)
            {
                clicked = 0;
                clickTime = 0;

                AutoZoom();
            }
            else if (clicked > 2 || Time.time - clickTime > 1) clicked = 0;



        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, zoomSpeedMouse);
    }

    void AutoZoom()
    {

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
        var panAlpha = (cam.orthographicSize - zoomBounds[0]) / (zoomBounds[1] - zoomBounds[0]);
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

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (offset * speed), zoomBounds[0], zoomBounds[1]);

        //Pan camera also when zooming in and out
        PanCamera(Input.mousePosition);
    }
}

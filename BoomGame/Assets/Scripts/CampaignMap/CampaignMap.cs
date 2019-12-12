using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CampaignMap : MonoBehaviour
{
    public GameObject chapterPanel;
    private bool inputAllowed = true;

    private static readonly float panSpeed = 500f;

    private static readonly float[] BoundsX = new float[] { -9f, 70f };
    private static readonly float[] BoundsY = new float[] { -32f, 8f };
    public float defaultCameraSize = 45f;

    private Camera cam;
    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only
    [HideInInspector]
    public Vector3 cameraDefaultPos;

    public GameObject gameMasterPrefab;
    private GameMaster gameMaster;

    private bool chapterPanelVisible = false;
    public Doors doors;

    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null)
        {
            gameMaster = Instantiate(gameMasterPrefab).GetComponent<GameMaster>();
        }
        cam = Camera.main;

        //cam.transform.position = cam.transform.position + cam.transform.parent.transform.position;
        //remove camera from any prefab
        gameObject.transform.parent = null;
        cameraDefaultPos = cam.transform.position;
        cam.orthographicSize = defaultCameraSize;


        if (!gameMaster.doorOpen && doors != null)
            StartCoroutine(OpenDoor());
    }
    private IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(0.1f);
        if (doors != null && !gameMaster.doorOpen)
            doors.OpenDoor();

    }

    private void Update()
    {
        if (inputAllowed)
        {
            if (!UtilityLibrary.IsPositionOverUI(Input.mousePosition))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    bool wasHit = false;
                    //is chapter clicked
                    Vector3 mousePos = UtilityLibrary.GetCurrentMousePosition();

                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10f));
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.tag == "Chapter")
                        {
                            var chapter = hit.collider.gameObject.GetComponent<Chapter>();
                            if (chapter != null)
                            {
                                ShowChapterInfo(chapter);
                                wasHit = true;
                            }
                            else
                            {
                                chapter = hit.collider.transform.parent.gameObject.GetComponent<Chapter>();

                                if (chapter != null)
                                {
                                    ShowChapterInfo(chapter);
                                    wasHit = true;
                                }
                            }
                        }
                    }
                    if (!wasHit)
                    {
                        HideChapterInfo();
                    }
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
        }
    }

    void HandleMouse()
    {
        //print(Input.mousePosition);
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

    }

    void HandleTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //DoubleClick();
        }

        switch (Input.touchCount)
        {
            case 1: // Panning

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

                break;

            default:
                break;
        }
    }

    private void ShowChapterInfo(Chapter chapter)
    {
        if (chapter.enoughPentagrams && chapter.previousLevelsCleared)
        {
            //set all chapters not selected
            foreach (var chap in this.transform.GetComponentsInChildren<Chapter>())
            {
                chap.SetSelected(false);
            }

            chapter.SetSelected(true);

            //show chapter info panel
            if (chapterPanel != null)
            {
                if (!chapterPanelVisible)
                    chapterPanel.GetComponent<RectTransform>().DOAnchorPosX(-50f, Random.Range(0.35f, 0.45f), false).SetEase(Ease.OutBack).SetUpdate(true);
                chapterPanelVisible = true;
                var chapterPanelScript = chapterPanel.GetComponent<ChapterPanel>();
                chapterPanelScript.CreateLevelInfoPanels(chapter);
            }

        }
    }

    private void HideChapterInfo()
    {
        //set all chapters not selected
        foreach (var chap in this.transform.GetComponentsInChildren<Chapter>())
        {
            chap.SetSelected(false);
        }

        //hide chapter info panel
        if (chapterPanel != null)
        {
            if (chapterPanelVisible)
                chapterPanel.GetComponent<RectTransform>().DOAnchorPosX(800f, Random.Range(0.35f, 0.45f), false).SetEase(Ease.InBack).SetUpdate(true);
            chapterPanelVisible = false;
            //chapterPanel.SetActive(false);
        }
    }

    public void BackToMenu()
    {
        if (doors != null)
        {
            doors.CloseDoor();
            StartCoroutine(LoadLevel(1.5f, "MainMenuScene"));
        }
        else
            SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Single);
    }


    private IEnumerator LoadLevel(float delay, string levelName)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);

    }


    Tween cameraMoveTween = null;


    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * panSpeed, offset.y * panSpeed, 0);

        // Perform the movement
        var newPos = cam.transform.position + move;
        newPos.x = Mathf.Clamp(newPos.x, BoundsX[0], BoundsX[1]);
        newPos.y = Mathf.Clamp(newPos.y, BoundsY[0], BoundsY[1]);

        //cam.transform.Translate(move, Space.World);
        if (cameraMoveTween != null)
            cameraMoveTween.Kill();
        cameraMoveTween = cam.transform.DOMove(newPos, 0.5f).SetUpdate(true);

        // Ensure the camera remains within bounds.
        //Vector3 pos = cam.transform.position;
        //pos.x = Mathf.Clamp(cam.transform.position.x, BoundsX[0], BoundsX[1]);
        //pos.y = Mathf.Clamp(cam.transform.position.y, BoundsY[0], BoundsY[1]);
        //cam.transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

}
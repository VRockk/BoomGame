using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;



/// <summary>
/// Resposible for the ingame logic and input
/// </summary>
public class GameController : MonoBehaviour
{
    public int maxRounds = 2;

    //Temporary solution for bomb spawning
    public GameObject bomb;
    public string nextLevelName;

    public int bombCount = 3;


    [HideInInspector]
    public bool allowInput;

    [HideInInspector]
    public int roundCounter = 1;

    [HideInInspector]
    public GameObject bombUnderMouse;

    private int shatteringObjectCount;
    private IngameHUD hud;
    private CameraHandler cameraHandler;

    private int movementCheckCount;

    private AudioSource audioSource;
    public AudioClip plopSound;

    private void Awake()
    {
        hud = GameObject.FindObjectOfType<IngameHUD>();

        if (hud == null)
            Debug.LogError("IngameHUD not found in the scene for the GameController");

        cameraHandler = GameObject.FindObjectOfType<CameraHandler>();

        if (cameraHandler == null)
            Debug.LogError("CameraHandler not found in the scene for the GameController");

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            Debug.LogError("AudioSource not found in the scene for the GameController");
    }

    // Start is called before the first frame update
    void Start()
    {
        allowInput = true;
        roundCounter = 1;
        shatteringObjectCount = GameObject.FindGameObjectsWithTag("ShatteringObject").Length;

        hud.UpdateBombCount(bombCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (allowInput)
        {
            //Checking if mouse is over the UI.
            if (!UtilityLibrary.IsMouseOverUI())
            {
                //Left click
                if (Input.GetMouseButtonDown(0))
                {
                    //print(UtilityLibrary.IsMouseOverUI());

                    Vector3 mousePos = UtilityLibrary.GetCurrentMousePosition();

                    // Check if clicking on a bomb and "attach" it to cursor
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10f));
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                    if (hit.collider != null)
                    {
                        print(hit.collider.gameObject.name);
                        if (hit.collider.gameObject.tag == "Bomb")
                        {
                            bombUnderMouse = hit.collider.gameObject;
                            if (audioSource != null)
                            {
                                audioSource.pitch = Random.Range(1.1f, 1.2f);
                                audioSource.PlayOneShot(plopSound);
                            }
                        }
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = UtilityLibrary.GetCurrentMousePosition();

                //Move bomb if its attached to cursor
                if (bombUnderMouse != null)
                {
                    ////Snap bomb position to shattering objects
                    //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10f));
                    //RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);
                    //foreach (var hit in hits)
                    //{
                    //    print(hit.collider.gameObject.name);

                    //    if (hit.collider.gameObject.tag == "ShatteringObject")
                    //    {
                    //        mousePos = new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, 0f);
                    //    }
                    //}

                    //Set new position for bomb. Add +1 to y axis so the bomb is over your finger
                    bombUnderMouse.transform.position = new Vector3(mousePos.x, mousePos.y + 5f, -1f);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (bombUnderMouse != null)
                {
                    if (audioSource != null)
                    {
                        audioSource.pitch = Random.Range(0.9f, 1.1f);
                        audioSource.PlayOneShot(plopSound);
                    }
                }
                //Mouse up. remove bomb from cursor
                bombUnderMouse = null;
                //cameraHandler.defaultCameraSize;
                //cameraHandler.ZoomToSize(45f);

            }
            else
            {
                //mouse over UI
                if (Input.GetMouseButtonDown(0) && bombCount > 0)
                {
                    Vector3 mousePos = UtilityLibrary.GetCurrentMousePosition();

                    PointerEventData pointerData = new PointerEventData(EventSystem.current);
                    pointerData.position = Input.mousePosition;
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerData, results);

                    //Create a new bomb instance when clicking on Bomb card
                    foreach (RaycastResult result in results)
                    {
                        print(result.gameObject.name);
                        if (result.gameObject.tag == "BombCard")
                        {
                            bombUnderMouse = Instantiate(bomb, new Vector3(mousePos.x, mousePos.y + 5f, -1f), Quaternion.identity);
                            cameraHandler.ZoomToSize(35f, new Vector3(0, -2f, 0));
                            bombCount--;
                            hud.UpdateBombCount(bombCount);

                            if (audioSource != null)
                            {
                                audioSource.pitch = Random.Range(1.1f, 1.2f);
                                audioSource.PlayOneShot(plopSound);
                            }
                        }
                    }
                }
            }
        }
    }


    public void WaitForNextRound()
    {
        allowInput = false;
        //gameController.NextRound();
        movementCheckCount = 0;
        //Check for movement once a second
        InvokeRepeating("CheckForMovement", 1.0f, 1.0f);

        // TODO Make game faster after a while so the pieces settle down faster
    }

    public void Detonation()
    {
        //No bombs. Dont allow
        if (GameObject.FindObjectsOfType<Bomb>().Length == 0)
            return;

        //if (!allowInput)
        //    return;

        //allowInput = false;
        //gameController.allowInput = false;

        cameraHandler.ZoomToSize(45f, new Vector3(0, 2f, 0));

        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (GameObject bombObject in bombs)
        {
            var bomb = bombObject.GetComponent<Bomb>();
            if (bomb != null)
                bomb.Detonate();
        }


        //gameController.WaitForNextRound();
        Invoke("WaitForNextRound", 2f);
    }

    private void CheckForMovement()
    {
        movementCheckCount++;
        bool isMovement = false;
        var rigidBodies = GameObject.FindObjectsOfType<Rigidbody2D>();
        foreach (var body in rigidBodies)
        {
            //Check if there is a bit of movement still
            if (body.velocity.magnitude > 0.1f)
            {
                print(body.gameObject.name + "   " + body.velocity.magnitude);
                isMovement = true;
                break;
            }
        }

        if (movementCheckCount == 5)
        {
            Time.timeScale = 2;
        }

        //If no movement, stop checking and start next round/next level if finished
        if (!isMovement || movementCheckCount > 15)
        {
            Time.timeScale = 1;
            CancelInvoke("CheckForMovement");
            NextRound();
        }
    }

    private void NextRound()
    {
        LevelClear levelClear = CheckLevelClear();
        print(levelClear);
        //Always when Failed
        if (levelClear == LevelClear.Failed)
        {
            allowInput = false;
            hud.LevelFailed();
            return;
        }

        if (levelClear == LevelClear.NotCleared)
        {
            if (roundCounter == maxRounds)
            {
                allowInput = false;
                hud.LevelFailed();
            }
            else
            {
                roundCounter++;
                if (roundCounter == maxRounds && bombCount == 0)
                {
                    //Fail if no bombs left
                    allowInput = false;
                    hud.LevelFailed();
                    return;
                }
                hud.NextRound(roundCounter);

                allowInput = true;
            }
        }
        else
        {
            hud.LevelFinished(levelClear);
        }
    }

    private LevelClear CheckLevelClear()
    {
        bool buildingsDamaged = false;

        var npcBuildings = GameObject.FindObjectsOfType<NPCBuilding>();
        //Check if any buildings have been destroyed
        foreach (var building in npcBuildings)
        {
            //If any building is destroyed level is instantly failed
            //Buildings can be damaged but you get lower score
            if (building.Hitpoints <= 0)
            {
                return LevelClear.Failed;
            }
            if (building.Hitpoints < building.maxHitpoints)
            {
                buildingsDamaged = true;
            }
        }

        var shatteringObjects = GameObject.FindObjectsOfType<ShatteringObject>();

        //Check if all shattering objects have been moved
        foreach (var obj in shatteringObjects)
        {
            if (!obj.ignoreForClear)
            {
                if (obj.initialPosition == obj.gameObject.transform.position)
                {
                    return LevelClear.NotCleared;
                }
            }
        }

        if (roundCounter == 1)
        {
            if (buildingsDamaged)
            {
                return LevelClear.TwoPentagram;
            }
            return LevelClear.ThreePentagram;
        }
        else
        {
            if (buildingsDamaged)
            {
                return LevelClear.OnePentagram;
            }
            return LevelClear.ThreePentagram;
        }
    }


    private void LoadNextLevel()
    {
        //TODO: Show loading screens
        SceneManager.LoadScene(nextLevelName, LoadSceneMode.Single);
    }

}

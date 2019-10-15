using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


using System.Linq;



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
    public bool inputAllowed;

    [HideInInspector]
    public int roundCounter = 0;

    [HideInInspector]
    public GameObject bombUnderMouse;

    private int shatteringObjectCount;
    private IngameHUD hud;
    private CameraHandler cameraHandler;

    private int movementCheckCount;

    private AudioSource audioSource;
    public AudioClip plopSound;

    private bool betweenRounds = false;

    private WinLines winlines;

    public int salvageValue;
    public GameMaster theGM;
    private float roundDelay = 0.5f;
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
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

        var winlinesObj = GameObject.Find("Winlines");

        if (winlinesObj == null)
            Debug.LogError("No winlines found");

        winlines = winlinesObj.GetComponent<WinLines>();

        if (winlines == null)
            Debug.LogError("No winlines found");

        //inputAllowed = true;
        roundCounter = 0;
        NextRound();
        shatteringObjectCount = GameObject.FindGameObjectsWithTag("ShatteringObject").Length;

        hud.UpdateBombCount(bombCount);

        theGM = FindObjectOfType<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputAllowed)
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
                        //print(result.gameObject.name);
                        if (result.gameObject.tag == "BombCard")
                        {
                            bombUnderMouse = Instantiate(bomb, new Vector3(mousePos.x, mousePos.y + 5f, -1f), Quaternion.identity);
                            //cameraHandler.ZoomToSize(35f, new Vector3(0, -2f, 0));
                            bombCount--;
                            hud.UpdateBombCount(bombCount);

                            if (audioSource != null)
                            {
                                audioSource.pitch = Random.Range(1.1f, 1.2f);
                                audioSource.PlayOneShot(plopSound);
                                print(plopSound.name);
                            }
                        }
                    }
                }
            }
        }

    }

    public void WaitForNextRound()
    {
        inputAllowed = false;
        //gameController.NextRound();
        movementCheckCount = 0;

        //Check block movement once a second
        InvokeRepeating("CheckForMovement", 1f, 1f);

        //TODO allow user to press for the faster time scale
    }

    public bool Detonation()
    {
        //No bombs. Dont allow
        if (GameObject.FindObjectsOfType<Bomb>().Length == 0 || !inputAllowed)
            return false;

        inputAllowed = false;

        // zoom to default zoom level
        cameraHandler.ZoomToSize(45f, new Vector3(0, 0, 0));

        //get all bombs and detonate them
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (GameObject bombObject in bombs)
        {
            var bomb = bombObject.GetComponent<Bomb>();
            if (bomb != null)
                bomb.Detonate();
        }
        betweenRounds = true;


        Invoke("WaitForNextRound", 2f);
        return true;
    }

    /// <summary>
    /// Checks for block movement. If waited enough time scale time faster and wait longer.
    /// </summary>
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

        //print(levelClear);
        betweenRounds = false;
        //Always when Failed
        if (levelClear == LevelClear.Failed)
        {
            inputAllowed = false;
            hud.LevelFailed();
            return;
        }

        if (levelClear == LevelClear.NotCleared)
        {
            if (roundCounter == maxRounds)
            {
                inputAllowed = false;
                hud.LevelFailed();
            }
            else
            {
                roundCounter++;
                if (roundCounter == maxRounds && bombCount == 0)
                {
                    //Fail if no bombs left
                    inputAllowed = false;
                    hud.LevelFailed();
                    return;
                }
                hud.NextRound(roundCounter, roundDelay);

                StartCoroutine(AllowInput(true, roundDelay));
                //inputAllowed = true;
            }
        }
        else
        {
            hud.LevelFinished(levelClear);
            theGM.AddSalvage(salvageValue);
        }
    }

    private LevelClear CheckLevelClear()
    {
        LevelClear levelClear = LevelClear.NotCleared;
        bool buildingsDamaged = false;

        var npcBuildings = GameObject.FindObjectsOfType<NPCBuilding>();
        //Check if any buildings have been destroyed
        foreach (var building in npcBuildings)
        {
            //If any building is destroyed level is instantly failed
            //Buildings can be damaged but you get less pentagrams
            if (building.Hitpoints <= 0)
            {
                return LevelClear.Failed;
            }
            if (building.Hitpoints < building.maxHitpoints)
            {
                buildingsDamaged = true;
            }
        }

        //Find the brick that has the highest position 
        var highestBrick = FindObjectsOfType<ShatteringObject>().Where(x => x.gameObject.transform.position.magnitude < 50).OrderByDescending(x => x.gameObject.transform.position.y + x.GetComponent<Collider2D>().bounds.extents.y).First();
        float highestBrickTopPos = highestBrick.transform.position.y + highestBrick.GetComponent<Collider2D>().bounds.extents.y;

        if (highestBrickTopPos <= winlines.threePentaLine)
        {
            levelClear = LevelClear.ThreePentagram;
        }
        else if (highestBrickTopPos <= winlines.twoPentaLine && (roundCounter == maxRounds || bombCount == 0))
        {
            levelClear = LevelClear.TwoPentagram;
        }
        else if (highestBrickTopPos <= winlines.onePentaLine && (roundCounter == maxRounds || bombCount == 0))
        {
            levelClear = LevelClear.OnePentagram;
        }
        else
        {
            return LevelClear.NotCleared;
        }

        //Damaged buildings lower the pentagrams gained by one
        if (buildingsDamaged)
        {
            if (levelClear == LevelClear.ThreePentagram)
                levelClear = LevelClear.TwoPentagram;
            else if (levelClear == LevelClear.TwoPentagram)
                levelClear = LevelClear.OnePentagram;
            else
                levelClear = LevelClear.NotCleared;
        }

        return levelClear;
    }

    private void LoadNextLevel()
    {


        //TODO: Show loading screens
        SceneManager.LoadScene(nextLevelName, LoadSceneMode.Single);

    }


    private IEnumerator AllowInput(bool allow, float delay)
    {
        yield return new WaitForSeconds(delay);
        inputAllowed = allow;
    }
}

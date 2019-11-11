using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


using System.Linq;
using System;

/// <summary>
/// Resposible for the ingame logic and input
/// </summary>
public class GameController : MonoBehaviour
{
    public int maxRounds = 2;

    public GameObject[] bombs;

    public int bombCount = 3;

    [HideInInspector]
    public bool inputAllowed;

    [HideInInspector]
    public int roundCounter = 0;

    [HideInInspector]
    public GameObject bombUnderMouse;


    public GameObject gameMasterPrefab;

    private IngameHUD hud;
    private CameraHandler cameraHandler;

    private int movementCheckCount;

    private AudioSource audioSource;
    public AudioClip plopSound;
    public AudioClip ingameMusic;


    private WinLines winlines;

    private int salvageValue = 100;
    private int bonusSalvageForSavedBomb = 25;

    private GameMaster gameMaster;
    private float roundDelay = 1f;

    [HideInInspector]
    public List<AudioClip> bombScreamsToPlay;

    private int levelScore = 0;

    private void Awake()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null)
        {
            gameMaster = Instantiate(gameMasterPrefab).GetComponent<GameMaster>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0.2f;
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


        CreateBombIcons();

        if (ingameMusic != null)
            gameMaster.SetMusic(ingameMusic);
        else
            gameMaster.SetMusic(null);

        InvokeRepeating("CheckScorelines", 1f, 1f);
    }

    private void CreateBombIcons()
    {
        foreach (var bomb in bombs)
        {
            hud.CreateBombCard(bomb);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputAllowed)
        {
            Vector3 mousePos = UtilityLibrary.GetCurrentMousePosition();


            //Checking if mouse is over the UI.
            if (!UtilityLibrary.IsPositionOverUI(Input.mousePosition))
            {
                //Left click
                if (Input.GetMouseButtonDown(0))
                {
                    // Check if clicking on a bomb and "attach" it to cursor
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10f));
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);


                    if (hit.collider != null)
                    {
                        //print(hit.collider.gameObject.name);
                        if (hit.collider.gameObject.tag == "Bomb")
                        {
                            bombUnderMouse = hit.collider.gameObject;
                            if (audioSource != null)
                            {
                                audioSource.pitch = UnityEngine.Random.Range(1.1f, 1.2f);
                                audioSource.PlayOneShot(plopSound);
                            }
                        }
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {

                //Move bomb if its attached to cursor
                if (bombUnderMouse != null)
                {
                    //Set new position for bomb. Add + to y axis so the bomb is not under player finger
                    bombUnderMouse.transform.position = new Vector3(mousePos.x, mousePos.y + 3f, -1f);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (bombUnderMouse != null)
                {
                    if (audioSource != null)
                    {
                        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                        audioSource.PlayOneShot(plopSound);
                    }

                    if (UtilityLibrary.IsPositionOverUI(Camera.main.WorldToScreenPoint(bombUnderMouse.transform.position)))
                    {
                        //if we are over UI destroy the bomb and add back to bomb count
                        Destroy(bombUnderMouse);
                        bombCount++;
                        StartCoroutine(hud.UpdateBombCount(bombCount));

                    }

                }




                //Mouse up. remove bomb from cursor
                bombUnderMouse = null;

            }
            else
            {
                //mouse over UI
                if (Input.GetMouseButtonDown(0) && bombCount > 0)
                {
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
                            var bombCardScript = result.gameObject.GetComponent<BombCard>();
                            if (bombCardScript != null)
                            {
                                bombUnderMouse = Instantiate(bombCardScript.bombPrefab, new Vector3(mousePos.x, mousePos.y + 3f, -1f), Quaternion.identity);
                                //cameraHandler.ZoomToSize(35f, new Vector3(0, -2f, 0));
                                bombCount--;
                                StartCoroutine(hud.UpdateBombCount(bombCount));

                                if (audioSource != null)
                                {
                                    audioSource.pitch = UnityEngine.Random.Range(1.1f, 1.2f);
                                    audioSource.PlayOneShot(plopSound);
                                }
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
        bombScreamsToPlay = new List<AudioClip>();

        // zoom to default zoom level
        cameraHandler.ZoomToSize(cameraHandler.defaultCameraSize, cameraHandler.cameraDefaultPos);

        //get all bombs and detonate them
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        float bombDelay = 0.1f;
        foreach (GameObject bombObject in bombs)
        {
            var bomb = bombObject.GetComponent<Bomb>();
            if (bomb != null)
            {
                bomb.Detonate(bombDelay);
                bombDelay += 0.1f;
                if (bomb.exposionScreamSound != null)
                {
                    if (!bombScreamsToPlay.Any(x => x.name == bomb.exposionScreamSound.name))
                    {
                        bombScreamsToPlay.Add(bomb.exposionScreamSound);
                    }
                }
            }
        }
        if (audioSource != null)
        {
            audioSource.volume = UnityEngine.Random.Range(0.95f, 1.05f);
            audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        }
        foreach (var clip in bombScreamsToPlay)
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(clip);
            }

        }

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
                //print(body.gameObject.name + "   " + body.velocity.magnitude);
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
            inputAllowed = false;
            hud.LevelFinished(LevelClear.Failed, 0, 0);
            return;
        }
        else if (levelClear == LevelClear.NotCleared)
        {
            if (roundCounter == maxRounds || bombCount == 0)
            {
                //Fail if no bombs left or rounds left
                inputAllowed = false;
                hud.LevelFinished(LevelClear.Failed, 0, 0);
                return;
            }
            else
            {
                roundCounter++;
                hud.NextRound(roundCounter, roundDelay);
                StartCoroutine(AllowInput(true, roundDelay));
            }
        }
        else if (levelClear == LevelClear.OnePentagram || levelClear == LevelClear.TwoPentagram)
        {
            if (roundCounter == maxRounds || bombCount == 0)
            {
                //if out of rounds or bombs we finish level even with one or two pentas
                FinishLevel(levelClear);
            }
            else
            {
                roundCounter++;
                hud.NextRound(roundCounter, roundDelay);
                StartCoroutine(AllowInput(true, roundDelay));
            }
        }
        else
        {
            //three pentas always finish right away
            FinishLevel(levelClear);
        }
    }

    private void FinishLevel(LevelClear levelClear)
    {
        //Get the previous saved values for this level
        var levelName = SceneManager.GetActiveScene().name;
        var pentagrams = PlayerPrefs.GetInt(levelName + "Pentagrams", 0);
        var savedBombs = PlayerPrefs.GetInt(levelName + "SavedBombs", 0);
        var score = PlayerPrefs.GetInt(levelName + "Score", 0);


        //PlayerPentagrams and PlayerScore are the values of all gained pentagrams and score from all maps the player has cleared
        var playerPentagrams = PlayerPrefs.GetInt("PlayerPentagrams", 0);
        var playerScore = PlayerPrefs.GetInt("PlayerScore", 0);


        //Check if new clear is better than previous runs and save to player prefs
        //remove the previous clear value from playerpenta
        playerPentagrams -= pentagrams;
        if (levelClear == LevelClear.OnePentagram && pentagrams < 1)
        {
            pentagrams = 1;
        }
        else if (levelClear == LevelClear.TwoPentagram && pentagrams < 2)
        {
            pentagrams = 2;
        }
        else if (levelClear == LevelClear.ThreePentagram && pentagrams < 3)
        {
            pentagrams = 3;
        }
        //add the new value to playerpenta and save to playerprefs
        playerPentagrams += pentagrams;
        PlayerPrefs.SetInt("PlayerPentagrams", playerPentagrams);
        PlayerPrefs.SetInt(levelName + "Pentagrams", pentagrams);


        //check if we saved more bombs than on previous runs
        if (savedBombs < bombCount)
            PlayerPrefs.SetInt(levelName + "SavedBombs", bombCount);

        //If the new score is higher than previously, save the new score to player prefs
        if (score < levelScore)
        {
            // Reduce the old level score from the player score and add the new score back
            playerScore = playerScore - score + levelScore;
            PlayerPrefs.SetInt(levelName + "Score", levelScore);
            PlayerPrefs.SetInt("PlayerScore", playerScore);
        }

        var bonusSalvage = (bombCount - savedBombs) * bonusSalvageForSavedBomb;

        hud.LevelFinished(levelClear, salvageValue, bonusSalvage);
        //print(salvageValue + bonusSalvage);
        gameMaster.AddSalvage(salvageValue + bonusSalvage);
    }

    private LevelClear CheckLevelClear()
    {
        LevelClear levelClear = LevelClear.NotCleared;
        //bool buildingsDamaged = false;

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
            //if (building.Hitpoints < building.maxHitpoints)
            //{
            //    buildingsDamaged = true;
            //}
        }
        var buildingObjects = FindObjectsOfType<BuildingObject>().Where(x => x.checkedInLevelClear && x.gameObject.transform.position.magnitude < 100);
        if (buildingObjects.Count() == 0)
        {
            levelClear = LevelClear.ThreePentagram;
        }
        else
        {
            //Find the brick that has the highest position 
            var highestBuildingObject = buildingObjects.OrderByDescending(x => x.gameObject.transform.position.y + x.GetComponent<Collider2D>().bounds.extents.y).First();
            float highestBrickTopPos = highestBuildingObject.transform.position.y + highestBuildingObject.GetComponent<Collider2D>().bounds.extents.y;

            if (highestBrickTopPos <= winlines.threePentaHeight)
            {
                levelClear = LevelClear.ThreePentagram;
            }
            else if (highestBrickTopPos <= winlines.twoPentaHeight)
            {
                levelClear = LevelClear.TwoPentagram;
            }
            else if (highestBrickTopPos <= winlines.onePentaHeight)
            {
                levelClear = LevelClear.OnePentagram;
            }
            else
            {
                levelClear = LevelClear.NotCleared;
            }
        }
        //Damaged buildings lower the pentagrams gained by one
        //if (buildingsDamaged)
        //{
        //    if (levelClear == LevelClear.ThreePentagram)
        //        levelClear = LevelClear.TwoPentagram;
        //    else if (levelClear == LevelClear.TwoPentagram)
        //        levelClear = LevelClear.OnePentagram;
        //    else
        //        levelClear = LevelClear.NotCleared;
        //}

        return levelClear;
    }
    
    private IEnumerator AllowInput(bool allow, float delay)
    {
        yield return new WaitForSeconds(delay);
        inputAllowed = allow;
    }

    private void CheckScorelines()
    {
        winlines.UpdateLines(CheckLevelClear());
    }

}

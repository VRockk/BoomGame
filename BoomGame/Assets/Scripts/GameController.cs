using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



/// <summary>
/// Resposible for the ingame logic and input
/// </summary>
public class GameController : MonoBehaviour
{
    public int maxRounds = 2;
    public GameObject bomb;
    public string nextLevelName;

    [HideInInspector]
    public bool allowInput;

    private int roundCounter = 1;
    private int shatteringObjectCount;
    private IngameHUD hud;


    // Start is called before the first frame update
    void Start()
    {
        allowInput = true;
        roundCounter = 1;
        shatteringObjectCount = GameObject.FindGameObjectsWithTag("ShatteringObject").Length;
        hud = GameObject.FindObjectOfType<IngameHUD>();

        if (hud == null)
            Debug.LogError("No IngameHUD found in the scene for the GameController");
    }

    // Update is called once per frame
    void Update()
    {
        if (allowInput)
        {
            if (!UtilityLibrary.IsMouseOverUI())
            {
                //Left mouse click
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
                    Vector3 worldPos;
                    Ray ray = Camera.main.ScreenPointToRay(mousePos);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 1000f))
                    {
                        worldPos = hit.point;
                    }
                    else
                    {
                        worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                    }
                    worldPos.z = 0f;
                    Instantiate(bomb, worldPos, Quaternion.identity);
                }
            }
        }
    }

    public void WaitForNextRound()
    {
        allowInput = false;
        //gameController.NextRound();

        //Check for movement once a second
        InvokeRepeating("CheckForMovement", 1.0f, 1.0f);
    }

    private void CheckForMovement()
    {
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

        //If no movement, stop checking and start next round/next level if finished
        if (!isMovement)
        {
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
            if (obj.initialPosition == obj.gameObject.transform.position)
            {
                return LevelClear.NotCleared;
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

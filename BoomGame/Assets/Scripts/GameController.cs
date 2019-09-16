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

    private bool allowInput;
    private int roundCounter = 1;
    private int shatteringObjectCount;

    private bool waitForNextRound;

    // Start is called before the first frame update
    void Start()
    {
        waitForNextRound = false;
        allowInput = true;
        roundCounter = 1;
        shatteringObjectCount = GameObject.FindGameObjectsWithTag("ShatteringObject").Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowInput)
        {
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

        if (waitForNextRound)
        {
            var rigidBodies = GameObject.FindObjectsOfType<Rigidbody2D>();
            foreach (var body in rigidBodies)
            {
                if (body.velocity.magnitude > 0.5f)
                {
                    print(body.gameObject.name + "   " + body.velocity.magnitude);
                }
                else
                {
                    if (body.gameObject.tag == "ShatteringObject")
                    {
                        if(body.gameObject.GetComponent<ShatteringObject>().isGrounded)
                        {
                            //body.constraints = RigidbodyConstraints2D.FreezeAll;
                        }
                    }
                    else
                    {
                        //body.constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                }
            }
        }
    }

    public void WaitForNextRound()
    {
        allowInput = true;
        waitForNextRound = true;
        //gameController.NextRound();
    }

    public void NextRound()
    {
        roundCounter++;

        if (roundCounter > maxRounds)
        {
            // Show Next level/Reset buttons
        }
    }
}

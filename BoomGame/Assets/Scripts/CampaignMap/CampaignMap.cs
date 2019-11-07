using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CampaignMap : MonoBehaviour
{
    public GameObject chapterPanel;
    private bool inputAllowed = true;

    void Start()
    {
    }

    private void Update()
    {
        if (inputAllowed)
        {
            if (!UtilityLibrary.IsMouseOverUI())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    bool wasHit = false;
                    //is chapter clicked
                    Vector3 mousePos = UtilityLibrary.GetCurrentMousePosition();

                    // Check if clicking on a bomb and "attach" it to cursor
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
                        }
                    }
                    if (!wasHit)
                    {
                        HideChapterInfo();

                    }
                }
            }
        }
    }

    private void ShowChapterInfo(Chapter chapter)
    {
        if (!chapter.locked)
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
                chapterPanel.SetActive(true);
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
            chapterPanel.SetActive(false);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}


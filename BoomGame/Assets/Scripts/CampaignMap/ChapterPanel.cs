using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ChapterPanel : MonoBehaviour
{
    public GameObject chapterName;
    public GameObject levelInfoPanelPrefab;
    private List<Level> levels;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateLevelInfoPanels(Chapter chapter)
    {
        if (chapter == null || chapter.levels == null)
            return;

        foreach(var levelPanel in this.transform.GetComponentsInChildren<LevelInfoPanel>())
        {
            //remove old panels
            Destroy(levelPanel.gameObject);
        }
        levels = new List<Level>();
        int count = 0;
        //Create new info panel for each level and set position. Then set the level infor inside Level Sc´ript
        foreach (var level in chapter.levels)
        {
            var levelInfoPanel = Instantiate(levelInfoPanelPrefab);
            var transform = levelInfoPanel.GetComponent<RectTransform>();
            transform.SetParent(this.transform);
            //transform.position = new Vector3(transform.position.x, , 0f);
            transform.SetLeft(25f);
            transform.SetRight(25f);

            transform.sizeDelta = new Vector2(transform.sizeDelta.x, 150f);
            Vector3 pos = transform.anchoredPosition;
            pos.y = -50f + (count * -175f);
            transform.anchoredPosition = pos;

            //transform.offsetMin = new Vector2(0f, -50f + (count * -175f));
            //transform.offsetMax = new Vector2(0f, -50f + (count * -175f));
            //transform.localPosition = new Vector3(0f, , 0);
            transform.localScale = new Vector3(1, 1, 1);


            var levelInfoPanelScript = levelInfoPanel.GetComponent<LevelInfoPanel>();
            var levelScript = level.GetComponent<Level>();
            count++;
            levelInfoPanelScript.SetLevelInfo(levelScript, count);
            levels.Add(levelScript);
        }

        var chapterNameText = chapterName.GetComponent<TextMeshProUGUI>();
        if(chapterNameText != null)
        {
            chapterNameText.text = chapter.chapterName;
        }
    }

    public void OpenLevel(string name)
    {
        var gameMaster = FindObjectOfType<GameMaster>();
        gameMaster.currentChapterLevels = levels;
        SceneManager.LoadScene(name);

    }
}

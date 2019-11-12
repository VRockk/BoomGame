using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ChapterPanel : MonoBehaviour
{
    public GameObject chapterName;
    public GameObject levelInfoPanelPrefab;
    public GameObject levelPanel;

    private Chapter chapter;
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
        if (chapter == null || chapter.levelNames == null || chapter.levelNames.Count == 0 || levelPanel == null)
            return;

        this.chapter = chapter;
        foreach (var levelInfoPanel in levelPanel.transform.GetComponentsInChildren<LevelInfoPanel>())
        {
            //remove old panels
            Destroy(levelInfoPanel.gameObject);
        }

        int count = 0;
        //Create new info panel for each level and set position. Then set the level infor inside Level Sc´ript
        foreach (var level in chapter.chapterLevels)
        {
            var levelInfoPanel = Instantiate(levelInfoPanelPrefab);
            var transform = levelInfoPanel.GetComponent<RectTransform>();
            transform.SetParent(levelPanel.transform);
            transform.localScale = new Vector3(1, 1, 1);

            var levelInfoPanelScript = levelInfoPanel.GetComponent<LevelInfoPanel>();
            count++;
            levelInfoPanelScript.SetLevelInfo(level, count);
        }

        var chapterNameText = chapterName.GetComponent<TextMeshProUGUI>();
        if (chapterNameText != null)
        {
            chapterNameText.text = chapter.chapterName;
        }
    }

    public void OpenLevel(string name)
    {
        var gameMaster = FindObjectOfType<GameMaster>();
        //gameMaster.currentChapterLevels = new List<Level>();
        gameMaster.currentChapterLevels = this.chapter.chapterLevels;
        SceneManager.LoadScene(name);
    }
}

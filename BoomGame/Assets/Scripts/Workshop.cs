using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Workshop : MonoBehaviour
{
    private string spritesPath = "Bombs/";

    public GameObject upgradePanel;
    public GameObject upgradeClipboard;


    public TextMeshProUGUI bombName;
    public TextMeshProUGUI bombLevel;
    public Image bombIcon;
    public TextMeshProUGUI blastRadiusCurrent;
    public TextMeshProUGUI blastRadiusNext;
    public TextMeshProUGUI powerCurrent;
    public TextMeshProUGUI powerNext;
    public TextMeshProUGUI cost;
    public GameObject classified;
    public Button upgradeButton;

    private BombType currentBombType;
    private int nextCost;


    private void SetUpgradePanelData(BombType bombType)
    {
        var gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null)
            return;

        currentBombType = bombType;

        //Get the data for the current bomb type from gamemaster
        var bombData = gameMaster.bombData.First(x => x.BombType == currentBombType);
        if (bombData == null)
            return;

        Sprite image = Resources.Load<Sprite>(spritesPath + bombData.SpritePath);

        if (bombIcon != null)
            bombIcon.sprite = image;
        if (bombName != null)
            bombName.text = bombData.Name;
        if (bombLevel != null)
            bombLevel.text = bombData.Level.ToString();
        if (blastRadiusCurrent != null)
            blastRadiusCurrent.text = "+" + ((5 * bombData.Level) - 5).ToString() + "%";
        if (blastRadiusNext != null)
            blastRadiusNext.text = "+" + ((5 * (bombData.Level + 1)) - 5).ToString() + "%";
        if (powerCurrent != null)
            powerCurrent.text = "+" + ((5 * bombData.Level) - 5).ToString() + "%";
        if (powerNext != null)
            powerNext.text = "+" + ((5 * (bombData.Level + 1)) - 5).ToString() + "%";
        nextCost = bombData.Level * 100 + 100;
        if (cost != null)
            cost.text = nextCost.ToString();

        if (bombData.Locked)
        {
            classified.SetActive(true);
            upgradeButton.interactable = false;
            var buttonText = upgradeButton.GetComponentInChildren<TextMeshProUGUI>();
            var color = buttonText.color;
            color.a = 0.3f;
            buttonText.color = color;
        }
        else
        {
            classified.SetActive(false);
            if (gameMaster.currentSalvage >= nextCost)
            {
                upgradeButton.interactable = true;
                var buttonText = upgradeButton.GetComponentInChildren<TextMeshProUGUI>();
                var color = buttonText.color;
                color.a = 1f;
                buttonText.color = color;
            }
            else
            {
                upgradeButton.interactable = false;
                var buttonText = upgradeButton.GetComponentInChildren<TextMeshProUGUI>();
                var color = buttonText.color;
                color.a = 0.3f;
                buttonText.color = color;
            }
        }


    }


    private bool upgradePanelVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        SetUpgradePanelData(BombType.Regular);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (upgradePanelVisible)
            {
                var eventData = new PointerEventData(EventSystem.current);
                eventData.position = Input.mousePosition;
                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                //hide the upgrade panel when clicked elsewheres
                bool upgradeClicked = false;
                foreach (var result in results)
                {
                    if (result.gameObject.tag == "Upgrades")
                    {
                        upgradeClicked = true;
                    }

                }
                if (!upgradeClicked)
                    HideUpgadePanel();
            }
        }

    }

    public void ShowUpgadePanel()
    {
        if (upgradePanel != null && upgradeClipboard != null && !upgradePanelVisible)
        {
            var upgradePanelBG = upgradePanel.GetComponent<Image>();
            var canvasGroup = upgradePanel.GetComponent<CanvasGroup>();
            upgradePanelVisible = true;
            upgradePanelBG.DOColor(new Color(0, 0, 0, 0.7f), 0.7f);
            canvasGroup.blocksRaycasts = true;
            upgradeClipboard.GetComponent<RectTransform>().DOAnchorPosY(0f, Random.Range(0.45f, 0.55f), false).SetEase(Ease.OutBack).SetUpdate(true);
        }
    }

    public void HideUpgadePanel()
    {
        if (upgradePanel != null && upgradeClipboard != null && upgradePanelVisible)
        {
            var upgradePanelBG = upgradePanel.GetComponent<Image>();
            var canvasGroup = upgradePanel.GetComponent<CanvasGroup>();
            upgradePanelVisible = false;
            upgradePanelBG.DOColor(new Color(0, 0, 0, 0f), 1f);
            canvasGroup.blocksRaycasts = false;
            upgradeClipboard.GetComponent<RectTransform>().DOAnchorPosY(-1100f, Random.Range(0.45f, 0.55f), false).SetEase(Ease.InBack).SetUpdate(true);
        }
    }

    public void PrevBombPanel()
    {
        var gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null)
            return;

        int index = gameMaster.bombData.FindIndex(x => x.BombType == currentBombType);
        if (index == 0)
        {
            //current is first, get the last one
            index = gameMaster.bombData.Count - 1;
        }
        else
        {
            index--;
        }

        SetUpgradePanelData(gameMaster.bombData[index].BombType);
    }

    public void NextBombPanel()
    {
        var gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null || gameMaster.bombData == null)
            return;


        int index = gameMaster.bombData.FindIndex(x => x.BombType == currentBombType);

        if (index == gameMaster.bombData.Count - 1)
        {
            //Current is last, get the first
            index = 0;
        }
        else
        {
            index++;
        }

        SetUpgradePanelData(gameMaster.bombData[index].BombType);

    }

    public void UpdadeBomb()
    {
        var gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null)
            return;

        if (gameMaster.currentSalvage >= nextCost)
        {
            var bomb = gameMaster.bombData.First(x => x.BombType == currentBombType);
            bomb.Level++;
            PlayerPrefs.SetInt(bomb.BombType.ToString() + "Level", bomb.Level);
            gameMaster.AddSalvage(-nextCost);
            SetUpgradePanelData(currentBombType);

            var mainmenu = FindObjectOfType<MainMenu>();
            if (mainmenu != null)
                mainmenu.UpdateSalvage();

        }
    }
}

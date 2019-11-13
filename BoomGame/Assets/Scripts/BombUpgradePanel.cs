using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BombUpgradePanel : MonoBehaviour
{
    public GameObject upgrade1;
    public GameObject upgrade2;
    public GameObject upgrade3;
    public GameObject upgrade4;
    public GameObject upgradePanel;
    public GameObject bombSelectionPanel;
    public GameObject upgradeButton;
    public BombType bombType;

    private GameMaster gameMaster;
    private BombData bombData;
    private GameObject selectedUpgrade;
    private MainMenu mainMenu;

    public Sprite lvlUnlockedIcon;

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null)
            Debug.LogError("No GameMaster found in bomb upgrade panel");

        SetBombUpgradeLevels();

        mainMenu = GetComponentInParent<MainMenu>();

        upgradePanel.SetActive(false);

    }

    private void SetBombUpgradeLevels()
    {
        if (bombType == BombType.Regular)
        {
            bombData = gameMaster.regularBombData;
        }
        else if (bombType == BombType.Acid)
        {
            bombData = gameMaster.acidBombData;
        }

        var upgrade1Lvl = bombData.BombUpgradeLevels[0];
        var upgrade2Lvl = bombData.BombUpgradeLevels[1];
        var upgrade3Lvl = bombData.BombUpgradeLevels[2];
        var upgrade4Lvl = bombData.BombUpgradeLevels[3];

        var upgrade1Info = upgrade1.GetComponent<BombUpgradeInfo>();
        var upgrade2Info = upgrade2.GetComponent<BombUpgradeInfo>();
        var upgrade3Info = upgrade3.GetComponent<BombUpgradeInfo>();
        var upgrade4Info = upgrade4.GetComponent<BombUpgradeInfo>();
        upgrade1Info.level = upgrade1Lvl;
        upgrade2Info.level = upgrade2Lvl;
        upgrade3Info.level = upgrade3Lvl;
        upgrade4Info.level = upgrade4Lvl;
        upgrade1Info.upgradePosition = 1;
        upgrade2Info.upgradePosition = 2;
        upgrade3Info.upgradePosition = 3;
        upgrade4Info.upgradePosition = 4;


        var upgrade1Lvl1Icon = upgrade1.transform.Find("Lvl1").gameObject.GetComponent<Image>();
        var upgrade1Lvl2Icon = upgrade1.transform.Find("Lvl2").gameObject.GetComponent<Image>();
        var upgrade1Lvl3Icon = upgrade1.transform.Find("Lvl3").gameObject.GetComponent<Image>();

        if (upgrade1Lvl >= 1)
        {
            upgrade1Lvl1Icon.sprite = lvlUnlockedIcon;
        }
        if (upgrade1Lvl >= 2)
        {
            upgrade1Lvl2Icon.sprite = lvlUnlockedIcon;
        }
        if (upgrade1Lvl >= 3)
        {
            upgrade1Lvl3Icon.sprite = lvlUnlockedIcon;
        }


        var upgrade2Lvl1Icon = upgrade2.transform.Find("Lvl1").gameObject.GetComponent<Image>();
        var upgrade2Lvl2Icon = upgrade2.transform.Find("Lvl2").gameObject.GetComponent<Image>();
        var upgrade2Lvl3Icon = upgrade2.transform.Find("Lvl3").gameObject.GetComponent<Image>();
        if (upgrade2Lvl >= 1)
        {
            upgrade2Lvl1Icon.sprite = lvlUnlockedIcon;
        }
        if (upgrade2Lvl >= 2)
        {
            upgrade2Lvl2Icon.sprite = lvlUnlockedIcon;
        }
        if (upgrade2Lvl >= 3)
        {
            upgrade2Lvl3Icon.sprite = lvlUnlockedIcon;
        }

        var upgrade3Lvl1Icon = upgrade3.transform.Find("Lvl1").gameObject.GetComponent<Image>();
        var upgrade3Lvl2Icon = upgrade3.transform.Find("Lvl2").gameObject.GetComponent<Image>();
        var upgrade3Lvl3Icon = upgrade3.transform.Find("Lvl3").gameObject.GetComponent<Image>();
        if (upgrade3Lvl >= 1)
        {
            upgrade2Lvl1Icon.sprite = lvlUnlockedIcon;
        }
        if (upgrade3Lvl >= 2)
        {
            upgrade3Lvl2Icon.sprite = lvlUnlockedIcon;
        }
        if (upgrade3Lvl >= 3)
        {
            upgrade3Lvl3Icon.sprite = lvlUnlockedIcon;
        }


        var upgrade4Lvl1Icon = upgrade4.transform.Find("Lvl1").gameObject.GetComponent<Image>();
        var upgrade4Lvl2Icon = upgrade4.transform.Find("Lvl2").gameObject.GetComponent<Image>();
        var upgrade4Lvl3Icon = upgrade4.transform.Find("Lvl3").gameObject.GetComponent<Image>();
        if (upgrade4Lvl >= 1)
        {
            upgrade4Lvl1Icon.sprite = lvlUnlockedIcon;
        }
        if (upgrade4Lvl >= 2)
        {
            upgrade4Lvl2Icon.sprite = lvlUnlockedIcon;
        }
        if (upgrade4Lvl >= 3)
        {
            upgrade4Lvl3Icon.sprite = lvlUnlockedIcon;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectUpgrade(GameObject selectedUpgrade)
    {
        upgradePanel.SetActive(true);
        this.selectedUpgrade = selectedUpgrade;

        var bombUpgradeInfo = selectedUpgrade.GetComponent<BombUpgradeInfo>();
        var upgradeHeader = upgradePanel.transform.Find("UpgradeHeader").gameObject.GetComponent<TextMeshProUGUI>();
        var upgradeDescription = upgradePanel.transform.Find("UpgradeDescription").gameObject.GetComponent<TextMeshProUGUI>();
        var upgradeCost = upgradePanel.transform.Find("Cost");
        var salvageIcon = upgradePanel.transform.Find("SalvageIcon");
        var upgradeCostAmount = upgradePanel.transform.Find("CostAmount").gameObject.GetComponent<TextMeshProUGUI>();
        var button = upgradeButton.GetComponent<Button>();
        var buttonText = upgradeButton.GetComponentInChildren<TextMeshProUGUI>();

        var upgradeCostValue = CalculateUpgradeCost(bombUpgradeInfo.level);
        upgradeHeader.text = bombUpgradeInfo.upgradeName;
        upgradeDescription.text = bombUpgradeInfo.description;

        if (bombUpgradeInfo.level < bombUpgradeInfo.maxLevel)
        {
            upgradeCost.gameObject.SetActive(true);
            salvageIcon.gameObject.SetActive(true);
            upgradeCostAmount.gameObject.SetActive(true);
            upgradeButton.SetActive(true);

            upgradeCostAmount.text = CalculateUpgradeCost(bombUpgradeInfo.level).ToString();

            if (gameMaster.currentSalvage >= upgradeCostValue)
            {
                button.interactable = true;
                buttonText.color = new Color(1, 1, 1, 1);
            }
            else
            {
                button.interactable = false;
                buttonText.color = new Color(1, 1, 1, 0.33f);
            }

        }
        else
        {
            upgradeCost.gameObject.SetActive(false);
            salvageIcon.gameObject.SetActive(false);
            upgradeCostAmount.gameObject.SetActive(false);
            upgradeButton.SetActive(false);

            //TODO Show "max level" text or something
        }

        //TODO Visuals for selected upgrade
    }

    private int CalculateUpgradeCost(int currentLevel)
    {

        return 250 + (currentLevel * 250);
    }


    public void UpgradeSkill()
    {
        var bombUpgradeInfo = selectedUpgrade.GetComponent<BombUpgradeInfo>();

        var upgradeCostValue = CalculateUpgradeCost(bombUpgradeInfo.level);

        if (gameMaster.currentSalvage < upgradeCostValue)
        {
            return;
        }
        if (bombUpgradeInfo.level >= bombUpgradeInfo.maxLevel)
        {
            return;
        }

        //bombUpgradeInfo.level++;
        //bombUpgradeInfo.upgradePosition;
        bombUpgradeInfo.level++;
        gameMaster.SetBombUpgradeLevel(bombType, bombUpgradeInfo.upgradePosition, bombUpgradeInfo.level);
        gameMaster.AddSalvage(-upgradeCostValue);
        mainMenu.UpdateSalvage();
        //Updates the values
        SetBombUpgradeLevels();
        SelectUpgrade(selectedUpgrade);
    }

    public void Back()
    {
        selectedUpgrade = null;
        upgradePanel.SetActive(false);
        bombSelectionPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}

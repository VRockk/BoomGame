using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workshop : MonoBehaviour
{

    public GameObject upgradePanel;

    private bool upgradePanelVisible = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePos = UtilityLibrary.GetCurrentMousePosition();

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10f));
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag != "Upgrades")
                {
                    HideUpgadePanel();
                }
            }
        }

    }

    public void ShowUpgadePanel()
    {
        if (upgradePanel != null && !upgradePanelVisible)
        {
            upgradePanelVisible = true;
            upgradePanel.GetComponent<RectTransform>().DOAnchorPosY(0f, Random.Range(0.55f, 0.65f), false).SetEase(Ease.OutBack).SetUpdate(true);
        }
    }

    public void HideUpgadePanel()
    {
        if (upgradePanel != null && upgradePanelVisible)
        {
            upgradePanelVisible = false;
            upgradePanel.GetComponent<RectTransform>().DOAnchorPosY(-1100f, Random.Range(0.55f, 0.65f), false).SetEase(Ease.InBack).SetUpdate(true);
        }
    }
}

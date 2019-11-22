using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Tutorial : MonoBehaviour
{
    Animator animator;
    Sequence bombIconScale = null;
    Sequence detonatorScale = null;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartTutorial()
    {
        ShowDragAndDrop();
    }

    public void ShowDragAndDrop()
    {
        animator.SetInteger("state", 1);
        var tutorialGuy = GameObject.Find("TutorialGuy").GetComponent<RectTransform>();
        tutorialGuy.DOAnchorPosX(-10f, 0.5f, false).SetEase(Ease.OutBack).SetUpdate(true);

        var transform = GameObject.Find("BombCardIcon").GetComponent<RectTransform>();
        bombIconScale = DOTween.Sequence().Append(transform.DOScale(1.35f, 0.5f)).SetLoops(-1, LoopType.Yoyo);

        var text = GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>();
        text.text = "Drag and drop the bombs on to the level.";
    }



    public void ShowDetonator()
    {
        animator.SetInteger("state", 2);
        bombIconScale.Kill();

        var transform = GameObject.Find("DetonatePanel").GetComponent<RectTransform>();
        detonatorScale = DOTween.Sequence().Append(transform.DOScale(1.15f, 0.5f)).SetLoops(-1, LoopType.Yoyo);

        var text = GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>();
        text.text = "Press the detonator to make things go Boom!";
    }

    public void EndTutorial()
    {
        animator.SetInteger("state", 0);
        detonatorScale.Kill();
        var tutorialGuy = GameObject.Find("TutorialGuy").GetComponent<RectTransform>();
        tutorialGuy.DOAnchorPosX(800f, 0.5f, false).SetEase(Ease.InBack).SetUpdate(true).OnComplete(RemoveTutorial);

    }

    public void RemoveTutorial()
    {
        PlayerPrefs.SetInt("TutorialPassed", 1);
        Destroy(gameObject);
    }
}

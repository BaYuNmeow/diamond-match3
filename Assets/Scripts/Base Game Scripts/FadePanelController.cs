using UnityEngine;
using DG.Tweening;
using System.Collections;

public class FadePanelController : MonoBehaviour
{
    [Header("Основная панель")]
    public CanvasGroup fadePanel;             

    [Header("Дочерние панели")]
    public CanvasGroup gameIntroPanel;       
    public CanvasGroup tryAgainPanel;        
    public CanvasGroup youWinPanel;          

    public Board board; 

    private void Start()
    {
        ShowPanel(fadePanel);
        ShowPanel(gameIntroPanel);

        HidePanel(tryAgainPanel);
        HidePanel(youWinPanel);
    }

    public void ShowPanel(CanvasGroup panel, float duration = 0.5f)
    {
        panel.gameObject.SetActive(true);
        panel.DOKill();
        panel.interactable = true;
        panel.blocksRaycasts = true;
        panel.DOFade(1f, duration);
    }

    public void HidePanel(CanvasGroup panel, float duration = 0.5f)
    {
        panel.DOKill();
        panel.interactable = false;
        panel.blocksRaycasts = false;
        panel.DOFade(0f, duration).OnComplete(() => panel.gameObject.SetActive(false));
    }

    public void StartGame()
    {
        StartCoroutine(HideGameIntro());
    }

    private IEnumerator HideGameIntro()
    {
        yield return HidePanelCoroutine(gameIntroPanel, 0.5f);

        yield return HidePanelCoroutine(fadePanel, 0.5f);

        board.currentState = GameState.Move;
    }

    private IEnumerator HidePanelCoroutine(CanvasGroup panel, float duration)
    {
        panel.DOKill();
        panel.DOFade(0f, duration);

        yield return new WaitForSeconds(duration);

        panel.interactable = false;
        panel.blocksRaycasts = false;
        panel.gameObject.SetActive(false);
    }

    public void ShowTryAgain()
    {
        ShowPanel(fadePanel);
        ShowPanel(tryAgainPanel);
        HidePanel(gameIntroPanel);
        HidePanel(youWinPanel);
    }

    public void ShowYouWin()
    {
        ShowPanel(fadePanel);
        ShowPanel(youWinPanel);
        HidePanel(gameIntroPanel);
        HidePanel(tryAgainPanel);
    }

    public void GameOver(bool isWin)
    {
        HidePanel(fadePanel);
        if (isWin)
        {
            ShowYouWin();
        }
        else
        {
            ShowTryAgain();
        }
    }
    private void OnDestroy()
    {
        fadePanel.DOKill();
        gameIntroPanel.DOKill();
        tryAgainPanel.DOKill();
        youWinPanel.DOKill();
    }

}

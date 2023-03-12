using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class GameOverScreen : MonoBehaviour
{
    private CanvasGroup gameoverScreen;
    public CanvasGroup retryButton;

    public Button PauseButton;

    void Awake()
    {
        gameoverScreen = GetComponent<CanvasGroup>();
        gameoverScreen.interactable = false;
        gameoverScreen.alpha = 0f;
    }

    public void Show()
    {
        gameoverScreen.interactable = true;

        gameoverScreen.DOKill();
        gameoverScreen.alpha = 0f;
        gameoverScreen.DOFade(1f, 1f).SetEase(Ease.OutQuad);

        retryButton.interactable = false;
        retryButton.DOKill();
        retryButton.alpha = 0f;
        retryButton.DOFade(1f, 0.5f).SetEase(Ease.OutQuad).SetDelay(0.5f)
            .OnComplete(() => { retryButton.interactable = true; } );

        if (PauseButton != null) 
            PauseButton.gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameoverScreen.interactable = false;

        gameoverScreen.DOKill();
        gameoverScreen.alpha = 1f;
        gameoverScreen.DOFade(0f, 0.5f).SetEase(Ease.OutSine)
            .OnComplete(() => { gameObject.SetActive(false); });

        if (PauseButton != null) 
            PauseButton.gameObject.SetActive(true);
    }
}

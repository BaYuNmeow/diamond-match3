using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private Board board;
    public bool paused = false;

    [SerializeField] private float animationDuration = .5f;
    [SerializeField] private Ease easeType = Ease.OutBack;
    [SerializeField] private RectTransform pausePanel;
    [SerializeField] private Image soundButton;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;
    private SoundManager sound;

    private Vector2 _hiddenPosition;
    private Vector2 _shownPosition;

    void Start()
    {
        sound = FindAnyObjectByType<SoundManager>();
        board = GameObject.FindWithTag("Board").GetComponent<Board>();

        // Инициализация soundButton
        if (PlayerPrefs.HasKey("Sound"))
        {
            soundButton.sprite = PlayerPrefs.GetInt("Sound") == 0 ? musicOffSprite : musicOnSprite;
        }
        else
        {
            soundButton.sprite = musicOnSprite;
        }

        if (pausePanel == null)
            pausePanel = GetComponent<RectTransform>();

        _shownPosition = pausePanel.anchoredPosition;
        _hiddenPosition = new Vector2(_shownPosition.x, _shownPosition.y + (Screen.height / 2) + (pausePanel.rect.height / 2) + 50f);

        pausePanel.anchoredPosition = _hiddenPosition;
        pausePanel.gameObject.SetActive(false);
    }

    public void ToggleMenu()
    {
        if (paused)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        paused = true;
        board.currentState = GameState.Pause;
        pausePanel.DOAnchorPos(_shownPosition, animationDuration)
            .SetEase(easeType)
            .OnStart(() => pausePanel.gameObject.SetActive(true));
    }

    private void CloseMenu()
    {
        paused = false;
        board.currentState = GameState.Move;
        pausePanel.DOAnchorPos(_hiddenPosition, animationDuration)
            .SetEase(easeType)
            .OnComplete(() => pausePanel.gameObject.SetActive(false));

    }

    public void SoundButton()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                soundButton.sprite = musicOnSprite;
                PlayerPrefs.SetInt("Sound", 1);
                sound.AdjustVolume();
            }
            else
            {
                soundButton.sprite = musicOffSprite;
                PlayerPrefs.SetInt("Sound", 0);
                sound.AdjustVolume();
            }
        }
        else
        {
            soundButton.sprite = musicOnSprite;
            PlayerPrefs.SetInt("Sound", 1);
            sound.AdjustVolume();
        }
    }

    public void ExitGame()
    {
        HintManager hintManager = FindAnyObjectByType<HintManager>();
        if (hintManager != null)
        {
            hintManager.DestroyHint();
        }

        SceneManager.LoadScene("Splash");
    }

    private void OnDestroy()
    {
        DOTween.Kill(gameObject);
    }
}
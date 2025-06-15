using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private Button startButton;

    [Header("Настройки анимации")]
    [SerializeField] private float pulseScale = 1.2f;
    [SerializeField] private float pulseDuration = 0.5f;
    [SerializeField] private Ease easeType = Ease.InOutSine;
    [SerializeField] private int loops = -1;
    [SerializeField] private LoopType loopType = LoopType.Yoyo;

    private RectTransform _buttonRectTransform;
    private Vector3 _originalScale;

    private void Awake()
    {

        if (startButton == null)
        {
            return;
        }

        _buttonRectTransform = startButton.GetComponent<RectTransform>();
        if (_buttonRectTransform == null)
        {
            return;
        }

        _originalScale = _buttonRectTransform.localScale;
    }

    private void Start()
    {
        if (_buttonRectTransform != null)
        {
            StartPulse();
        }
        startPanel.SetActive(true);
        levelPanel.SetActive(false);
    }

    public void PlayGame()
    {
        startPanel.SetActive(false);
        levelPanel.SetActive(true);
        StopPulse();
    }

    public void Home()
    {
        startPanel.SetActive(true);
        levelPanel.SetActive(false);
        StartPulse();
    }

    public void StartPulse()
    {
        if (_buttonRectTransform == null) return;

        _buttonRectTransform.DOKill();
        _buttonRectTransform.DOScale(_originalScale * pulseScale, pulseDuration)
            .SetEase(easeType)
            .SetLoops(loops, loopType)
            .OnKill(() =>
            {
                if (_buttonRectTransform != null)
                {
                    _buttonRectTransform.localScale = _originalScale;
                }
            });
    }

    public void StopPulse()
    {
        if (_buttonRectTransform == null) return;
        {
            _buttonRectTransform.DOKill();
            _buttonRectTransform.localScale = _originalScale;
        }
    }

    private void OnDestroy()
    {
        if (_buttonRectTransform != null)
        {
            _buttonRectTransform.DOKill();
        }
    }
}

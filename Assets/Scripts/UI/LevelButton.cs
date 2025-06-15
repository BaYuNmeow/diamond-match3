using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [Header("Acrive Stuff")]
    public bool isActive;
    public Sprite activeSprite;
    public Sprite lockedSprite;
    private Image buttonImage;
    private Button myButton;
    private int starsActive;

    [Header("Level UI")]
    public Image[] stars;
    public TMP_Text levelText;
    public int level;
    public GameObject confirmPanel;

    private GameData gameData;

    void Start()
    {
        gameData = GameData.Instance;
        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        LoadData();
        ActivateStars();
        ShowLevel();
        DecideSprite();
    }

    void LoadData()
    {
        if (gameData != null)
        {
            int index = level - 1;
            if (index >= 0 && index < gameData.saveData.isActive.Length)
            {
                isActive = gameData.saveData.isActive[index];
                starsActive = gameData.saveData.stars[index];
            }
            else
            {
                isActive = false;
                starsActive = 0;
            }
        }
    }

    void ActivateStars()
    {
        for (int i = 0; i < starsActive; i++)
        {

            stars[i].enabled = true;
        }
    }

    void DecideSprite()
    {
        if(isActive)
        {
            buttonImage.sprite = activeSprite;
            myButton.interactable = true;
            levelText.enabled = true;
        }
        else
        {
            buttonImage.sprite = lockedSprite;
            myButton.interactable = false;
            levelText.enabled=false;
        }
    }

    void ShowLevel()
    {
        levelText.text = level.ToString();
    }

    public void ConfirmPanel(int level)
    {
        if (confirmPanel != null)
        {
            var panel = confirmPanel.GetComponent<ConfirmPanel>();
            if (panel != null)
            {
                panel.level = level;
                confirmPanel.SetActive(true);
            }
        }
    }

}

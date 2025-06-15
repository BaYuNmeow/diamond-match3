using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ConfirmPanel : MonoBehaviour
{
    [Header("Level Information")]
    public string levelToLoad;
    public int level;
    private GameData gameData;
    private int starsActive;
    private int highScore;


    [Header("UI Stuff")]
    public Image[] stars;
    public TMP_Text highScoreText;
    public TMP_Text starText;
    

    private void OnEnable()
    {
        gameData = FindAnyObjectByType<GameData>();
        LoadData();
        ActivateStars();
        SetText();
    }

    void LoadData()
    {
        if (gameData != null && level - 1 >= 0 && level - 1 < gameData.saveData.stars.Length)
        {
            starsActive = gameData.saveData.stars[level - 1];
            highScore = gameData.saveData.highScores[level - 1];
        }
        else
        {
            starsActive = 0;
            highScore = 0;
        }
    }


    void SetText()
    {
        highScoreText.text = "" + highScore;
        starText.text = "" + starsActive + "/3";
    }

    void ActivateStars()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = false;
        }

        for (int i = 0; i < starsActive; i++)
        {
            stars[i].enabled = true;
        }
    }

    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }

    public void Play()
    {
        PlayerPrefs.SetInt("Current Level", level - 1);
        SceneManager.LoadScene("Main");
    }
}

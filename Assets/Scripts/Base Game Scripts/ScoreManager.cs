using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private Board board;
    public TMP_Text scoreText;
    public int score;
    public Image scoreBar;
    private GameData gameData;
    private int numberStars;

    void Start()
    {
        gameData = FindAnyObjectByType<GameData>();
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
        UpdateBar();
    }

    public void IncreaseScore(int amountToIncrease)
    {
        score += amountToIncrease;

        int stars = 0;
        for (int i = 0; i < board.scoreGoals.Length; i++)
        {
            if (score >= board.scoreGoals[i])
                stars = i + 1;
        }
        numberStars = stars;

        if (gameData != null)
        {
            int highScore = gameData.saveData.highScores[board.level];
            int prevStars = gameData.saveData.stars[board.level];
            bool scoreUpdated = false;
            bool starsUpdated = false;

            if (score > highScore)
            {
                gameData.saveData.highScores[board.level] = score;
                scoreUpdated = true;
            }
            if (numberStars > prevStars)
            {
                gameData.saveData.stars[board.level] = numberStars;
                starsUpdated = true;
            }
            if (scoreUpdated || starsUpdated)
                gameData.Save();
        }

        UpdateUI();
        UpdateBar();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && gameData != null)
        {
            gameData.saveData.stars[board.level] = numberStars;
            gameData.Save();
        }
    }

    private void UpdateBar()
    {
        if (board != null && scoreBar != null && board.scoreGoals != null && board.scoreGoals.Length > 0)
        {
            int maxGoal = board.scoreGoals[board.scoreGoals.Length - 1];
            scoreBar.fillAmount = Mathf.Clamp01((float)score / maxGoal);
        }
    }

}

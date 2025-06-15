using UnityEngine;
using TMPro;

public enum GameType
{
    Moves,
    Time
}

[System.Serializable]
public class EndGameRequirements
{
    public GameType gameType;
    public int counterValue;
}

public class EndGameManager : MonoBehaviour
{
    public GameObject movesLabel;
    public GameObject timeLabel;
    public GameObject youWinPanel;
    public GameObject tryAgainPanel;
    public TMP_Text counter;
    public EndGameRequirements requirements;
    public int currentCounterValue;
    private Board board;
    private float timerSeconds;

    void Start()
    {
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
        SetGameType();
        SetupGame();
    }

    void SetGameType()
    {
        if (board.world != null)
        {
            if (board.level < board.world.levels.Length)
            {
                if (board.world.levels[board.level] != null)
                {
                    requirements = board.world.levels[board.level].endGameRequirements;
                }
            }
        }
    }

    void SetupGame()
    {

        currentCounterValue = requirements.counterValue;
        if (requirements.gameType == GameType.Moves)
        {
            movesLabel.SetActive(true);
            timeLabel.SetActive(false);
        }
        else
        {
            timerSeconds = 1;
            movesLabel.SetActive(false);
            timeLabel.SetActive(true);
        }
        UpdateCounterText();
    }

    public void DecreaseCounterValue()
    {
        if (board.currentState != GameState.Pause)
        {
            currentCounterValue--;
            UpdateCounterText();
            if (currentCounterValue <= 0)
            {
                LoseGame();
            }
        }
    }
    void UpdateCounterText()
    {
        counter.text = currentCounterValue.ToString();
    }

    public void WinGame()
    {
        youWinPanel.SetActive(true);
        board.currentState = GameState.Win;
        currentCounterValue = 0;
        counter.text = currentCounterValue.ToString();
        FadePanelController fade = FindAnyObjectByType<FadePanelController>();
        fade.GameOver(true);
    }

    public void LoseGame()
    {
        tryAgainPanel.SetActive(true);
        board.currentState = GameState.Lose;
        currentCounterValue = 0;
        counter.text = currentCounterValue.ToString();
        FadePanelController fade = FindAnyObjectByType<FadePanelController>();
        fade.GameOver(false);
    }

    void Update()
    {
        if (requirements.gameType == GameType.Time && currentCounterValue > 0)
        {
            timerSeconds -= Time.deltaTime;
            if (timerSeconds <= 0)
            {
                DecreaseCounterValue();
                timerSeconds = 1;
            }
        }
    }
}

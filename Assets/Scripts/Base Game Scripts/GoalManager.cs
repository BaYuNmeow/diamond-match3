using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlankGoal
{
    public int numberNeeded;
    [HideInInspector]
    public int numberCollected;
    public Sprite goalSprite;
    public string matchValue;
    public int pointsPerGoal;
}

public class GoalManager : MonoBehaviour
{
    public BlankGoal[] levelGoals;
    public List<GoalPanel> currentGoals = new List<GoalPanel>();
    public GameObject goalPrefab;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;

    private Board board;
    private EndGameManager endGame;
    public ScoreManager scoreManager;

    private Dictionary<string, BlankGoal> goalsDictionary;

    private void Start()
    {
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
        endGame = FindAnyObjectByType<EndGameManager>();

        if (scoreManager == null)
        {
            scoreManager = FindAnyObjectByType<ScoreManager>();
        }

        GetGoals();
        SetupGoals();
    }

    void GetGoals()
    {
        if (IsLevelValid())
        {
            levelGoals = board.world.levels[board.level].levelGoals;
            goalsDictionary = new Dictionary<string, BlankGoal>();

            foreach (var goal in levelGoals)
            {
                goal.numberCollected = 0;
                if (!goalsDictionary.ContainsKey(goal.matchValue))
                    goalsDictionary.Add(goal.matchValue, goal);
            }
        }
        else
        {
            levelGoals = new BlankGoal[0];
            goalsDictionary = new Dictionary<string, BlankGoal>();
        }
    }

    bool IsLevelValid()
    {
        return board != null && board.world != null && board.level < board.world.levels.Length && board.world.levels[board.level] != null;
    }

    void SetupGoals()
    {
        currentGoals.Clear();

        for (int i = 0; i < levelGoals.Length; i++)
        {
            CreateGoalPanel(levelGoals[i], goalIntroParent);
            var panel = CreateGoalPanel(levelGoals[i], goalGameParent);
            currentGoals.Add(panel);
        }
    }

    GoalPanel CreateGoalPanel(BlankGoal goal, GameObject parent)
    {
        GameObject goalGO = Instantiate(goalPrefab, parent.transform);
        goalGO.transform.localPosition = Vector3.zero;

        GoalPanel panel = goalGO.GetComponent<GoalPanel>();
        panel.thisSprite = goal.goalSprite;
        panel.thisString = $"0 / {goal.numberNeeded}";
        panel.Setup();

        return panel;
    }

    public void UpdateGoals()
    {
        int goalsCompleted = 0;
        for (int i = 0; i < levelGoals.Length; i++)
        {
            currentGoals[i].thisText.text = $"{levelGoals[i].numberCollected} / {levelGoals[i].numberNeeded}";

            if (levelGoals[i].numberCollected >= levelGoals[i].numberNeeded)
            {
                goalsCompleted++;
                currentGoals[i].thisText.text = $"{levelGoals[i].numberNeeded} / {levelGoals[i].numberNeeded}";
            }
        }

        if (goalsCompleted >= levelGoals.Length)
        {
            if (endGame != null)
            {
                endGame.WinGame();
            }
        }
    }

    public void CompareGoal(string goalToCompare)
    {
        if (goalsDictionary == null)
        {
            return;
        }

        if (goalsDictionary.TryGetValue(goalToCompare, out BlankGoal goal))
        {
            if (goal.numberCollected < goal.numberNeeded)
            {
                goal.numberCollected++;

                if (scoreManager != null)
                {
                    scoreManager.IncreaseScore(goal.pointsPerGoal);
                }

                UpdateGoals();
            }
        }
    }
}
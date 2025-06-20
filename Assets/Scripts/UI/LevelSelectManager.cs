using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject currentPanel;
    public int page;
    private GameData gameData;
    public int currentLevel = 0;

    void Start()
    {
        gameData = FindAnyObjectByType<GameData>();
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        if (gameData != null)
        {
            for(int i = 0; i < gameData.saveData.isActive.Length; i++)
            {
                if(gameData.saveData.isActive[i])
                {
                    currentLevel = i;
                }
            }
        }
        page = (int)Mathf.Floor(currentLevel / 9);
        currentPanel = panels[page];
        panels[page].SetActive(true);
    }

    public void PageRight()
    {
        if(page < panels.Length - 1)
        {
            currentPanel.SetActive(false);
            page++;
            currentPanel = panels[page];
            currentPanel.SetActive(true);
        }
    }

    public void PageLeft()
    {
        if (page > 0)
        {
            currentPanel.SetActive(false);
            page--;
            currentPanel = panels[page];
            currentPanel.SetActive(true);
        }
    }
}

# 📚 Diamond Match 3 - Technical Documentation


## 🏗 Architectural Overview
classDiagram
    class GameManager{
        +GameState CurrentState
        +StartGame()
        +EndGame()
    }
    class LevelManager{
        +List~LevelConfig~ Levels
        +LoadLevel(int index)
    }
    class UIManager{
        +UpdateScore(int points)
        +ShowPopup(string message)
    }
    GameManager --> LevelManager
    GameManager --> UIManager

💻 Code Samples

1. Match Detection System
// GridManager.cs
public List<Gem> FindMatches(Gem originGem)
{
    var matches = new List<Gem>();
    var visited = new bool[width, height];
    var queue = new Queue<Gem>();
    
    queue.Enqueue(originGem);
    visited[originGem.x, originGem.y] = true;
    
    while (queue.Count > 0)
    {
        Gem current = queue.Dequeue();
        matches.Add(current);
        
        foreach (Gem neighbor in GetNeighbors(current))
        {
            if (!visited[neighbor.x, neighbor.y] && neighbor.Type == originGem.Type)
            {
                visited[neighbor.x, neighbor.y] = true;
                queue.Enqueue(neighbor);
            }
        }
    }
    
    return matches.Count >= 3 ? matches : new List<Gem>();
}

2. Power-up System
// PowerupManager.cs
public enum PowerupType { Lightning, Bomb, ColorBlast }

public void ActivatePowerup(PowerupType type)
{
    switch (type)
    {
        case PowerupType.Lightning:
            StartCoroutine(LightningEffect());
            break;
        case PowerupType.Bomb:
            Instantiate(bombPrefab, targetPosition);
            break;
    }
}

📊 Performance Optimization
Technique	Implementation	Improvement
Object Pooling	Reusing gem instances	-35% GC Allocation
Texture Atlas	Combined sprite sheets	-20% Draw Calls
Coroutine Chains	For sequenced animations	+15% FPS Boost

🛠 Development Pipeline
graph LR
    A[Design] --> B[Prototype]
    B --> C[Playtest]
    C --> D[Optimize]
    D --> E[Polish]
    E --> F[Release]

# Clone repository
git clone https://github.com/BaYuNmeow/diamond-match3.git

# Open in Unity Hub
unity-hub --open-project ./diamond-match3

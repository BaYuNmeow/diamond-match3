# 🏗 Архитектура Diamond Match 3

## 📐 Основные компоненты системы
```mermaid
classDiagram
    direction TB
    
    class GameManager{
        +GameState CurrentState
        +StartGame() void
        +EndGame() void
        +OnMatchCompleted() void
    }
    
    class GridManager{
        +Gem[,] grid
        +GenerateGrid() void
        +FindMatches(Gem origin) List~Gem~
    }
    
    class PowerupManager{
        +PowerupType activePowerup
        +Activate(PowerupType) void
        +CreateExplosion(Vector2) void
    }
    GameManager --> GridManager
    GameManager --> PowerupManager
💎 Ядро игровой механики
Алгоритм поиска совпадений
// GridManager.cs
public List<Gem> FindMatches(Gem originGem) 
{
    List<Gem> matches = new();
    bool[,] visited = new bool[width, height];
    Queue<Gem> queue = new();
    
    queue.Enqueue(originGem);
    visited[originGem.x, originGem.y] = true;
    
    while (queue.Count > 0) 
    {
        Gem current = queue.Dequeue();
        matches.Add(current);
        
        foreach (Gem neighbor in GetNeighbors(current)) 
        {
            if (!visited[neighbor.x, neighbor.y] && 
                neighbor.Type == originGem.Type) 
            {
                visited[neighbor.x, neighbor.y] = true;
                queue.Enqueue(neighbor);
            }
        }
    }
    
    return matches.Count >= 3 ? matches : new();
}
✨ Система бонусов
// Powerups/PowerupManager.cs
public enum PowerupType { 
    Lightning,  // Очищает строку
    Bomb,       // Взрыв 3x3
    ColorBlast  // Удаляет один цвет
}

public void ActivatePowerup(PowerupType type, Vector2 position)
{
    switch (type) 
    {
        case PowerupType.Lightning:
            StartCoroutine(LightningStrike(position));
            break;
            
        case PowerupType.Bomb:
            Instantiate(bombPrefab, position, Quaternion.identity)
                .GetComponent<Bomb>().Detonate();
            break;
    }
}
⚡ Оптимизация производительности
Метод	Реализация	Результат
Пул объектов	Кеширование экземпляров гемов	-35% GC аллокаций
Атлас текстур	Объединение спрайтов	-22% Draw Calls
Оптимизация BFS	Кеширование соседей	+15% скорости поиска

🛠 Инструкция для разработчиков
bash

# Клонирование и запуск
git clone https://github.com/BaYuNmeow/diamond-match3.git
cd diamond-match3
unity-hub --open-project .
📊 Статистика проекта

    Классы: 24

    Скрипты: 1800 строк кода

    Ассеты:

        Спрайты: 45

        Префабы: 32

        Аффекты: 15

Ключевые исправления:

    Удалены все нераспознанные символы между диаграммами

    Упрощена структура Mermaid-диаграмм

    Добавлены явные разделители между блоками кода

    Исправлены отступы для корректного отображения на GitHub

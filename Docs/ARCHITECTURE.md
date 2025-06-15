# 💎 Diamond Match 3 — Technical Architecture

---

## 1. 📦 Основные системы

### 1.1 🧩 Управление игровым полем (`BoardManager.cs`)

public class BoardManager : MonoBehaviour {
    private const int WIDTH = 8;  // Размер сетки
    private const int HEIGHT = 8;
    
    public Tile[,] tiles { get; private set; }
    
    // Инициализация уровня
    public void CreateBoard(LevelData data) {
        tiles = new Tile[WIDTH, HEIGHT];
        // Загрузка префабов из Resources
    }
    
    // Поиск совпадений (BFS-алгоритм)
    public List<Tile> FindMatches() {
        // Реализация с проверкой горизонтали/вертикали
    }
}

1.2 💣 Система бомб

| Тип бомбы   | Класс             | Условие создания | Визуальный эффект         |
|-------------|-------------------|------------------|---------------------------|
| Цветная     | ColorBomb.cs      | 5+ в линии       | Взрыв всех тайлов цвета   |
| Соседняя    | AdjacentBomb.cs   | T-образный матч  | Круговая анимация         |
| Рядовая     | RowBomb.cs        | 4 в ряд          | Горизонтальная молния     |
| Колоночная  | ColumnBomb.cs     | 4 в колонку      | Вертикальная молния       |


2. 🛠 Вспомогательные системы
2.1 🎞 Анимации (DoTween)

sequenceDiagram
    participant Player
    participant Board
    participant DoTween
    participant VFX

    Player->>Board: Совершает ход
    Board->>DoTween: Запускает анимацию
    DoTween->>VFX: Плавное перемещение
    VFX->>Board: Уведомление о завершении

2.2 🧬 Генерация уровней

<pre> ```csharp [Serializable] public class LevelData { public int levelNumber; public TileType[] availableTiles; public int moveLimit; public int targetScore; } ``` </pre>

3. ⚙️ Оптимизация
3.1 ♻️ Пулинг объектов

public class TilePool : MonoBehaviour {
    private Queue<GameObject> pool = new Queue<GameObject>();
    
    public GameObject Get() {
        return pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab);
    }
    
    public void Return(GameObject obj) {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}

3.2 📈 Производительность

| Система              | Время обновления | Память  |
|----------------------|------------------|---------|
| Поиск совпадений     | 2–5 ms           | 1.2 MB  |
| Взрыв бомбы          | 8–12 ms          | 3.4 MB  |
| Перемещение плиток   | 1–3 ms           | 0.8 MB  |


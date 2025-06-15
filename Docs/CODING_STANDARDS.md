# 🧑💻 Стандарты кода Diamond Match 3

## 📝 Именование
| Элемент          | Правило                | Пример из проекта       |
|------------------|------------------------|-------------------------|
| Классы          | PascalCase             | `GemController`         |
| Публичные поля  | PascalCase             | `public int Score`      |
| Приватные поля  | _camelCase             | `private int _gemCount` |
| Константы       | ALL_CAPS               | `MAX_LEVEL = 50`        |

## 🏗 Структура кода
```csharp
// Правильный порядок в скриптах:
public class Gem : MonoBehaviour {
    // 1. SerializeField
    [SerializeField] private GemType _type;
    
    // 2. Публичные свойства
    public bool IsMatched { get; private set; }
    
    // 3. Unity-методы
    private void OnMouseDown() { ... }
    
    // 4. Кастомные методы
    public void TriggerMatch() { ... }
}

✅ Best Practices проекта

    Для анимаций используем DOTween:

transform.DOShakeScale(0.3f, 0.5f);

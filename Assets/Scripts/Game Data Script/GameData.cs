using UnityEngine;
using System.IO;
using System;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    [Serializable]
    public class SaveData
    {
        public bool[] isActive = new bool[0];
        public int[] highScores = new int[0];
        public int[] stars = new int[0];
    }

    public SaveData saveData = new SaveData();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();            // ← сначала загружаем
            InitializeData();  // ← потом инициализируем, если нужно
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void InitializeData()
    {
        // Проверяем, инициализирован ли массив isActive
        if (saveData.isActive == null || saveData.isActive.Length == 0)
        {
            saveData.isActive = new bool[27]; // Создаём массив на 27 уровней
            saveData.isActive[0] = true; // Устанавливаем первый уровень как активный
        }
        else
        {
            // Если массив уже инициализирован, проверяем, активен ли первый уровень
            if (saveData.isActive.Length > 0 && !saveData.isActive[0])
            {
                saveData.isActive[0] = true; // Устанавливаем первый уровень как активный, если он не активен
            }
        }

        // Инициализация других массивов
        if (saveData.highScores == null || saveData.highScores.Length == 0)
            saveData.highScores = new int[27]; // Массив для высоких оценок на 27 уровней

        if (saveData.stars == null || saveData.stars.Length == 0)
            saveData.stars = new int[27]; // Массив для звёзд на 27 уровней
    }


    public void Save()
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, "save.json");
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(path, json);
        }
        catch (Exception)
        {
            // Ошибки сохранения игнорируются
        }
    }

    public void Load()
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, "save.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                saveData = JsonUtility.FromJson<SaveData>(json);
            }
        }
        catch (Exception)
        {
            // Ошибки загрузки игнорируются
        }
    }

    void OnApplicationQuit() => Save();
    void OnDisable() => Save();

}
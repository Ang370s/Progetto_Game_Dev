using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ScoreList
{
    public List<ScoreData> scores = new List<ScoreData>();
}

public class ScoreManager : MonoBehaviour
{

    private string filePath;
    public ScoreList scoreList = new ScoreList();

    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("ScoreManager");
                _instance = obj.AddComponent<ScoreManager>();
            }
            return _instance;
        }
    }

    private static ScoreManager _instance;



    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            filePath = Application.persistentDataPath + "/scores.json";
            LoadScores();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }


    public void SaveNewScore(int score)
    {
        ScoreData newScore = new ScoreData(score);
        scoreList.scores.Add(newScore);

        // Ordina dal più alto al più basso
        scoreList.scores.Sort((a, b) => b.score.CompareTo(a.score));

        // Tieni solo i primi 10
        if (scoreList.scores.Count > 10)
            scoreList.scores.RemoveAt(scoreList.scores.Count - 1);

        SaveToFile();
    }

    void SaveToFile()
    {
        string json = JsonUtility.ToJson(scoreList, true);
        File.WriteAllText(filePath, json);
    }

    void LoadScores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            scoreList = JsonUtility.FromJson<ScoreList>(json);
        }
    }
}
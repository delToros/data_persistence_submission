using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    public static DataPersistence Instance;

    public string playerName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // All about saving data to json
    [Serializable]
    public class ScoreEntry
    {
        public string playerName;
        public int score;

        public ScoreEntry(string name, int s)
        {
            playerName = name;
            score = s;
        }
    }

    [Serializable]
    public class HighScoreData
    {
        public List<ScoreEntry> scores = new List<ScoreEntry>();
    }

}

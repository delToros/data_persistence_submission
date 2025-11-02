using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataPersistence : MonoBehaviour
{
    public static DataPersistence Instance;

    public string playerName;
    public int playerHighScore;

    string path;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScoreData();

        path = Application.persistentDataPath + "/savefile.json";
    }

    // All about saving data to json

    // 1. Containers
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

    // Global variable to hold the loaded data in memory
    private HighScoreData _highScoreData = new HighScoreData();

    // NEW HELPER METHOD: Loads JSON from file into the C# object
    private void LoadHighScoreData()
    {
        if (File.Exists(path))
        {
            // Read the entire file contents as a single string
            string json = File.ReadAllText(path);

            // Deserialize the JSON string into the HighScoreData object
            _highScoreData = JsonUtility.FromJson<HighScoreData>(json);
        }
        else
        {
            // If the file doesn't exist, start with an empty list
            _highScoreData = new HighScoreData();
            Debug.Log("Save file not found. Starting with a new high score list.");
        }
    }

    // 2. Load player score (Completed logic)
    public int LoadPlayersScore(string playerName)
    {
        // Ensure data is loaded (redundant if called after Awake, but safe)
        if (_highScoreData.scores.Count == 0 && File.Exists(path))
        {
            LoadHighScoreData();
        }

        // Search the list for an entry that matches the playerName
        ScoreEntry existingEntry = _highScoreData.scores.Find(e => e.playerName == playerName);

        if (existingEntry != null)
        {
            // Player exists: return their highest score
            this.playerHighScore = existingEntry.score;
            return existingEntry.score;
        }
        else
        {
            // Player does not exist in the file: return 0
            return 0;
        }
    }

    // 3. Save score
    public void AddOrUpdateScore(string playerName, int newScore)
    {
        // 1. Load existing data first
        LoadHighScoreData();

        // 2. Try to find the existing player entry
        ScoreEntry existingEntry = _highScoreData.scores.Find(e => e.playerName == playerName);

        if (existingEntry != null)
        {
            // Player exists: update if the new score is higher
            if (newScore > existingEntry.score)
            {
                existingEntry.score = newScore;
            }
        }
        else
        {
            // Player is new: add a new entry
            _highScoreData.scores.Add(new ScoreEntry(playerName, newScore));
        }
        // 3. Sort the list (e.g., descending score)
        _highScoreData.scores.Sort((a, b) => b.score.CompareTo(a.score));

        // 4. Save the updated data
        SaveScores();
    }

    private void SaveScores()
    {
        string json = JsonUtility.ToJson(_highScoreData);
        File.WriteAllText(path, json);
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public TMP_InputField nameInput;
    public string playerName;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitApp()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

#else
        Application.Quit();
#endif
    }

    public void GetInputName()
    {
        playerName = nameInput.text;

        if (!string.IsNullOrEmpty(playerName) )
        {
            Debug.Log($"Player name is: {playerName}");
            DataPersistence.Instance.playerName = playerName;
        }
        else
        {
            Debug.Log($"Input field in empty");
        }
    }
}

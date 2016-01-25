using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour
{
    AsyncOperation operation;
    string levelName;


    void Start()
    {
        levelName = PlayerPrefs.GetString("LoadLevel");
    }

    void Update()
    {
        operation = Application.LoadLevelAsync(levelName);
        if (operation.isDone)
        {
            PlayerPrefs.SetInt("Load", 0); // False
            Application.LoadLevel(levelName);
        }
    }
}
using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour
{
    string levelName;

    void Start()
    {
        levelName = PlayerPrefs.GetString("LoadLevel");
        Application.LoadLevelAsync(levelName).allowSceneActivation = true;
    }

    
}
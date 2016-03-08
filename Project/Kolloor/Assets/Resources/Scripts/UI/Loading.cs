using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour
{
    AsyncOperation operation;
    bool loadingLevel = false;
    int level;


    void Awake()
    {
        level = PlayerPrefs.GetInt("LoadLevel");
    }

    void Update()
    {
        if (!loadingLevel)
        {
            operation = Application.LoadLevelAsync(level);
            loadingLevel = true;
        }
        if (operation.isDone)
            Application.LoadLevel(level);
    }
}
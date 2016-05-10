using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
            operation = SceneManager.LoadSceneAsync(level);
            loadingLevel = true;
        }
        if (operation.isDone)
            SceneManager.LoadScene(level);
    }
}
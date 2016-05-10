using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelImageFix : MonoBehaviour
{
    bool first, fix;


    void Awake()
    {
        Debug.Log("LevelImageFix");
        DontDestroyOnLoad(this);
        if (!first)
        {
            PlayerPrefs.SetInt("LoadLevel", (int)Scenes.Level1);
            SceneManager.LoadScene((int)Scenes.Loading);
            first = true;
        }
    }

    void Update()
    {
        if (!fix && SceneManager.GetActiveScene().buildIndex == (int)Scenes.Level1)
        {
            int standardScenes = 3;     // -Start -Loading -Credits
            int currentLevelIndex = SceneManager.GetActiveScene().buildIndex - standardScenes;
            int nextLevelIndex = currentLevelIndex + 1;
            int maxLevelIndex = SceneManager.sceneCountInBuildSettings - standardScenes - 1;     // -1 because index

            if (PlayerPrefs.GetInt("CurrentLevel") <= nextLevelIndex)
                PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel"));

            if (currentLevelIndex == maxLevelIndex)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PlayerPrefs.SetInt("LoadLevel", (int)Scenes.Start);
                fix = true;
                SceneManager.LoadScene("Loading");
            }
            else
            {
                PlayerPrefs.SetInt("LoadLevel", (int)Scenes.Start);
                fix = true;
                SceneManager.LoadScene("Loading");
            }
        }
    }
}
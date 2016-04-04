using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    public static StartMenu instance;

    public GameObject continueButton;

    public GameObject startObject;
    public GameObject newGameObject;
    public GameObject levelsObject;
    public GameObject optionsObject;

    int standardScenes = 3;     // -Start -Loading -Credits


    void Awake()
    {
        instance = this;
        continueButton.SetActive(PlayerPrefs.GetInt("CurrentLevel") > (int)Scenes.Level1-standardScenes&& PlayerPrefs.GetInt("CurrentLevel") < SceneManager.sceneCountInBuildSettings-standardScenes);
    }

    public void Continue()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        PlayerPrefs.SetInt("LoadLevel", PlayerPrefs.GetInt("CurrentLevel") + standardScenes);
        SceneManager.LoadScene("Loading");
    }

    public void NewGame()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        startObject.SetActive(false);
        newGameObject.SetActive(true);
    }

    public void Level()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        startObject.SetActive(false);
        levelsObject.SetActive(true);
        Levels.instance.Start();
    }

    public void Options()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        startObject.SetActive(false);
        optionsObject.SetActive(true);
    }

    public void Quit()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        Application.Quit();
    }

    public void Back()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        newGameObject.SetActive(false);
        levelsObject.SetActive(false);
        optionsObject.SetActive(false);
        startObject.SetActive(true);
    }

    public void CreateNewGame()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        PlayerPrefs.SetInt("CurrentLevel", (int)Scenes.Level1 - standardScenes);
        continueButton.SetActive(PlayerPrefs.GetInt("CurrentLevel") > (int)Scenes.Level1 && PlayerPrefs.GetInt("CurrentLevel") < SceneManager.sceneCountInBuildSettings - standardScenes);
        PlayerPrefs.SetInt("LoadLevel", (int)Scenes.Level1);
        SceneManager.LoadScene("Loading");
    }
}
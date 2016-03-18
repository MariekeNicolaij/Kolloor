using UnityEngine;
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


    void Awake()
    {
        instance = this;
        if (PlayerPrefs.GetInt("CurrentLevel") < (int)Scenes.Level1)
            continueButton.SetActive(false);
    }

    public void Continue()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        PlayerPrefs.SetInt("LoadLevel", PlayerPrefs.GetInt("CurrentLevel"));
        Application.LoadLevel("Loading");
    }

    public void NewGame()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        startObject.SetActive(false);
        newGameObject.SetActive(true);
    }

    public void Levels()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        startObject.SetActive(false);
        levelsObject.SetActive(true);
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
        PlayerPrefs.SetInt("CurrentLevel", (int)Scenes.Level1);
        PlayerPrefs.SetInt("LoadLevel", (int)Scenes.Level1);
        Application.LoadLevel("Loading");
    }
}
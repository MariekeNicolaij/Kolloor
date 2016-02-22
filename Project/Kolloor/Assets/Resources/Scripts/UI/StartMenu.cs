using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    public static StartMenu instance;

    public GameObject continueButton;

    public GameObject startObject;
    public GameObject newGameObject;
    public GameObject optionsObject;


    void Awake()
    {
        instance = this;
        if (PlayerPrefs.GetString("CurrentLevel") == string.Empty)
            continueButton.SetActive(false);
    }

    public void Continue()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        PlayerPrefs.SetString("LoadLevel", PlayerPrefs.GetString("CurrentLevel"));
        Application.LoadLevel("Loading");
    }

    public void NewGame()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        startObject.SetActive(false);
        newGameObject.SetActive(true);
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
        optionsObject.SetActive(false);
        startObject.SetActive(true);
    }

    public void CreateNewGame()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        PlayerPrefs.SetString("CurrentLevel", "Level 1 - Nature");
        PlayerPrefs.SetString("LoadLevel", "Level 1 - Nature");
        Application.LoadLevel("Loading");
    }
}
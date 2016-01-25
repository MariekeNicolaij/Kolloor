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
        if (PlayerPrefs.GetInt("CurrentLevel") == 0)
            continueButton.SetActive(false);
    }

    public void Continue()
    {

    }

    public void NewGame()
    {
        startObject.SetActive(false);
        newGameObject.SetActive(true);
    }

    public void Options()
    {
        startObject.SetActive(false);
        optionsObject.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackToStartMenu()
    {
        newGameObject.SetActive(false);
        optionsObject.SetActive(false);
        startObject.SetActive(true);
    }

    public void CreateNewGame()
    {
        PlayerPrefs.SetInt("CurrentLevel", 1);
        PlayerPrefs.SetString("LoadLevel", "Level 1 - Nature");
        Application.LoadLevel("Loading");
    }
}
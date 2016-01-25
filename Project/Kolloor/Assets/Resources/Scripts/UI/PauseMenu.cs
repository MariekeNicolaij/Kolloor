using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public GameObject startPauseObject;
    public GameObject pauseObject;
    public GameObject restartObject;
    public GameObject optionsObject;
    public GameObject mainMenuObject;


    void Awake()
    {
        instance = this;
    }

    void EnableCursor(bool enable)
    {
        MouseLook.instance.canMove = !enable;
        Cursor.lockState = enable ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = enable;
    }

    public void Pause(bool pause)
    {
        Time.timeScale = System.Convert.ToInt32(!pause);
        EnableCursor(pause);
        startPauseObject.SetActive(pause);
    }

    public void Resume()
    {
        Pause(false);
    }

    public void Restart()
    {
        pauseObject.SetActive(false);
        restartObject.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetString("LoadLevel", Application.loadedLevelName);
        Application.LoadLevel("Loading");
    }

    public void Options()
    {
        pauseObject.SetActive(false);
        optionsObject.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        restartObject.SetActive(false);
        optionsObject.SetActive(false);
        mainMenuObject.SetActive(false);
        pauseObject.SetActive(true);
    }

    public void MainMenu()
    {
        pauseObject.SetActive(false);
        mainMenuObject.SetActive(true);
    }

    public void ActivateMainMenu()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetString("LoadLevel", "Start");
        Application.LoadLevel("Loading");
    }
}
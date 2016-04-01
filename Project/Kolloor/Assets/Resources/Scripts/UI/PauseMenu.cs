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
        Options.instance.Back();
        Back();
        EnableCursor(pause);
        startPauseObject.SetActive(pause);
    }

    public void Resume()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        Pause(false);
    }

    public void Restart()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        pauseObject.SetActive(false);
        restartObject.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        PlayerPrefs.SetInt("LoadLevel", Application.loadedLevel);
        Application.LoadLevel("Loading");
    }

    public void OptionsMenu()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        pauseObject.SetActive(false);
        optionsObject.SetActive(true);
    }

    public void Back()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        restartObject.SetActive(false);
        optionsObject.SetActive(false);
        mainMenuObject.SetActive(false);
        pauseObject.SetActive(true);
    }

    public void MainMenu()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        pauseObject.SetActive(false);
        mainMenuObject.SetActive(true);
    }

    public void ActivateMainMenu()
    {
        Time.timeScale = 1;
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        PlayerPrefs.SetInt("LoadLevel", (int)Scenes.Start);
        Application.LoadLevel("Loading");
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class NextLevelPortal : MonoBehaviour
{
    DirectoryInfo levelDirectoryPath = new DirectoryInfo("Assets/Resources/Scenes/Levels");

    Vector3 positionInGround;

    public bool lerpToGround, isLerped;

    float slotHeight = 3.05f;
    float maxLerpDistance = 0.025f;

    List<string> levelNames = new List<string>();


    void Start()
    {
        positionInGround = transform.position;
        positionInGround.y = transform.position.y + slotHeight;
    }

    void Update()
    {
        Lerp();
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Achievement unlocked! : Cheater!");
            NextLevelCheck();
        }
    }

    void Lerp()
    {
        if (ColorManager.instance.allColorsUnlocked && !isLerped)
            lerpToGround = true;

        if (lerpToGround)
        {
            transform.position = Vector3.Lerp(transform.position, positionInGround, Time.smoothDeltaTime);

            if (Vector3.Distance(transform.position, positionInGround) < maxLerpDistance)
            {
                lerpToGround = false;
                isLerped = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            NextLevelCheck();
    }

    void NextLevelCheck()
    {
        int currentLevel = Application.loadedLevel;
        int nextLevel = currentLevel + 1;           // +1 Because index
        int maxLevel = Application.levelCount-1;    // -1 Because index

        if (currentLevel == maxLevel)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerPrefs.SetInt("CurrentLevel", nextLevel);
            PlayerPrefs.SetInt("LoadLevel", (int)Scenes.Credits);
            Application.LoadLevel("Loading");
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", nextLevel);
            PlayerPrefs.SetInt("LoadLevel", nextLevel);
            Application.LoadLevel("Loading");
        }
    }
}
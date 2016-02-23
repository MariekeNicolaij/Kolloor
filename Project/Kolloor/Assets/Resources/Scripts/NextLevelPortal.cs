using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class NextLevelPortal : MonoBehaviour
{
    Vector3 positionInGround;

    public bool lerpToGround, isLerped;

    float slotHeight = 3.05f;
    float maxLerpDistance = 0.025f;

    List<string> levelNames = new List<string>();


    void Start()
    {
        positionInGround = transform.position;
        positionInGround.y = transform.position.y + slotHeight;

        GetLevelNames();
    }

    void GetLevelNames()
    {
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            string name = scene.path.Substring(scene.path.LastIndexOf('/') + 1);    // +1 deletes /
            name = name.Substring(0, name.Length - 6);                              // Removes unity extension from string
            levelNames.Add(name);
        }

        levelNames.Remove("Start");
        levelNames.Remove("Loading");
        levelNames.Remove("Credits");
    }

    void Update()
    {
        Lerp();
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
        {
            PlayerPrefs.SetString("CurrentLevel", LevelName(levelNames));
            PlayerPrefs.SetString("LoadLevel", LevelName(levelNames));
            Application.LoadLevel("Loading");
        }
    }

    /// <summary>
    /// Checks if next level exists, if not end screen
    /// </summary>
    /// <returns></returns>
    string LevelName(List<string> names)
    {
        int currentIndex = 0, nextIndex = 0, maxIndex = levelNames.Count;

        for (int i = 0; i < maxIndex; i++)
            if (Application.loadedLevelName == levelNames[i])           // Bepalen welk level dit is
                currentIndex = i;

        nextIndex++;                                                    // Volgend level

        if (nextIndex >= maxIndex)                                      // Checken of er een volgend level bestaat
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return "Credits";
        }
        return levelNames[nextIndex];
    }
}
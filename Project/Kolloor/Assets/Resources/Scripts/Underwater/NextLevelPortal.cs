using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NextLevelPortal : MonoBehaviour
{
    Vector3 positionInGround;

    public bool lerpToGround, isLerped;

    float slotHeight = 3.05f;
    float maxLerpDistance = 0.025f;


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
        int standardScenes = 3;     // -Start -Loading -Credits
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex - standardScenes;
        int nextLevelIndex = currentLevelIndex + 1;
        int maxLevelIndex = SceneManager.sceneCountInBuildSettings - standardScenes - 1;     // -1 because index

        if (PlayerPrefs.GetInt("CurrentLevel") <= nextLevelIndex)
            PlayerPrefs.SetInt("CurrentLevel", nextLevelIndex);

        if (currentLevelIndex == maxLevelIndex)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerPrefs.SetInt("LoadLevel", (int)Scenes.Credits);
            SceneManager.LoadScene("Loading");
        }
        else
        {
            PlayerPrefs.SetInt("LoadLevel", nextLevelIndex + standardScenes);
            SceneManager.LoadScene("Loading");
        }
    }
}
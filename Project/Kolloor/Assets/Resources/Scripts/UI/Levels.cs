using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Levels : MonoBehaviour
{
    public static Levels instance;

    public Material levelImageMaterial;
    public GameObject playButton;

    public GameObject levelsParent;
    Sprite lockImage;
    List<Image> levelImages = new List<Image>();
    List<Image> lockImages = new List<Image>();

    Vector3 startPosition;

    int lastLevelIndex;
    int currentLevelIndex = 0;
    int maxLevelIndex;
    int indexChange = 0;

    float lerpTime = 1;
    float lerpSpeed = 10;
    float colorRadius;

    bool locksSet = false;
    bool lerpPosition, lerpColor;


    void Awake()
    {
        instance = this;
        GetImages();
    }

    public void Start()
    {
        if (!levelsParent.activeInHierarchy)
            return;

        IndexCheck();
        SetImages();
        SetLockImages();
        PlayButtonCheck();

        lerpColor = true;
    }

    /// <summary>
    /// Get all level images of the first children and get lock image
    /// </summary>
    void GetImages()
    {
        levelImages.Clear();
        lockImage = Resources.Load<Sprite>("UI/Levels/Lock");

        foreach (Transform t in levelsParent.transform)
            levelImages.Add(t.GetComponent<Image>());

        startPosition = levelImages[currentLevelIndex].transform.localPosition;
    }

    /// <summary>
    /// Set lock images in front of level images which aren't completed yet
    /// </summary>
    void SetLockImages()
    {
        if (locksSet)
            return;

        //Debug.Log("MaxLevelIndex: " + maxLevelIndex);
        for (int i = PlayerPrefs.GetInt("CurrentLevel") + 1; i <= maxLevelIndex; i++)     // +1 because nextlevel
        {
            //Debug.Log("CurrentLevel: " + i);
            GameObject go = new GameObject();
            go.AddComponent<Image>().sprite = lockImage;
            go.transform.SetParent(levelImages[i].transform);
            go.transform.localPosition = Vector3.zero;
        }
        locksSet = true;
    }

    void Update()
    {
        if (lerpPosition)
            LerpImage();
        if (lerpColor)
            LerpColor();
    }

    /// <summary>
    /// Lerp last image for fancyness
    /// </summary>
    void LerpImage()
    {
        lerpTime -= Time.smoothDeltaTime;

        if (lerpTime > 0)
        {
            levelImages[lastLevelIndex].transform.localPosition += (Vector3.left * indexChange).normalized * lerpSpeed;
            levelImages[lastLevelIndex].transform.localScale *= lerpTime;
        }
        else
        {
            levelImages[lastLevelIndex].transform.localPosition = startPosition;
            levelImages[lastLevelIndex].transform.localScale = Vector3.one;
            levelImages[lastLevelIndex].gameObject.SetActive(false);
            lerpTime = 1;
            lerpColor = true;
            lerpPosition = false;
        }
    }

    /// <summary>
    ///  Lerps color back
    /// </summary>
    void LerpColor()
    {
        levelImageMaterial.SetVector("ColorStartPoint", levelImages[currentLevelIndex].sprite.bounds.center);
        if (currentLevelIndex < PlayerPrefs.GetInt("CurrentLevel"))
            levelImageMaterial.SetFloat("ColorRadius", colorRadius);
        else
            lerpColor = false;

        float maxRadius = 1500;
        colorRadius += lerpSpeed;

        if (colorRadius > maxRadius)
        {
            colorRadius = 0;
            lerpColor = false;
        }
    }

    void IndexCheck()
    {
        maxLevelIndex = levelImages.Count - 1;         // -1 because index
        lastLevelIndex = currentLevelIndex;
        currentLevelIndex += indexChange;

        if (currentLevelIndex > maxLevelIndex)
            currentLevelIndex = 0;
        else if (currentLevelIndex < 0)
            currentLevelIndex = maxLevelIndex;
    }

    /// <summary>
    /// Deactivate all levelImages except the current and the last one
    /// </summary>
    void SetImages()
    {
        foreach (Image i in levelImages)
            i.gameObject.SetActive(false);
        levelImages[currentLevelIndex].gameObject.SetActive(true);
        levelImages[lastLevelIndex].gameObject.SetActive(true);
        levelImages[currentLevelIndex].transform.SetAsLastSibling();
        levelImages[lastLevelIndex].transform.SetAsLastSibling();
        levelImageMaterial.SetFloat("ColorRadius", 0);
    }

    /// <summary>
    /// Left or right button
    /// </summary>
    /// <param name="indexChange"></param>
    public void Button(int indexChange)
    {
        if (lerpPosition || lerpColor)
            return;

        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        this.indexChange = indexChange;
        IndexCheck();
        SetImages();
        PlayButtonCheck();
        lerpPosition = true;
    }

    /// <summary>
    /// Play selected level
    /// </summary>
    public void PlayButton()
    {
        int standardScenes = 3;         // Start, Loading, Credits
        PlayerPrefs.SetInt("LoadLevel", currentLevelIndex+standardScenes);
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// Set play button to active when you are allowed to play this level
    /// </summary>
    void PlayButtonCheck()
    {
        playButton.SetActive(currentLevelIndex <= PlayerPrefs.GetInt("CurrentLevel"));
    }
}
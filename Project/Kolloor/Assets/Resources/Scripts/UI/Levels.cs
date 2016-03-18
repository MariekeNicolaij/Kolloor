using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Levels : MonoBehaviour
{
    public static Levels instance;

    public GameObject playButton;

    public GameObject levelsParent;
    Sprite lockImage;
    List<Image> levelImages = new List<Image>();
    List<Image> lockImages = new List<Image>();
    List<GameObject> lockedLevels = new List<GameObject>();

    Vector3 startPosition;

    int minusBasicScenes = 2;   // - Start - Loading - Credits + 1 because index

    int lastIndex;
    int currentIndex = 0;
    int maxIndex;
    int indexChange;

    float lerpTime = 1;
    float lerpSpeed = 6;

    bool lerp;


    public void Start()
    {
        instance = this;

        GetImages();
        IndexCheck();
        SetImages();
        SetLockImages();
        SetColorInImages();
    }

    void GetImages()
    {
        lockImage = Resources.Load<Sprite>("UI/Levels/Lock");

        foreach (Image i in levelsParent.GetComponentsInChildren<Image>())
            levelImages.Add(i);
        levelImages[currentIndex].gameObject.SetActive(true);
        startPosition = levelImages[currentIndex].transform.position;
    }

    void SetLockImages()
    {
        for (int i = PlayerPrefs.GetInt("CurrentLevel") - minusBasicScenes; i <= maxIndex; i++)
        {
            GameObject go = new GameObject();
            go.AddComponent<Image>().sprite = lockImage;
            go.transform.SetParent(levelImages[i].transform);
            go.transform.localPosition = Vector3.zero;
        }
    }

    void SetColorInImages()
    {
        foreach (Image i in levelImages)
        {
            Debug.Log("gs " + i.color.grayscale);
            //i.color= 
        }
        for (int i = PlayerPrefs.GetInt("CurrentLevel") - minusBasicScenes; i <= maxIndex; i++)
        {

        }
    }

    void Update()
    {
        if (lerp)
            LerpImage();
    }

    void LerpImage()
    {
        lerpTime -= Time.smoothDeltaTime;

        if (lerpTime > 0)
        {
            levelImages[lastIndex].transform.position += (Vector3.right * indexChange).normalized * lerpSpeed;
            levelImages[lastIndex].color = new Color(levelImages[lastIndex].color.r, levelImages[lastIndex].color.g, levelImages[lastIndex].color.b, lerpTime);
        }
        else
        {
            levelImages[lastIndex].color = new Color(levelImages[lastIndex].color.r, levelImages[lastIndex].color.g, levelImages[lastIndex].color.b, 1);
            levelImages[lastIndex].transform.position = startPosition;
            levelImages[lastIndex].gameObject.SetActive(false);
            lerpTime = 1;
            lerp = false;
        }
    }

    /// <summary>
    /// Index out of range exception method
    /// </summary>
    /// <param name="indexChange"></param>
    void IndexCheck()
    {
        maxIndex = Application.levelCount - minusBasicScenes;                   // -1 because index
        lastIndex = currentIndex;
        currentIndex += indexChange;

        if (currentIndex > maxIndex)
            currentIndex = 0;
        else if (currentIndex < 0)
            currentIndex = maxIndex;
    }

    void SetImages()
    {
        foreach (Image i in levelImages)
            i.gameObject.SetActive(false);
        levelImages[currentIndex].gameObject.SetActive(true);
        levelImages[lastIndex].gameObject.SetActive(true);
        levelImages[currentIndex].transform.SetAsLastSibling();
        levelImages[lastIndex].transform.SetAsLastSibling();
    }

    public void Button(int indexChange)
    {
        if (lerp)
            return;
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        this.indexChange = indexChange;
        IndexCheck();
        SetImages();
        PlayButtonCheck();
        lerp = true;
    }

    public void PlayButton()
    {

    }

    void PlayButtonCheck()
    {
        playButton.SetActive(currentIndex < PlayerPrefs.GetInt("CurrentLevel") - minusBasicScenes);
    }
}

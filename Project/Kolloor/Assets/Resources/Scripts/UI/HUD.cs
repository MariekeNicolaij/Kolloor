using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HUD : MonoBehaviour
{
    public GameObject hudParent;
    Dictionary<string, Color> colorsWithName = new Dictionary<string, Color>();
    List<GameObject> colorObjects = new List<GameObject>();
    List<Color> oldColor = new List<Color>();
    Color gray = Color.white;

    public Material rainbow;
    public Sprite sprite;

    bool rainbowTime = false;

    int curRainbowIndex = 0, nextIndex;
    float time;
    float speed = 1;


    void Start()
    {
        GetColorMaterials();
        PlaceImagesWithMaterial();
    }

    /// <summary>
    /// Gets the material of first color in ColorManager.spreadlist
    /// </summary>
    void GetColorMaterials()
    {
        foreach (Material m in Resources.LoadAll<Material>("Materials/Puzzle"))
            colorsWithName.Add(m.name, m.color);
    }

    void PlaceImagesWithMaterial()
    {
        float spaceBetweenImages = Screen.width * 0.05f;
        int indexFix = 1;

        for (int i = 0; i < ColorManager.instance.colorSpreadList.Count; i++)
        {
            GameObject go = new GameObject();
            go.AddComponent<Image>().sprite = sprite;
            go.GetComponent<Image>().color = gray;
            oldColor.Add(colorsWithName[ColorManager.instance.colorSpreadList[i].color.ToString()]);

            go.name = ColorManager.instance.colorSpreadList[i].color.ToString();
            go.transform.SetParent(hudParent.transform);
            go.transform.localPosition = new Vector3(((i % 2 == 0) ? i + indexFix : -i) * spaceBetweenImages, 0, 0);     // Oneven index word links geplaats vanaf het midden van het scherm en even rechts
            colorObjects.Add(go);
        }
        if (colorObjects.Count % 2 != 0)     // Als count oneven is
            hudParent.transform.localPosition = new Vector3(-colorObjects[0].GetComponent<RectTransform>().rect.width, 440, 0);
    }

    void Update()
    {
        LerpColor();
        RainbowTime();
    }

    void LerpColor()
    {
        for (int i = 0; i < colorObjects.Count; i++)
            if (ColorManager.instance.colorSpreadList[i].unlocked)
            {
                if (colorObjects[i].GetComponent<Image>().color != oldColor[i])                                                         // Als deze color nog grijs is
                    colorObjects[i].GetComponent<Image>().color = Color.Lerp(colorObjects[i].GetComponent<Image>().color, oldColor[i], Time.smoothDeltaTime);
                else if (ColorManager.instance.allColorsUnlocked && colorObjects[i].GetComponent<Image>().color != oldColor[curRainbowIndex])  // Anders als alle kleuren terug in de wereld zijn en als regenboog start kleur niet de start kleur heeft van oldColor
                    colorObjects[i].GetComponent<Image>().color = Color.Lerp(colorObjects[i].GetComponent<Image>().color, oldColor[curRainbowIndex], time / speed);   //Lerp color naar start kleur van oldColor
                else if (ColorManager.instance.allColorsUnlocked && colorObjects[i].GetComponent<Image>().color == oldColor[curRainbowIndex])   // Anders als de kleuren goed staan.... rainbowTime!
                    rainbowTime = true; // Lerp colors
            }
    }

    void RainbowTime()
    {
        if (!rainbowTime)
            return;

        time += Time.smoothDeltaTime;

        if (time > speed)
        {
            IndexCheck(1);                                          // Set current index
            time = 0;                                               // Reset timer
        }
        for (int i = 0; i < colorObjects.Count; i++)
            colorObjects[i].GetComponent<Image>().color = Color.Lerp(oldColor[curRainbowIndex], oldColor[nextIndex], time / speed);   //Lerp Color
    }

    void IndexCheck(int indexChange)
    {
        int maxIndex = oldColor.Count;
        curRainbowIndex = nextIndex;
        nextIndex += indexChange;

        if (nextIndex >= maxIndex)
            nextIndex = 0;
    }
}
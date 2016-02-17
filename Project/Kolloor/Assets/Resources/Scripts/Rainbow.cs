using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rainbow : MonoBehaviour
{
    Material mattie;
    List<Color> lerpColors = new List<Color>();

    public float speed = 3;

    float time = 0;
    int curIndex = 0, nextIndex;
    

    void Start()
    {
        mattie = GetComponent<Renderer>().material;
        GetColors();
    }

    void GetColors()
    {
        foreach (Material mat in Resources.LoadAll("Materials/Rainbow"))
            lerpColors.Add(mat.color);
    }

    void Update()
    {
        time += Time.smoothDeltaTime;

        if (time > speed)
        {
            curIndex = IndexCheck(lerpColors, curIndex + 1);        // Set current index
            nextIndex = IndexCheck(lerpColors, curIndex + 1);       // Set next index
            time = 0;                                               // Reset timer
        }
        mattie.color = Color.Lerp(lerpColors[curIndex], lerpColors[nextIndex], time / speed);   //Lerp Color
    }

    int IndexCheck(List<Color> lerpColors, int currentIndex)
    {
        int maxIndex = lerpColors.Count;
        currentIndex++;

        if (currentIndex >= maxIndex)
            return 0;

        return currentIndex;
    }
}
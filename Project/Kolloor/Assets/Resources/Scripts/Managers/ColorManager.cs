using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;

    [Serializable]
    public class ColorSpreadData
    {
        public PuzzleColors color;
        public Material[] colorMaterial;
        public Vector3 startPoint;
        public bool unlocked;
        public float radius;
        [Range(0, 1)]
        public float metallicness;
        [Range(0, 1)]
        public float smoothness = 0.5f;
    }

    public List<ColorSpreadData> colorSpreadList;
    [HideInInspector]
    public List<Material> colorMaterials;

    public bool allColorsUnlocked;

    float colorSpreadSpeed = 10;
    int unlockedColorsCount = 0;


    void Awake()
    {
        instance = this;

        for (int i = 0; i < colorSpreadList.Count; i++)
        {
            ColorSpreadData colorSpreadData = colorSpreadList[i];

            for (int j = 0; j < colorSpreadData.colorMaterial.Length; j++)
            {
                colorSpreadData.colorMaterial[j].SetFloat("ColorRadius", 0);
                colorMaterials.Add(colorSpreadData.colorMaterial[j]);
            }
        }
    }

    void Update()
    {
        FillColor();
    }

    void FillColor()
    {
        for (int i = 0; i < colorSpreadList.Count; i++)
        {
            ColorSpreadData colorSpreadData = colorSpreadList[i];
            if (colorSpreadData.unlocked)
            {
                colorSpreadData.radius += Time.smoothDeltaTime * colorSpreadSpeed;
               
                for (int j = 0; j < colorSpreadData.colorMaterial.Length; j++)
                {
                    colorSpreadData.colorMaterial[j].SetVector("ColorStartPoint", colorSpreadData.startPoint);
                    colorSpreadData.colorMaterial[j].SetFloat("ColorRadius", colorSpreadData.radius);
                }
            }
        }
    }

    public void UnlockColor(PuzzleColors color, Vector3 startPoint, float radius)
    {
        for (int i = 0; i < colorSpreadList.Count; i++)
        {
            ColorSpreadData colorSpreadData = colorSpreadList[i];

            if (colorSpreadData.color == color)
            {
                colorSpreadData.unlocked = true;
                colorSpreadData.startPoint = startPoint;
                colorSpreadData.radius = radius;

                unlockedColorsCount++;

                if (unlockedColorsCount == colorSpreadList.Count)
                    allColorsUnlocked = true;
            }
        }
    }
}
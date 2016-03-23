using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ColorsIngame : MonoBehaviour
{
    public GameObject colorsParent;
    List<Material> colorMaterials = new List<Material>();

    public int DistanceBetween = 20;


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
        foreach (Material m in ColorManager.instance.colorSpreadList.Select(ss => ss.colorMaterial[0]))
            colorMaterials.Add(m);
    }

    void PlaceImagesWithMaterial()
    {
        float offset = 75;

        for (int i = 0; i < colorMaterials.Count; i++)
        {
            GameObject go = new GameObject();
            go.name = ColorManager.instance.colorSpreadList[i].color.ToString();
            go.transform.parent = colorsParent.transform;

            Debug.Log(colorMaterials[i].color);
            Debug.Log("Shit is grijs");
            if (i % 2 == 0)
                go.transform.localPosition = new Vector3((i+1) * offset, 0, 0);
            else
                go.transform.localPosition = new Vector3((-i) * offset, 0, 0);

            Debug.Log("Sprite toevoegen!");
            go.AddComponent<Image>();//.sprite = 
            go.GetComponent<Image>().color = colorMaterials[i].color;
            go.GetComponent<Image>().material = colorMaterials[i];
        }
    }
}
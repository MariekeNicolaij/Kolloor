using UnityEngine;
using System.Collections;

public class InGameUI : MonoBehaviour
{
    public GameObject interactImage;


    public void ActivateInteractHelp(bool activate)
    {
        interactImage.SetActive(activate);
    }
}
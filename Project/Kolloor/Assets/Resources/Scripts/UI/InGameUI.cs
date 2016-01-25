using UnityEngine;
using System.Collections;

public class InGameUI : MonoBehaviour
{
    public GameObject interactHandImage;

    
    public void ActivateInteractHand(bool activate)
    {
        interactHandImage.SetActive(activate);
    }
}

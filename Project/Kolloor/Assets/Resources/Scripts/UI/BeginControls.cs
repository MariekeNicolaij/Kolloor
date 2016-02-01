using UnityEngine;
using System.Collections;

public class BeginControls : MonoBehaviour
{
    void Update()
    {
        Debug.Log("New UI Elements gebruiken! en dit een minuut laten staan");
        if (Input.anyKeyDown)
            Destroy(gameObject);
    }
}
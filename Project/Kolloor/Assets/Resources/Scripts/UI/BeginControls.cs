using UnityEngine;
using System.Collections;

public class BeginControls : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
            Destroy(gameObject);
    }
}
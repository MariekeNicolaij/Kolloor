using UnityEngine;
using System.Collections;

public class BeginControls : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            Destroy(gameObject);
    }
}
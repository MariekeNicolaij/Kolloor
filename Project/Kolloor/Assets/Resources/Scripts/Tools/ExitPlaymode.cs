using UnityEngine;
using System.Collections;

public class ExitPlaymode : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
            UnityEditor.EditorApplication.isPlaying = false;
    }
}

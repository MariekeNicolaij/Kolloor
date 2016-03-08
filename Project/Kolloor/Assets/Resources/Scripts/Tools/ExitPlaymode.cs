using UnityEngine;
using System.Collections;

public class ExitPlaymode : MonoBehaviour
{
#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
            UnityEditor.EditorApplication.isPlaying = false;
    }
#endif
}

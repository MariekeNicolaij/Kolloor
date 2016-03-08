using UnityEngine;

public class AudioBackground : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlayMusic(AudioCategory.Background, true, 0.5f);
    }
}
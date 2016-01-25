//using UnityEngine;

//public class AudioFootSteps : MonoBehaviour
//{
//    public GameObject player;
//    AudioCategory audioCategory;
//    AudioClip clip;

//    bool loop;
//    float waitTime = 2;
//    string oldTag, curTag;


//    void Start()
//    {
//        if (!player)
//            player = GameObject.FindGameObjectWithTag("Player");
//    }

//    void Update()
//    {
//        PlaySound();
//    }

//    void OnCollisionEnter(Collision other)
//    {
//        oldTag = curTag;
//        curTag = other.collider.tag;
//        CheckTagAndSetClip(curTag);
//    }

//    /// <summary>
//    /// Checks tag and sets audioSource.clip
//    /// </summary>
//    /// <param name="tag"></param>
//    void CheckTagAndSetClip(string tag)
//    {
//        if (oldTag == curTag)
//            return;

//        if (IsUnderwater(Camera.main))
//            audioCategory = AudioCategory.WaterFootsteps;
//        else if (tag == "Grass")
//            audioCategory = AudioCategory.GrassFootsteps;
//        else if (tag == "Mushroom")
//            audioCategory = AudioCategory.MushroomFootsteps;
//        else if (tag == "Stone")
//            audioCategory = AudioCategory.StoneFootsteps;
//        else if (tag == "Water")
//            audioCategory = AudioCategory.WaterFootsteps;
//        else if (tag == "Wood")
//            audioCategory = AudioCategory.WoodFootsteps;

//        clip = SoundLibrary.instance.GetClipFromAudioCategory(audioCategory);
//    }

//    /// <summary>
//    /// Play sound
//    /// </summary>
//    void PlaySound()
//    {
//        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && OnGround() && !Player.instance.playerLocked)
//        {
//            if (!AudioManager.instance.GetSuitableAudioSource(audioCategory).isPlaying)
//                AudioManager.instance.PlaySound(audioCategory, clip, new Vector3(), true, true, true, false);
//        }
//        else
//        {
//            AudioManager.instance.PauseSound(audioCategory);
//        }
//    }

//    /// <summary>
//    /// Is player on ground?
//    /// </summary>
//    /// <returns></returns>
//    bool OnGround()
//    {
//        RaycastHit hit;

//        if (Physics.Raycast(player.transform.position, Vector3.down, out hit, 1))
//            if (hit.collider)
//                return true;
//        return false;
//    }

//    bool IsUnderwater(Camera cam)
//    {
//        return cam.transform.position.y + 0.001f < transform.position.y ? true : false;
//    }
//}
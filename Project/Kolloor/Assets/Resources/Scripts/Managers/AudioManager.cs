using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SoundLibrary))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    GameObject audioListener, cam;

    Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
    string sourceText = "Source";

    public float musicVolumePercent = 1;
    public float sfxVolumePercent = 1;

    [Header("Volume")]
    [Range(0, 1)]
    [SerializeField]
    float lowVolumeRange = 0.25f;           // The lowest random volume amount.
    [Range(0, 1)]
    [SerializeField]
    float highVolumeRange = 1;              // The highest random volume amount.

    [Header("Pitch")]
    [Range(0.5f, 1.5f)]
    [SerializeField]
    float lowPitchRange = 0.95f;            // The lowest random pitch amount.
    [Range(0.5f, 1.5f)]
    [SerializeField]
    float highPitchRange = 1.05f;           // The highest random pitch amount.


    void Awake()
    {
        if (instance)                               // if there is an instance of this gameobject already, destroy it so there remains only one audiomanager
            Destroy(gameObject);
        else                                        // if not then set up audiomanager
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioListener = FindObjectOfType<AudioListener>().gameObject;
            cam = Camera.main.gameObject;

            musicVolumePercent = PlayerPrefs.GetFloat("MusicVolumePercent");
            sfxVolumePercent = PlayerPrefs.GetFloat("SfxVolumePercent");
            if (musicVolumePercent == 0)
                musicVolumePercent = 0.5f;
            if (sfxVolumePercent == 0)
                sfxVolumePercent = 1;
        }
    }

    void Update()
    {
        if (cam)
            audioListener.transform.position = cam.transform.position;
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        if (volumePercent >= 1)
            volumePercent /= 100;

        switch (channel)
        {
            case AudioChannel.Music:
            musicVolumePercent = volumePercent;
            break;
            case AudioChannel.Sfx:
            sfxVolumePercent = volumePercent;
            break;
        }

        foreach (AudioSource source in audioSources.Values)
            source.volume = source.name == AudioCategory.Background.ToString() + sourceText ?
                musicVolumePercent : sfxVolumePercent;

        PlayerPrefs.SetFloat("MusicVolumePercent", musicVolumePercent);
        PlayerPrefs.SetFloat("SfxVolumePercent", sfxVolumePercent);
    }

    public void AddAudioSource(string name)
    {
        foreach (string s in audioSources.Keys)
            if (s == name)
                return;
        GameObject newAudioSource = new GameObject(name);
        audioSources.Add(name, newAudioSource.AddComponent<AudioSource>());
        newAudioSource.transform.parent = transform;
    }

    #region PlayMusic()
    public void PlayMusic(AudioCategory audioCategory, bool loop)
    {
        PlayMusic(audioCategory, loop, 1, new Vector3(), false);
    }

    public void PlayMusic(AudioCategory audioCategory, bool loop, float volume)
    {
        PlayMusic(audioCategory, loop, volume, new Vector3(), false);
    }

    public void PlayMusic(AudioCategory audioCategory, bool loop, float volume, Vector3 playPosition, bool is3D = true)
    {
        if (!audioSources.ContainsKey(audioCategory.ToString() + sourceText))
            AddAudioSource(audioCategory.ToString() + sourceText);

        AudioSource source = GetSuitableAudioSource(audioCategory);
        source.Stop();
        source.clip = SoundLibrary.instance.GetClipFromAudioCategory(audioCategory);
        source.loop = loop;
        source.volume = volume * musicVolumePercent;
        source.transform.position = playPosition;
        source.spatialBlend = System.Convert.ToInt32(is3D);
        source.Play();
    }
    #endregion

    #region PlaySound()
    public void PlaySound(AudioCategory audioCategory)
    {
        PlaySound(audioCategory, SoundLibrary.instance.GetClipFromAudioCategory(audioCategory), new Vector3(), false, false, false, false);
    }

    public void PlaySound(AudioCategory audioCategory, bool loop)
    {
        PlaySound(audioCategory, SoundLibrary.instance.GetClipFromAudioCategory(audioCategory), new Vector3(), loop, false, false, false);
    }

    public void PlaySound(AudioCategory audioCategory, bool randomVolume, bool randomPitch)
    {
        PlaySound(audioCategory, SoundLibrary.instance.GetClipFromAudioCategory(audioCategory), new Vector3(), false, randomVolume, randomPitch, false);
    }

    public void PlaySound(AudioCategory audioCategory, bool loop, bool randomVolume, bool randomPitch)
    {
        PlaySound(audioCategory, SoundLibrary.instance.GetClipFromAudioCategory(audioCategory), new Vector3(), loop, randomVolume, randomPitch, false);
    }

    public void PlaySound(AudioCategory audioCategory, Vector3 position, bool loop, bool randomVolume, bool randomPitch, bool is3D = true)
    {
        PlaySound(audioCategory, SoundLibrary.instance.GetClipFromAudioCategory(audioCategory), position, loop, randomVolume, randomPitch, is3D);
    }

    public void PlaySound(AudioCategory audioCategory, AudioClip clip, Vector3 position, bool loop, bool randomVolume, bool randomPitch, bool is3D = true)
    {
        if (!audioSources.ContainsKey(audioCategory.ToString() + sourceText))
            AddAudioSource(audioCategory.ToString() + sourceText);

        AudioSource source = GetSuitableAudioSource(audioCategory);
        RandomizeVolumeAndPitch(source, AudioChannel.Sfx, randomVolume, randomPitch);

        source.Stop();
        source.clip = clip;
        source.loop = loop;
        source.volume = 100;
        source.spatialBlend = System.Convert.ToInt32(is3D);
        source.transform.position = position;
        source.Play();
    }
    #endregion

    public void PauseSound(AudioCategory audioCategory)
    {
        AudioSource source = GetSuitableAudioSource(audioCategory);
        source.Pause();
    }

    public void UnPauseSound(AudioCategory audioCategory)
    {
        AudioSource source = GetSuitableAudioSource(audioCategory);
        source.UnPause();
    }

    public void StopSound(AudioCategory audioCategory)
    {
        AudioSource source = GetSuitableAudioSource(audioCategory);
        source.Stop();
    }

    public AudioSource GetSuitableAudioSource(AudioCategory audioCategory)
    {
        AudioSource source = new AudioSource();
        string sourceName = audioCategory.ToString() + sourceText;

        foreach (string name in audioSources.Keys)
            if (name == sourceName)
                audioSources.TryGetValue(name, out source);

        //Debug.Log("Source: " + source);
        return source;
    }

    /// <summary>
    /// Sets random volume and pitch and index for
    /// </summary>
    void RandomizeVolumeAndPitch(AudioSource source, AudioChannel channel, bool randomVolume, bool randomPitch)
    {
        if (randomVolume)
        {
            switch (channel)
            {
                case AudioChannel.Music:
                source.volume = Random.Range(lowVolumeRange * musicVolumePercent, highVolumeRange * musicVolumePercent);
                break;
                case AudioChannel.Sfx:
                source.volume = Random.Range(lowVolumeRange * sfxVolumePercent, highVolumeRange * sfxVolumePercent);
                break;
            }
        }
        if (randomPitch)
            source.pitch = Random.Range(lowPitchRange, highPitchRange);
    }
}

public enum AudioChannel
{
    Music,
    Sfx
}
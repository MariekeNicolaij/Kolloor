//using UnityEngine;
//using System.Collections.Generic;

//[RequireComponent(typeof(SoundLibrary))]
//public class AudioManager : MonoBehaviour
//{
//    public static AudioManager instance;

//    GameObject audioListener, player;

//    Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

//    float masterVolumePercent = 1;
//    float musicVolumePercent = 1;
//    float sfxVolumePercent = 1;

//    [Header("Volume")]
//    [Range(0, 1)]
//    [SerializeField]
//    float lowVolumeRange = 0.25f;           // The lowest random volume amount.
//    [Range(0, 1)]
//    [SerializeField]
//    float highVolumeRange = 1;              // The highest random volume amount.

//    [Header("Pitch")]
//    [Range(0.5f, 1.5f)]
//    [SerializeField]
//    float lowPitchRange = 0.95f;            // The lowest random pitch amount.
//    [Range(0.5f, 1.5f)]
//    [SerializeField]
//    float highPitchRange = 1.05f;           // The highest random pitch amount.

//    string footSteps = "Footsteps", waterSplashes = "Watersplashes", plantCollision = "PlantCollision", boatCollision = "BoatCollsion", boatConnect = "BoatConnect", boatMovement = "BoatMovement", aircraftHelicopterBlades = "AircraftHelicopterBlades", aircraftWind = "AircraftWind", elevator = "Elevator", natureAnimals = "NatureAnimals", natureWind = "NatureWind", natureCherryBlossom = "NatureCherryBlossom", background = "Background", waterfall = "Waterfall";


//    void Awake()
//    {
//        if (instance)                               // if there is an instance of this gameobject already, destroy it so there remains only one audiomanager
//            Destroy(gameObject);
//        else                                        // if not then set up audiomanager
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);

//            audioListener = FindObjectOfType<AudioListener>().gameObject;
//            player = FindObjectOfType<Player>().gameObject;

//            AddAudioSource(footSteps);
//            AddAudioSource(waterSplashes);
//            AddAudioSource(plantCollision);
//            AddAudioSource(boatCollision);
//            AddAudioSource(boatConnect);
//            AddAudioSource(boatMovement);
//            AddAudioSource(aircraftHelicopterBlades);
//            AddAudioSource(aircraftWind);
//            AddAudioSource(elevator);
//            AddAudioSource(natureAnimals);
//            AddAudioSource(natureWind);
//            AddAudioSource(natureCherryBlossom);
//            AddAudioSource(background);
//            AddAudioSource(waterfall);
//        }
//    }

//    void Update()
//    {
//        if (player)
//            audioListener.transform.position = player.transform.position;
//    }

//    public void SetVolume(float volumePercent, AudioChannel channel)
//    {
//        if (volumePercent > 1)
//            volumePercent /= 100;

//        switch (channel)
//        {
//            case AudioChannel.Master:
//            masterVolumePercent = volumePercent;
//            break;
//            case AudioChannel.Music:
//            musicVolumePercent = volumePercent;
//            break;
//            case AudioChannel.Sfx:
//            sfxVolumePercent = volumePercent;
//            break;
//        }

//        foreach (AudioSource source in audioSources.Values)
//            source.volume = source.name == "Music source" ?
//                musicVolumePercent * masterVolumePercent : sfxVolumePercent * masterVolumePercent;
//    }

//    #region AddAudioSource()
//    public void AddAudioSource(string name)
//    {
//        foreach (string s in audioSources.Keys)
//            if (s == name)
//                return;
//        GameObject newAudioSource = new GameObject(name);
//        audioSources.Add(name, newAudioSource.AddComponent<AudioSource>());
//        newAudioSource.transform.parent = transform;
//    }

//    public void AddAudioSource(string name, AudioClip clip)
//    {
//        foreach (string s in audioSources.Keys)
//            if (s == name)
//                return;
//        GameObject newAudioSource = new GameObject(name);
//        audioSources.Add(name, newAudioSource.AddComponent<AudioSource>());
//        newAudioSource.GetComponent<AudioSource>().clip = clip;
//        newAudioSource.transform.parent = transform;
//    }

//    public void AddAudioSource(string name, AudioClip clip, bool loop, bool playNow)
//    {
//        foreach (string s in audioSources.Keys)
//            if (s == name)
//                return;
//        GameObject newAudioSource = new GameObject(name);
//        audioSources.Add(name, newAudioSource.AddComponent<AudioSource>());
//        newAudioSource.GetComponent<AudioSource>().clip = clip;
//        newAudioSource.GetComponent<AudioSource>().loop = loop;
//        if (playNow) newAudioSource.GetComponent<AudioSource>().Play();
//        newAudioSource.transform.parent = transform;
//    }
//    #endregion

//    #region PlayMusic()
//    public void PlayMusic(AudioCategory audioCategory, bool loop)
//    {
//        PlayMusic(audioCategory, loop, new Vector3(), false);
//    }

//    public void PlayMusic(AudioCategory audioCategory, bool loop, Vector3 playPosition, bool is3D)
//    {
//        AudioSource source = GetSuitableAudioSource(audioCategory);
//        source.Stop();
//        source.clip = SoundLibrary.instance.GetClipFromAudioCategory(audioCategory);
//        source.loop = loop;
//        source.transform.position = playPosition;
//        source.spatialBlend = System.Convert.ToInt32(is3D);
//        source.Play();
//    }
//    #endregion

//    #region PlaySound()
//    public void PlaySound(AudioCategory audioCategory)
//    {
//        PlaySound(audioCategory, SoundLibrary.instance.GetClipFromAudioCategory(audioCategory), new Vector3(), false, false, false, false);
//    }

//    public void PlaySound(AudioCategory audioCategory, bool loop)
//    {
//        PlaySound(audioCategory, SoundLibrary.instance.GetClipFromAudioCategory(audioCategory), new Vector3(), loop, false, false, false);
//    }

//    public void PlaySound(AudioCategory audioCategory, bool randomVolume, bool randomPitch)
//    {
//        PlaySound(audioCategory, SoundLibrary.instance.GetClipFromAudioCategory(audioCategory), new Vector3(), false, randomVolume, randomPitch, false);
//    }

//    public void PlaySound(AudioCategory audioCategory, bool loop, bool randomVolume, bool randomPitch)
//    {
//        PlaySound(audioCategory, SoundLibrary.instance.GetClipFromAudioCategory(audioCategory), new Vector3(), loop, randomVolume, randomPitch, false);
//    }

//    public void PlaySound(AudioCategory audioCategory, Vector3 position, bool loop, bool randomVolume, bool randomPitch, bool is3D)
//    {
//        PlaySound(audioCategory, SoundLibrary.instance.GetClipFromAudioCategory(audioCategory), position, loop, randomVolume, randomPitch, is3D);
//    }
    
//    public void PlaySound(AudioCategory audioCategory, AudioClip clip, Vector3 position, bool loop, bool randomVolume, bool randomPitch, bool is3D)
//    {
//        AudioSource source = GetSuitableAudioSource(audioCategory);
//        RandomizeVolumeAndPitch(source, AudioChannel.Sfx, randomVolume, randomPitch);

//        source.Stop();
//        source.clip = clip;
//        source.loop = loop;
//        source.spatialBlend = System.Convert.ToInt32(is3D);
//        source.transform.position = position;
//        source.Play();
//    }
//    #endregion

//    public void PauseSound(AudioCategory audioCategory)
//    {
//        AudioSource source = GetSuitableAudioSource(audioCategory);
//        source.Pause();
//    }

//    public void UnPauseSound(AudioCategory audioCategory)
//    {
//        AudioSource source = GetSuitableAudioSource(audioCategory);
//        source.UnPause();
//    }

//    public void StopSound(AudioCategory audioCategory)
//    {
//        AudioSource source = GetSuitableAudioSource(audioCategory);
//        source.Stop();
//    }

//    public AudioSource GetSuitableAudioSource(AudioCategory audioCategory)
//    {
//        AudioSource source = new AudioSource();
//        string sourceName = "";

//        switch (audioCategory)
//        {
//            case AudioCategory.DefaultFootsteps:
//            sourceName = footSteps;
//            break;
//            case AudioCategory.GrassFootsteps:
//            sourceName = footSteps;
//            break;
//            case AudioCategory.MushroomFootsteps:
//            sourceName = footSteps;
//            break;
//            case AudioCategory.StoneFootsteps:
//            sourceName = footSteps;
//            break;
//            case AudioCategory.WaterFootsteps:
//            sourceName = footSteps;
//            break;
//            case AudioCategory.WoodFootsteps:
//            sourceName = footSteps;
//            break;
//            case AudioCategory.WaterSplashes:
//            sourceName = waterSplashes;
//            break;
//            case AudioCategory.PlantCollision:
//            sourceName = plantCollision;
//            break;
//            case AudioCategory.BoatCollision:
//            sourceName = boatCollision;
//            break;
//            case AudioCategory.BoatConnect:
//            sourceName = boatConnect;
//            break;
//            case AudioCategory.BoatMovement:
//            sourceName = boatMovement;
//            break;
//            case AudioCategory.AircraftHelicopterBlades:
//            sourceName = aircraftHelicopterBlades;
//            break;
//            case AudioCategory.AircraftWind:
//            sourceName = aircraftWind;
//            break;
//            case AudioCategory.ElevatorRattlingSteel:
//            sourceName = elevator;
//            break;
//            case AudioCategory.ElevatorRattlingLiana:
//            sourceName = elevator;
//            break;
//            case AudioCategory.ElevatorConnect:
//            sourceName = elevator;
//            break;
//            case AudioCategory.NatureAnimals:
//            sourceName = natureAnimals;
//            break;
//            case AudioCategory.NatureWind:
//            sourceName = natureWind;
//            break;
//            case AudioCategory.NatureCherryBlossom:
//            sourceName = natureCherryBlossom;
//            break;
//            case AudioCategory.Background:
//            sourceName = background;
//            break;
//            case AudioCategory.Waterfall:
//            sourceName = waterfall;
//            break;
//        }

//        foreach (string name in audioSources.Keys)
//            if (name == sourceName)
//                audioSources.TryGetValue(name, out source);

//        //Debug.Log("Source: " + source);
//        return source;
//    }

//    /// <summary>
//    /// Sets random volume and pitch and index for
//    /// </summary>
//    void RandomizeVolumeAndPitch(AudioSource source, AudioChannel channel, bool randomVolume, bool randomPitch)
//    {
//        if (randomVolume)
//        {
//            switch (channel)
//            {
//                case AudioChannel.Music:
//                source.volume = Random.Range(lowVolumeRange * musicVolumePercent, highVolumeRange * musicVolumePercent);
//                break;
//                case AudioChannel.Sfx:
//                source.volume = Random.Range(lowVolumeRange * sfxVolumePercent, highVolumeRange * sfxVolumePercent);
//                break;
//            }
//        }
//        if (randomPitch)
//            source.pitch = Random.Range(lowPitchRange, highPitchRange);
//    }
//}

//public enum AudioChannel
//{
//    Master,
//    Music,
//    Sfx
//}
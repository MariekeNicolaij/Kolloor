﻿//using UnityEngine;
//using System.Collections.Generic;

//public class SoundLibrary : MonoBehaviour
//{
//    public static SoundLibrary instance;
//    [Header("Add sounds here")]
//    public SoundGroup[] soundGroups;

//    Dictionary<AudioCategory, AudioClip[]> audioGroupDictionary = new Dictionary<AudioCategory, AudioClip[]>();

//    int oldIndex, newIndex;

//    void Awake()
//    {
//        if (!instance)
//            instance = this;
//        foreach (SoundGroup soundGroup in soundGroups)
//            audioGroupDictionary.Add(soundGroup.category, soundGroup.sounds);
//    }

//    public AudioClip GetClipFromAudioCategory(AudioCategory audioCategory)
//    {
//        if (audioGroupDictionary.ContainsKey(audioCategory))
//        {
//            AudioClip[] sounds = audioGroupDictionary[audioCategory];
//            if (sounds.Length > 0)
//                return sounds[IndexCheck(sounds)];
//        }
//        return null;
//    }

//    /// <summary>
//    /// Prevents previous sound from playing again
//    /// </summary>
//    /// <param name="sounds"></param>
//    /// <returns></returns>
//    int IndexCheck(AudioClip[] sounds)
//    {
//        oldIndex = newIndex;
//        newIndex = Random.Range(0, sounds.Length);

//        if (oldIndex == newIndex)
//            if (sounds.Length > 1)
//                IndexCheck(sounds);

//        return newIndex;
//    }

//    [System.Serializable]
//    public class SoundGroup
//    {
//        public AudioCategory category;
//        [Tooltip("Add all sounds of this category")]
//        public AudioClip[] sounds;
//    }
//}

//public enum AudioCategory
//{
//    DefaultFootsteps,//
//    GrassFootsteps,//
//    MushroomFootsteps,//
//    StoneFootsteps,//
//    WaterFootsteps,//
//    WoodFootsteps,//
//    WaterSplashes,//
//    PlantCollision,//
//    BoatCollision,//
//    BoatConnect,//
//    BoatMovement,//
//    AircraftHelicopterBlades,//
//    AircraftWind,//
//    ElevatorRattlingSteel,//
//    ElevatorRattlingLiana,//
//    ElevatorConnect,//
//    NatureAnimals,//
//    NatureWind,//
//    NatureCherryBlossom,//
//    Background,//
//    Waterfall//
//}
//using UnityEngine;
//using System.Collections;

//public class AudioNature : MonoBehaviour
//{
//    public NatureType natureType;
//    GameObject waterfall;

//    float animalTime = 3, windTime = 4, cherryTime = 5;
//    [Range(0, 30)]
//    public float minWaitTime = 15;
//    [Range(0, 60)]
//    public float maxWaitTime = 50;


//    void Start()
//    {
//        waterfall = GameObject.FindGameObjectWithTag("Waterfall");
//        if (waterfall)
//            Waterfall();
//    }

//    void Update()
//    {
//        AnimalTimer();
//        CherryTimer();
//        WindTimer();
//    }

//    void AnimalTimer()
//    {
//        animalTime -= Time.smoothDeltaTime;

//        if (animalTime < 0)
//        {
//            AudioSource source = AudioManager.instance.GetSuitableAudioSource(AudioCategory.NatureAnimals);
//            if (!source.isPlaying)
//                AudioManager.instance.PlaySound(AudioCategory.NatureAnimals, RandomPos(transform.position), false, true, true, true);
//            if (source.clip)
//                animalTime = Random.Range(source.clip.length + minWaitTime,
//                                          source.clip.length + maxWaitTime);
//        }
//    }

//    void WindTimer()
//    {
//        windTime -= Time.smoothDeltaTime;

//        if (windTime < 0)
//        {
//            AudioSource source = AudioManager.instance.GetSuitableAudioSource(AudioCategory.NatureWind);
//            if (!source.isPlaying)
//                AudioManager.instance.PlaySound(AudioCategory.NatureWind, RandomPos(transform.position), false, true, true, true);
//            if (source.clip)
//                windTime = Random.Range(source.clip.length + minWaitTime,
//                                      source.clip.length + maxWaitTime);
//        }
//    }

//    void CherryTimer()
//    {
//        cherryTime -= Time.smoothDeltaTime;

//        if (cherryTime < 0)
//        {
//            AudioSource source = AudioManager.instance.GetSuitableAudioSource(AudioCategory.NatureCherryBlossom);
//            if (!source.isPlaying)
//                AudioManager.instance.PlaySound(AudioCategory.NatureCherryBlossom, RandomPos(transform.position), false, true, true, true);
//            if (source.clip)
//                cherryTime = Random.Range(source.clip.length + minWaitTime,
//                                      source.clip.length + maxWaitTime);
//        }
//    }

//    void Waterfall()
//    {
//        AudioManager.instance.PlaySound(AudioCategory.Waterfall, waterfall.transform.position, true, false, false, true);
//    }

//    Vector3 RandomPos(Vector3 basePosition)
//    {
//        float min = -10, max = 10;
//        return basePosition + new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
//    }

//    public enum NatureType
//    {
//        Animal,
//        Cherry,
//        Waterfall,
//        Wind
//    }
//}
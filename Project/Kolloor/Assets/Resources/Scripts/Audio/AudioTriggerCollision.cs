//using UnityEngine;

//public class AudioTriggerCollision : MonoBehaviour
//{
//    void OnCollisionEnter(Collision other)
//    {
//        if (tag == "Vehicle")
//        {
//            if (other.collider.tag == "Stone")
//                AudioManager.instance.PlaySound(AudioCategory.BoatCollision, false, true, true);
//            if (other.collider.tag == "Wood")
//                AudioManager.instance.PlaySound(AudioCategory.BoatCollision, false, true, true);
//            if (other.collider.tag == "Grass")
//                AudioManager.instance.PlaySound(AudioCategory.BoatCollision, false, true, true);
//        }
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (tag == "Player")
//        {
//            if (other.tag == "Plant")
//                AudioManager.instance.PlaySound(AudioCategory.PlantCollision, false, true, true);
//        }
//    }
//}
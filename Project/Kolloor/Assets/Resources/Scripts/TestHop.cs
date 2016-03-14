using UnityEngine;
using System.Collections;

public class TestHop : MonoBehaviour
{
    NavMeshAgent agent;

    [Range(0, 250)]
    public float minForce = 10, maxForce = 100;
    float force;
    float distance;
    float time = 2;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        distance = transform.localScale.z;
    }

    void Update()
    {
        Hop();
    }

    void Hop()
    {
        time -= Time.smoothDeltaTime;

        if (time > 0)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
            {
                if (hit.collider)
                {
                    force = Random.Range(minForce, maxForce);
                    agent.enabled = false;
                    GetComponent<Rigidbody>().isKinematic = false;
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<Rigidbody>().velocity = Vector3.zero;                              // Zodat hij niet hoger en hoger gaat bouncen
                    GetComponent<Rigidbody>().AddForce(Vector3.up * force);                         // Bounce
                }
            }
        }
        else
        {
            agent.enabled = true;
            time = Random.Range(1, 3);
        }
    }
}
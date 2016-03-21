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
    Vector3 position;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        distance = transform.localScale.z;
    }

    void Update()
    {
        position = Player.instance.transform.position;

        agent.SetDestination(position);
    }
}
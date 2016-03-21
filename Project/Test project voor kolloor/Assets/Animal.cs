using UnityEngine;
using System.Collections;

public class Animal : MonoBehaviour
{
    Vector3 pos = Vector3.zero;

    NavMeshAgent agent;

    LayerMask area;

    public aiType AItype;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        switch (AItype)
        {
            case aiType.ground:
                    area = 1 << NavMesh.GetAreaFromName("Terrain");
                    break;
            case aiType.water:
                area = 1 << NavMesh.GetAreaFromName("Water");
                break;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");

            RaycastHit hit = new RaycastHit();

            NavMeshHit navHit = new NavMeshHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Debug.Log("hit");

                Debug.Log(area);
                Debug.Log(agent.areaMask);

                if (NavMesh.SamplePosition(hit.point, out navHit, 100, area))
                {
                    Debug.Log("pos");


                    pos = navHit.position;
                }
            }
        }

        if (pos != Vector3.zero)
            agent.SetDestination(pos);
    }

    public enum aiType
    {
        ground,
        water
    }
}

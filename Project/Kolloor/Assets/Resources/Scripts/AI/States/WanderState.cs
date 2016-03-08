using UnityEngine;
using System.Collections;
using System.Linq;

namespace AI.States
{
    public class WanderState : State
    {
        private int corner = 0;
        private bool wait = false;
        private float WaitTime = 0;

        public override void Enter()
        {
            // the owner always get's a new point to wander to
            Owner.path = Owner.aiManager.GetRandomPath(Owner.transform.position);
        }

        public override void Execute()
        {
            //Debug.Log(corner);
            //Debug.Log(Owner.path);
            //Debug.Log(Owner.path.corners.Length);
            //Debug.Log(Owner.path.corners[corner]);

            Debug.DrawLine(Owner.transform.position, Owner.path.corners[corner], Color.red);

            for (int i = 0; i < Owner.path.corners.Length - 1; i++)
            {
                Debug.DrawLine(Owner.path.corners[i], Owner.path.corners[i + 1], Color.red);
            }

            if (!wait)
            {
                if (Vector3.Distance(Owner.path.corners.Last(), Owner.transform.position) <= Owner.maxPointDistance)
                {
                    Owner.path = Owner.aiManager.GetRandomPath(Owner.transform.position);
                    corner = 0;
                    wait = true;
                    WaitTime = Random.Range(Owner.MinWaitTime, Owner.MaxWaitTime);
                }
                else {
                    Vector3 VecToLookAt = Owner.path.corners[corner];
                    VecToLookAt.y = Owner.transform.position.y;

                    if (Vector3.Distance(Owner.path.corners[corner], Owner.transform.position) <= Owner.maxPointDistance)
                    {
                        corner++;
                        wait = System.Convert.ToBoolean(Random.Range(0, 1));
                        if (wait)
                            WaitTime = Random.Range(Owner.MinWaitTime, Owner.MaxWaitTime);
                    }
                    else {
                        Owner.LookAt(VecToLookAt);
                        Owner.Move();
                    }
                }
            }
            else
            {
                WaitTime -= Time.smoothDeltaTime;
                if (WaitTime < 0)
                {
                    wait = false;
                }

            }
        }

        public override void Exit() { }
    }
}
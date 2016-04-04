using UnityEngine;
using Managers;

namespace AI
{
    public class WaterAI : GroundWaterBaseAI
    {
        private bool MoveUp = false;

        bool MoveUpDone = false;

        protected override void Start()
        {
            type = AITypes.WaterAI;

            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            if (MoveUp)
            {
                if (transform.position.y-aiManager.Water.transform.position.y > -maxPointDistance)
                {
                    transform.Translate(Vector3.up * Time.smoothDeltaTime);
                }
                else
                {
                    MoveUp = false;
                    MoveUpDone = true;
                }
            }

            //if (dive)
            //    Dive();
        }

        protected override void enableAfterDrop()
        {
            if (!MoveUpDone)
            {
                if (aiManager.Water.transform.position.y - transform.position.y < 0.1 && aiManager.Water.transform.position.y - transform.position.y > -0.1)
                    base.enableAfterDrop();
                else if (aiManager.Water.transform.position.y > transform.position.y)
                    MoveUp = true;
                else {
                    Respawn();
                    base.enableAfterDrop();
                }
            }
            else
            {
                base.enableAfterDrop();
                MoveUpDone = false;
            }
        }

        protected override void MoveForward()
        {
            //if (dive)
            //    return;

            //if (FallenTrunkLayer != -1)
            //{
            //    RaycastHit hit = new RaycastHit();

            //    Ray ray = new Ray(transform.position, transform.forward);

            //    Debug.DrawRay(ray.origin, ray.direction);

            //    if (Physics.Raycast(ray, out hit))
            //    {
            //        if (hit.collider.gameObject.layer == FallenTrunkLayer)
            //        {
            //            dive = true;
            //            trunk = hit.collider.gameObject;
            //            trunkCol = hit.collider;
            //        }
            //    }
            //}
            base.MoveForward();
        }
    }
}
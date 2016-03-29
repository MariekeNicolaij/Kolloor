using UnityEngine;
using Managers;

namespace AI
{
    public class WaterAI : GroundWaterBaseAI
    {
        public int FallenTrunkLayer = -1;

        private bool MoveUp = false;

        bool MoveUpDone = false;

        //private bool dive = false;
        private GameObject trunk;
        private Collider trunkCol;

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

        private void Dive()
        {
            if (agent.enabled)
                agent.enabled = false;

            if (trunk.transform.position.y - (trunkCol.bounds.size.y * .5f) > transform.position.y)
                transform.Translate(Vector3.down * Time.smoothDeltaTime);
            else
                transform.Translate(Vector3.forward * Time.smoothDeltaTime * MovementSpeed);
        }
    }
}
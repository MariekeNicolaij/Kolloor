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

        //protected override void Update()
        //{
        //    base.Update();

        //    if (MoveUp)
        //    {
        //        if (transform.position.y-aiManager.Water.transform.position.y > -maxPointDistance)
        //        {
        //            transform.Translate(Vector3.up * Time.smoothDeltaTime);
        //        }
        //        else
        //        {
        //            MoveUp = false;
        //            MoveUpDone = true;
        //        }
        //    }
        //}

        //protected override void enableAfterDrop()
        //{
        //    if (!MoveUpDone)
        //    {
        //        if (aiManager.Water.transform.position.y - transform.position.y < 0.1 && aiManager.Water.transform.position.y - transform.position.y > -0.1)
        //            base.enableAfterDrop();
        //        else if (aiManager.Water.transform.position.y > transform.position.y)
        //            MoveUp = true;
        //        else {
        //            Respawn();
        //            base.enableAfterDrop();
        //        }
        //    }
        //    else
        //    {
        //        base.enableAfterDrop();
        //        MoveUpDone = false;
        //    }
        //}
    }
}
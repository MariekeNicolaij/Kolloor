using UnityEngine;
using Managers;

namespace AI
{
    public class WaterAI : BaseAI
    {
        public bool UnderWater = false;

        public float MaxAboveWater = 2;
        public float MinAboveWater = 2;

        private GameObject Water;

        protected override void Start()
        {
            type = AITypes.WaterAI;

            base.Start();

            if (rigidBody.useGravity)
                rigidBody.useGravity = false;

            Water = AIManager.instance.Water;
        }

        protected override void Update()
        {
            base.Update();

            if (!UnderWater)
            {
                if (transform.position.y < Water.transform.position.y)
                    transform.Translate(Vector3.up * Time.smoothDeltaTime);
                else if (Water.transform.position.y + MaxAboveWater < transform.position.y)
                    transform.Translate(Vector3.down * Time.smoothDeltaTime);
            }
            else
            {

            }
        }

        public override void MoveForward()
        {
            Vector3 DownForward = Vector3.forward;
            DownForward.y -= .5f;

            Debug.DrawRay(transform.position, DownForward, Color.blue);

            Ray ray = new Ray(transform.position, DownForward);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, 1))
            {
                if (hit.collider.gameObject.layer != (int)Layers.Water)
                {
                    return;
                }
            }
            base.MoveForward();
        }

        public override void OnCollisionExit(Collision other)
        {
            base.OnCollisionExit(other);

            if (other.gameObject.layer == (int)Layers.Water)
            {
                Debug.LogError("WaterAi " + this.ID + " on pos " + transform.position + " isn't in contact with water anymore!");
            }
        }
    }
}
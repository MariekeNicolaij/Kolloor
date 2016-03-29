using UnityEngine;
using System.Collections;

namespace AI
{
    public class AirAI : BaseAI
    {
        /// <summary>
        /// The max hight above the ground
        /// </summary>
        public float MaxFlighHight = 20;
        /// <summary>
        /// Tne minimal height above the ground
        /// </summary>
        public float MinFlighHight = 15;

        protected override void Start()
        {
            type = AITypes.AirAI;
            base.Start();
        }

    }
}

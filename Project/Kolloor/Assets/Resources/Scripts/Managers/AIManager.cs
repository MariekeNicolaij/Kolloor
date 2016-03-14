using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI;
using System.Linq;

namespace Managers
{
    public class AIManager : MonoBehaviour
    {
        public static AIManager instance;

        #region ID
        private Dictionary<int, BaseAI> AIs = new Dictionary<int, BaseAI>();
        private Dictionary<int, AITypes> AIIDTypes = new Dictionary<int, AITypes>();
        private int ID = 0;
        #endregion

        #region AI help
        public int TimeTillHelp = 60;

        private bool timerOn = true;
        private float timer = 0;
        #endregion

        private Collider terrainCollider;
        private Terrain terrain;

        void Awake()
        {
            instance = this;

            terrain = Terrain.activeTerrain;
            terrainCollider = Terrain.activeTerrain.GetComponent<Collider>();
        }

        void Update()
        {
            if (PuzzleObjectManager.instance.ObjectsAmound() > 0 && timerOn)
            {
                timer += Time.smoothDeltaTime;

                if (timer >= TimeTillHelp * 10)
                    AiHelp(true);

            }
        }

        /// <summary>
        /// for getting an Random point within the navmesh;
        /// </summary>
        /// <returns> returns a random vector3 within the nav mesh </returns>
        public Vector3 CreateRandomPoint()
        {
            Vector3 vec = new Vector3();

                vec.x = Random.Range(terrain.transform.position.x, terrainCollider.bounds.size.x);
                vec.z = Random.Range(terrain.transform.position.z, terrainCollider.bounds.size.z);

            NavMeshHit hit;

            NavMesh.SamplePosition(vec, out hit, terrainCollider.bounds.size.x, NavMesh.AllAreas);

            return hit.position;
        }

        #region AI ID system

        /// <summary>
        /// Adds an ai to the list so it exists
        /// </summary>
        /// <param name="AI"> The Ai to add</param>
        /// <returns> returns the new given ID of the just added AI </returns>
        public int Register(BaseAI AI, AITypes Type)
        {
            ID++;
            AIs.Add(ID, AI);
            AIIDTypes.Add(ID, Type);
            return ID;
        }

        /// <summary>
        /// Remove An AI by its ID
        /// </summary>
        /// <param name="ID"> the ID of the ai to remove </param>
        public void RemoveAI(int ID)
        {
            AIs.Remove(ID);
        }

        /// <summary>
        /// Remove An AI by its ID
        /// </summary>
        /// <param name="AI"> the AI to Remove </param>
        public void RemoveAI(BaseAI AI)
        {
            AIs.Remove(GetID(AI));
        }

        /// <summary>
        /// get the ID of an AI
        /// </summary>
        /// <param name="AI"> the Ai from which you want the ID </param>
        /// <returns> returns the id of the given AI </returns>
        public int GetID(BaseAI AI)
        {
            return AIs.FirstOrDefault(i => i.Value == AI).Key;
        }

        /// <summary>
        /// get the AI by its ID
        /// </summary>
        /// <param name="ID"> the ID of the AI you want </param>
        /// <returns> returns the corresponding AI </returns>
        public BaseAI GetAI(int ID)
        {
            return AIs[ID];
        }
        #endregion

        #region HelpTimer System
        /// <summary>
        /// for setting the AI help timer on or off
        /// </summary>
        /// <param name="status"> if true, timer is on </param>
        public void SetTimer(bool status)
        {
            timerOn = status;

            if (status)
                timer = 0;
            else
                AiHelp(false);
        }

        private void AiHelp(bool help)
        {
            for (int i = 0; i < AIIDTypes.Count; i++)
            {
                if (AIIDTypes[i] == AITypes.GroundAI)
                {
                    AIs[i].HelpPlayer(help);
                }
            }

            if (help)
                timerOn = false;
        }
        #endregion
    }
}
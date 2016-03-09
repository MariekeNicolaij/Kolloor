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

        #region Random Walkpoints
        public GameObject Water;

        public int WalkPointsAmound = 100;
        private Vector3[] WalkPoints;
        #endregion

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

            if (Water == null)
            {
                Debug.LogError("AIManager doesn't contain an Water GameObject");
            }

            WalkPoints = new Vector3[WalkPointsAmound];

            terrain = Terrain.activeTerrain;
            terrainCollider = Terrain.activeTerrain.GetComponent<Collider>();

            CreateRandomPoints();
        }

        void Update()
        {
            for (int i = 0; i < WalkPointsAmound - 1; i++)
            {
                Debug.DrawLine(WalkPoints[i], WalkPoints[i + 1], Color.black);
            }

            if (PuzzleObjectManager.instance.ObjectsAmound() > 0 && timerOn)
            {
                timer += Time.smoothDeltaTime;

                if (timer >= TimeTillHelp * 10)
                    AiHelp(true);

            }
        }

        #region creating random point system

        /// <summary>
        /// for filling the WalkPoints list with RandomPositions
        /// </summary>
        private void CreateRandomPoints()
        {
            for (int i = 0; i < WalkPointsAmound; i++)
            {
                Vector3 ranPos = CreateRandomPoint();

                if (ranPos == null)
                    ranPos = CreateRandomPoint();

                WalkPoints[i] = ranPos;
            }
        }

        /// <summary>
        /// creates a few random vectors and gets the first one thats good
        /// </summary>
        /// <param name="terrainCollider"> the terain collider of the current terrain </param>
        /// <returns> returns a random vector3 </returns>
        public Vector3 CreateRandomPoint()
        {
            Vector3[] vecs = new Vector3[3];

            for (int i = 0; i < 3; i++)
            {
                vecs[i].x = Random.Range(terrain.transform.position.x, terrainCollider.bounds.size.x);
                vecs[i].z = Random.Range(terrain.transform.position.z, terrainCollider.bounds.size.z);
                vecs[i].y = Terrain.activeTerrain.SampleHeight(vecs[i]);

            }
            return vecs.FirstOrDefault(v=>v.y > Water.transform.position.y);
        }
        #endregion

        /// <summary>
        /// for making an random path to a random point within the random points created in the start
        /// </summary>
        /// <param name="AIPosition"> the position of the ai </param>
        /// <returns>returns a random path</returns>
        public NavMeshPath GetRandomPath(Vector3 AIPosition)
        {
            int randomIndex = Random.Range(0, WalkPointsAmound);

            NavMeshPath path = new NavMeshPath();

            while (!NavMesh.CalculatePath(AIPosition, WalkPoints[randomIndex], NavMesh.AllAreas, path))
            {
                WalkPoints[randomIndex] = CreateRandomPoint();
            }
            return path;
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
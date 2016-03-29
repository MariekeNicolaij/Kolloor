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

        public int WaypointAmound = 100;

        private List<Vector3> TerrainWayPoints = new List<Vector3>();
        private List<Vector3> WaterWayPoints = new List<Vector3>();

        public Terrain terrain;
        public GameObject Water;

        public bool IceLevel = false;

        void Awake()
        {
            instance = this;

            if (terrain == null)
                Debug.LogError("the ai manager needs the terrain");
            if (Water == null)
            {
                Debug.LogWarning("Ai manager doesn't contain an WaterGameobject, if this is an ice level it isn't a problem, else ad water");
                IceLevel = true;
            }

            terrainCollider = terrain.GetComponent<Collider>();

            CreateRandomPoints();
        }

        void Update()
        {
            if (PuzzleObjectManager.instance.ObjectsAmound() > 0 && timerOn)
            {
                timer += Time.smoothDeltaTime;

                if (timer >= TimeTillHelp * 10)
                    AiHelp(true);
            }

            if (!IceLevel && WaterWayPoints.Count <= 5)
            {
                NavMeshHit hit = new NavMeshHit();

                LayerMask area = 1 << NavMesh.GetAreaFromName("Water");

                for (int i = 0; i < 5; i++)
                {
                    if (NavMesh.SamplePosition(TerrainWayPoints[i], out hit, 100, area)) ;
                    WaterWayPoints.Add(hit.position);

                }
            }
        }

        private void CreateRandomPoints()
        {
            for (int i = 0; i < WaypointAmound; i++)
            {
                CreateRandomPoint();
            }
        }

        private void CreateRandomPoint(int index = -1, Vector3 pos = new Vector3())
        {
            Vector3 vec = new Vector3();

            vec.x = Random.Range(terrain.transform.position.x, terrainCollider.bounds.size.x);
            vec.z = Random.Range(terrain.transform.position.z, terrainCollider.bounds.size.z);
            vec.y = terrain.SampleHeight(vec);

            if (IceLevel)
            {
                if (index == -1)
                    TerrainWayPoints.Add(vec);
                else
                    TerrainWayPoints[index] = vec;
            }
            else
            {
                if (index != -1)
                {
                    if (TerrainWayPoints[index] == pos)
                        TerrainWayPoints.Remove(pos);
                    else
                        WaterWayPoints.Remove(pos);
                }

                if (vec.y < Water.transform.position.y)
                {
                    vec.y = Water.transform.position.y;
                    WaterWayPoints.Add(vec);
                }
                else
                    TerrainWayPoints.Add(vec);
            }
        }

        public Vector3 GetRandomPoint(Vector3 pos, AITypes aiType, LayerMask area)
        {
            List<Vector3> list = new List<Vector3>();

            bool Return = false;

            switch (aiType)
            {
                case AITypes.GroundAI:
                    list = TerrainWayPoints;
                    break;
                case AITypes.WaterAI:
                    if (!IceLevel)
                        list = WaterWayPoints;
                    else
                        Debug.LogError("there is no water in the Ai manager");
                    break;
                case AITypes.AirAI:
                    bool r = Random.value > 0.5;

                    if (r)
                        list = TerrainWayPoints;
                    else
                        list = WaterWayPoints;

                    break;
                default:
                    Debug.LogError("there is no Return for this ai type" + aiType);
                    Return = true;
                    break;
            }

            if (Return)
                return Vector3.zero;

            int index = Random.Range(0, list.Count - 1);

            Vector3 vec = list[index];

            return vec;
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
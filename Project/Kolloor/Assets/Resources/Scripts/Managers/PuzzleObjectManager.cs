using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Managers
{
    public class PuzzleObjectManager : MonoBehaviour
    {
        public static PuzzleObjectManager instance;

        private Dictionary<int, PuzzleObject> puzzleObjects = new Dictionary<int, PuzzleObject>();
        private int ID = 0;

        private int firstID;

        void Awake()
        {
            instance = this;

        }

        public int Register(PuzzleObject puzzleObject)
        {
            ID++;
            firstID = ID;
            puzzleObjects.Add(ID, puzzleObject);
            return ID;
        }

        public void Remove(int ID)
        {
            puzzleObjects.Remove(ID);
        }

        public void Remove(PuzzleObject puzzleObject)
        {
            puzzleObjects.Remove(GetPuzzleObjectID(puzzleObject));
        }

        private PuzzleObject GetPuzzleObject(int ID)
        {
            return puzzleObjects[ID];
        }

        private int GetPuzzleObjectID(PuzzleObject puzzleObject)
        {
            return puzzleObjects.FirstOrDefault(p => p.Value == puzzleObject).Key;
        }

        public int ObjectsAmound()
        {
            return puzzleObjects.Count;
        }

        public PuzzleObject GetClosestObject(Vector3 position)
        {
            float ClosestDistance = Vector3.Distance(position, puzzleObjects[firstID].transform.position);
            PuzzleObject closestObj = puzzleObjects[firstID];
            foreach (PuzzleObject pObject in puzzleObjects.Values)
            {
                float vec = Vector3.Distance(position, pObject.transform.position);
                if (vec < ClosestDistance)
                {
                    ClosestDistance = vec;
                    closestObj = pObject;
                }
            }

            return closestObj;
        }
    }
}
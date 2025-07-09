using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Database
{
    public class LevelDatabase : MonoBehaviour
    {
        public static LevelDatabase Instance;
        [SerializeField] private List<LevelDataSo> levelList = new List<LevelDataSo>();
        private readonly Dictionary<int,LevelDataSo> levelDictionary = new Dictionary<int, LevelDataSo>();

        private void Awake()
        {
            Instance = this;
            foreach (LevelDataSo levelDataSo in levelList)
            {
                levelDictionary.Add(levelDataSo.level,levelDataSo);
            }
        }

        public LevelDataSo GetLevelDataSo(int level)
        {
            if (level > levelList.Count)
            {
                level = levelList.Count - 1;
            }
            return  levelDictionary[level];
        }

        public int GetTotalLevelCount()
        {
            return levelList.Count;
        }
    }
}
using System.Collections.Generic;
using GoalRelated;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Level/Level Data")]
    public class LevelDataSo : ScriptableObject
    {
        public int totalMoveCount;
        public List<GoalConfig> goalConfigs;
        public GridDataSo gridDataSo;
        public string levelName;
        
        
        public void SetFrom(LevelDataSo dataSo)
        {
            gridDataSo = dataSo.gridDataSo;
            goalConfigs = dataSo.goalConfigs;
            totalMoveCount = dataSo.totalMoveCount;
            levelName = dataSo.levelName;
        }
    }
    
    
}
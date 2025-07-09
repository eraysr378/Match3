using System.Collections.Generic;
using GoalRelated;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Level/Level Data")]
    public class LevelDataSo : ScriptableObject
    {
        public int totalMoveCount;
        public List<GoalConfig> goalConfigs;
        public GridDataSo gridDataSo;
        public int level;
        
        
        public void SetFrom(LevelDataSo dataSo)
        {
            gridDataSo = dataSo.gridDataSo;
            goalConfigs = dataSo.goalConfigs;
            totalMoveCount = dataSo.totalMoveCount;
            level = dataSo.level;
        }
    }
    
    
}
using LevelSystem;
using Managers;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectionButton : MonoBehaviour
    {
        [FormerlySerializedAs("levelData")] [SerializeField] private ScriptableObjects.LevelDataSo levelDataSo;
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                EventManager.OnLevelSelected?.Invoke(levelDataSo);
            });
        }
        
        public void SetLevelData(ScriptableObjects.LevelDataSo dataSo)
        {
            levelDataSo = dataSo;
            GetComponentInChildren<TextMeshProUGUI>().text = dataSo.levelName;
            GetComponent<Button>().onClick.AddListener(() =>
            {
                EventManager.OnLevelSelected?.Invoke(levelDataSo);
            });
        }
    }
}

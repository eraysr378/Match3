using LevelSystem;
using ScriptableObjects;
using UnityEngine;

namespace UI
{
    public class LevelSelectionPanelUI : MonoBehaviour
    {
        [SerializeField] private ScriptableObjects.LevelDataSo[] allLevels;
        [SerializeField] private GameObject levelButtonPrefab;
        [SerializeField] private Transform buttonContainer;

        private void Start()
        {
            foreach (ScriptableObjects.LevelDataSo level in allLevels)
            {
                GameObject buttonObj = Instantiate(levelButtonPrefab, buttonContainer);
                LevelSelectionButton selectionButton = buttonObj.GetComponent<LevelSelectionButton>();
                selectionButton.SetLevelData(level); // You can create this method
            }
        }
    }
}
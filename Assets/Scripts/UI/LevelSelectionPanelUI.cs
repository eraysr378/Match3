using Database;
using LevelSystem;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectionPanelUI : MonoBehaviour
    {
        [SerializeField] private GameObject levelButtonPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private int maxLevelCompleted;

        public void Init()
        {
            if (!PlayerPrefs.HasKey("MaxLevel"))
            {
                PlayerPrefs.SetInt("MaxLevel", 1);
            }
            maxLevelCompleted =  PlayerPrefs.GetInt("MaxLevel");
            int totalLevelCount = LevelDatabase.Instance.GetTotalLevelCount();
            for (int level = 1; level <= totalLevelCount; level++)
            {
                GameObject buttonObj = Instantiate(levelButtonPrefab, buttonContainer);
                LevelSelectionButton selectionButton = buttonObj.GetComponent<LevelSelectionButton>();
                selectionButton.SetLevel(level);
                if (level > maxLevelCompleted)
                {
                    selectionButton.GetComponent<Button>().interactable = false;
                }
            }
          
        }
    }
}
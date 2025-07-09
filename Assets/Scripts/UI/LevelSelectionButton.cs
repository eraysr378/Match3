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
        [SerializeField] private int level;
        private Button _button;
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() =>
            {
                EventManager.OnLevelSelected?.Invoke(level);
            });
        }
        
        public void SetLevel(int level)
        {
            this.level = level;

            GetComponentInChildren<TextMeshProUGUI>().text = $"Level {level}";
            _button.onClick.AddListener(() =>
            {
                EventManager.OnLevelSelected?.Invoke(level);
            });
        }
    }
}

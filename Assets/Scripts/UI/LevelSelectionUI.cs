using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectionUI : MonoBehaviour
    {
        [SerializeField] private LevelSelectionPanelUI levelSelectionPanelUI;
        [SerializeField] private Button backButton;

        private void Awake()
        {
            backButton.onClick.AddListener(Hide);
        }

        private void Start()
        {
            levelSelectionPanelUI.Init();
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
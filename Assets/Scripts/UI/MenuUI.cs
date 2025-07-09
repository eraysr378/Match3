using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private LevelSelectionUI levelSelectionUI;
        [SerializeField] private Button playNextButton;
        [SerializeField] private Button selectMenuButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button resetPlayerPrefs;

        private void Awake()
        {
            playNextButton.onClick.AddListener(() =>
            {
                EventManager.OnLevelSelected?.Invoke(PlayerPrefs.GetInt("MaxLevel"));
            });
            selectMenuButton.onClick.AddListener(() =>
            {
                levelSelectionUI.Show();
            });
            quitButton.onClick.AddListener(Application.Quit);
            resetPlayerPrefs.onClick.AddListener(PlayerPrefs.DeleteAll);
        }


    }
}
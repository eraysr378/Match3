
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LevelFailedScreenUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button tryAgainButton;
        private void Awake()
        {
            menuButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("MenuScene");
            });
            tryAgainButton.onClick.AddListener(() =>
            {
                EventManager.OnCurrentLevelSelected?.Invoke();
            });
        }
        private void Start()
        {
            Hide();
        }
        private void Show()
        {
            gameObject.SetActive(true);
        }
        public void Show(int score)
        {
            SetScore(score);
            Show();
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }
        private void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }
        
    }
}
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LevelCompletedScreenUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private List<GameObject> starList;

        private void Awake()
        {
            menuButton.onClick.AddListener(() => { SceneManager.LoadScene("MenuScene"); });
            nextButton.onClick.AddListener(() => { EventManager.OnNextLevelSelected?.Invoke(); });
            menuButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
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
            StartCoroutine(ActivateUIElements());
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }

        private IEnumerator ActivateUIElements()
        {
            yield return ActivateStars();
            yield return new WaitForSeconds(0.25f);
            menuButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }

        private IEnumerator ActivateStars()
        {
            foreach (var star in starList)
            {
                yield return new WaitForSeconds(0.3f);
                star.SetActive(true);
            }
        }
    }
}
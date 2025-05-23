using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class ScoreScreenUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Button menuButton;
        [SerializeField] private List<GameObject> starList;

        private void Awake()
        {
            menuButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(0);
            });
            menuButton.gameObject.SetActive(false);
        }
        private void Start()
        {
            Hide();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Show(int score,bool isCompleted)
        {
            scoreText.text = score.ToString();
            titleText.text = isCompleted ? "Level Completed" : "Level Failed";
            Show();
            StartCoroutine(ActivateUIElements(isCompleted));
            
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }
        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }

        

        private IEnumerator ActivateUIElements(bool isCompleted)
        {
            if (isCompleted)
            {
                yield return ActivateStars();
                yield return new WaitForSeconds(0.25f);
            }
            menuButton.gameObject.SetActive(true);
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

using Managers;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelSystem
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private LevelDataSo currentLevelDataSo;
        private void OnEnable()
        {
            EventManager.OnLevelSelected += LoadLevel;
        }
        private void OnDisable()
        {
            EventManager.OnLevelSelected -= LoadLevel;
        }

        private void LoadLevel(LevelDataSo levelDataSo)
        {
            currentLevelDataSo.SetFrom(levelDataSo);
            SceneManager.LoadScene("GameScene"); 
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Utils
{
    public class FirstMenuLoader : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            SceneManager.LoadScene("MenuScene");
        }
    }
}
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace DefaultNamespace
{
    public class SceneHelper : SingletonMonoBehaviour<SceneHelper>
    {

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void GoToStart() => SceneManager.LoadScene("Start");
        public void GoToGame() => SceneManager.LoadScene("Main");
        public void GoToDefeat() => SceneManager.LoadScene("Defeat");
        public void GoToVictory() => SceneManager.LoadScene("Victory");

    }
}
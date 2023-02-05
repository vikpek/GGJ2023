using System;
using System.Collections;
using UnityEngine;
namespace DefaultNamespace
{
    public class JumpToGame : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(ContinueToGame());
        }
        private IEnumerator ContinueToGame()
        {
            yield return new WaitForSeconds(2);
            SceneHelper.Instance.GoToGame();
        }
    }
}
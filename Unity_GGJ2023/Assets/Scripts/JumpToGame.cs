using System;
using System.Collections;
using UnityEngine;
namespace DefaultNamespace
{
    public class JumpToGame : MonoBehaviour
    {       
        [SerializeField] TMPro.TMP_Text text;
        public void GoToGame()
        {
            text.text = "Loading...";
;            SceneHelper.Instance.GoToGame();
        }
    }
}
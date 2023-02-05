using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class JumpToGame : MonoBehaviour
    {       
        [SerializeField] TMPro.TMP_Text text;
        [SerializeField] PlayerInput playerInput;
        public void GoToGame()
        {
            text.text = "Loading...";
            playerInput.DeactivateInput();
            playerInput.gameObject.SetActive(false);
            Destroy(playerInput.gameObject);
;            SceneHelper.Instance.GoToGame();
        }
    }
}
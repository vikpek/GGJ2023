using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class JumpToGame : MonoBehaviour
    {       
        [SerializeField] PlayerInput playerInput;

        public void GoToGame()
        {
            playerInput.DeactivateInput();
            playerInput.gameObject.SetActive(false);
            Destroy(playerInput.gameObject);
;            SceneHelper.Instance.GoToGame();
        }
    }
}
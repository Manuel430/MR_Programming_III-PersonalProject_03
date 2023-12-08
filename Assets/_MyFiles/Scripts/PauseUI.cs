using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace MR
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] GameObject answerUI;
        [SerializeField] GameObject pauseUI;
        [SerializeField] bool isPaused;

        [SerializeField] PlayerInputsScript playerInput;
        [SerializeField] MR_PlayerMovementScript player;

        public void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerInput = new PlayerInputsScript();
            playerInput.Player.Enable();
            playerInput.Player.Pause.performed += PausingGame;
        }

        public void Update()
        {
            if(answerUI.gameObject == true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState= CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public bool GetPaused()
        {
            return isPaused;
        }

        public void PausingGame(InputAction.CallbackContext context)
        {
            if (player.GetCutscene())
            {
                return;
            }

            if (!isPaused)
            {
                isPaused = true;
                Time.timeScale = 0f;
                pauseUI.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                isPaused = false;
                Time.timeScale = 1f;
                pauseUI.SetActive(false);

                Cursor.lockState= CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }



    }
}

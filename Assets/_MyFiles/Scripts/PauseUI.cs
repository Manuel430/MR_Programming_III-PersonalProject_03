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
            playerInput = new PlayerInputsScript();
            playerInput.Player.Enable();
            playerInput.Player.Pause.performed += PausingGame;
            Time.timeScale = 1.0f;
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
            }
            else
            {
                isPaused = false;
                Time.timeScale = 1f;
                pauseUI.SetActive(false);
            }
        }



    }
}

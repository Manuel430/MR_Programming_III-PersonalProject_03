using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

namespace MR
{
    public class MR_NPCDialogueScript : MonoBehaviour
    {
        [SerializeField] bool talkedTo;
        [SerializeField] bool inCutscene;
        bool finalActive;
        [SerializeField] GameObject interactionKey;
        [SerializeField] GameObject sentenceUI;
        [SerializeField] GameObject answerUI;

        [Header("ReputationPoints")]
        [SerializeField] float highPoints = 100;
        [SerializeField] float mediumPoints;
        [SerializeField] float lowPoints;
        [SerializeField] float reputation;

        [Header("OutsideScripts")]
        [SerializeField] MR_DialogueBehavior dialogueBehavior;
        [SerializeField] MR_ReputationScript reputationPoints;
        [SerializeField] PauseUI pause;

        [SerializeField] PlayerInputsScript playerInputs;
        [SerializeField] MR_PlayerMovementScript player;

        [Header("Dialogue Choices - New Chat")]
        [SerializeField] MR_DialogueNodeGraph lowReputation;
        [SerializeField] MR_DialogueNodeGraph mediumReputation;
        [SerializeField] MR_DialogueNodeGraph highReputation;

        [Header("Dialogue Choices - Talked To")]
        [SerializeField] MR_DialogueNodeGraph lowTalkedTo;
        [SerializeField] MR_DialogueNodeGraph mediumTalkedTo;
        [SerializeField] MR_DialogueNodeGraph highTalkedTo;

        private void Awake()
        {
            talkedTo = false;
            interactionKey.SetActive(false);

            playerInputs = new PlayerInputsScript();
            playerInputs.Player.Enable();
        }

        public void Update()
        {
            reputation = reputationPoints.GetReputationPoints();
        }

        public bool SetCutsceneAndInteraction(bool setActive)
        {
            player.SetCutscene(!setActive);
            SetCutscene(!setActive);
            
            interactionKey.SetActive(setActive);

            finalActive = setActive;

            return finalActive;
        }

        public bool GetCutscene()
        {
            return inCutscene;
        }

        public bool SetCutscene(bool setActive)
        {
            return inCutscene = setActive;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            interactionKey.SetActive(true);
            playerInputs.Player.Interact.performed += Interact;
        }

        public void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            interactionKey.SetActive(false);
            playerInputs.Player.Interact.performed -= Interact;
            inCutscene = false;
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if(pause.GetPaused())
            {
                return;
            }

            if (interactionKey.gameObject == true && inCutscene == false)
            {

                inCutscene = true;
                interactionKey.SetActive(false);
                player.SetCutscene(true);
                DialougeSelection();
            }
        }

        private void DialougeSelection()
        {
            if (reputation >= highPoints)
            {
                if (!talkedTo)
                {
                    dialogueBehavior.StartDialogue(highReputation);
                    talkedTo = true;
                }
                else
                {
                    dialogueBehavior.StartDialogue(highTalkedTo);
                }
            }
            else if (reputation >= mediumPoints && reputation < highPoints)
            {
                if (!talkedTo)
                {
                    dialogueBehavior.StartDialogue(mediumReputation);
                    talkedTo = true;
                }
                else
                {
                    dialogueBehavior.StartDialogue(mediumTalkedTo);
                }
            }
            else if (reputation >= lowPoints && reputation < mediumPoints || reputation < lowPoints)
            {
                if (!talkedTo)
                {
                    dialogueBehavior.StartDialogue(lowReputation);
                    talkedTo = true;
                }
                else
                {
                    dialogueBehavior.StartDialogue(lowTalkedTo);
                }
            }
        }

    }
}

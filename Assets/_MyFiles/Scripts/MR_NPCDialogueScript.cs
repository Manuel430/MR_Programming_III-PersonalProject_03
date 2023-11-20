using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace MR
{
    public class MR_NPCDialogueScript : MonoBehaviour
    {
        [SerializeField] bool talkedTo;
        [SerializeField] GameObject interactionKey;

        [Header("ReputationPoints")]
        [SerializeField] float highPoints = 100;
        [SerializeField] float mediumPoints;
        [SerializeField] float lowPoints;

        [Header("OutsideScripts")]
        [SerializeField] MR_DialogueBehavior dialogueBehavior;
        [SerializeField] MR_ReputationScript reputationPoints;
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

            playerInputs = new PlayerInputsScript();
            playerInputs.Player.Enable();
            playerInputs.Player.Interact.performed += Interact;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            interactionKey.SetActive(true);
        }

        public void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            interactionKey.SetActive(false);
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (interactionKey.gameObject == true)
            {
                Debug.Log("Test");
/*                StartDialogue();
                interactionKey.SetActive(false);
                player.SetCutscene(true);*/
            }
        }

        private void StartDialogue()
        {
            if (reputationPoints.GetReputationPoints() > mediumPoints || reputationPoints.GetReputationPoints() <= highPoints)
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
            else if (reputationPoints.GetReputationPoints() > lowPoints || reputationPoints.GetReputationPoints() <= mediumPoints)
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
            else if (reputationPoints.GetReputationPoints() <= lowPoints)
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

            player.SetCutscene(false);
        }

    }
}

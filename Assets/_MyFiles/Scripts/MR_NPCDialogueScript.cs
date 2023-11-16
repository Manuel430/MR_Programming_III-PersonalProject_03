using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

namespace MR
{
    public class MR_NPCDialogueScript : MonoBehaviour
    {
        [SerializeField] int requiredPoints;
        [SerializeField] bool talkedTo;
        [SerializeField] GameObject interactionKey;

        [Header("OutsideScripts")]
        [SerializeField] MR_DialogueBehavior dialogueBehavior;
        [SerializeField] MR_ReputationScript reputationPoints;
        [SerializeField] PlayerInputsScript playerInputsScript;

        [Header("Dialogue Choices")]
        [SerializeField] MR_DialogueNodeGraph worstReputation;
        [SerializeField] MR_DialogueNodeGraph lowReputation;
        [SerializeField] MR_DialogueNodeGraph mediumReputation;
        [SerializeField] MR_DialogueNodeGraph highReputation;
        [SerializeField] MR_DialogueNodeGraph lowTalkedTo;
        [SerializeField] MR_DialogueNodeGraph mediumTalkedTo;
        [SerializeField] MR_DialogueNodeGraph highTalkedTo;

        private void Awake()
        {
            talkedTo = false;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            if(reputationPoints.GetReputationPoints() >= requiredPoints)
            {

            }
            else if (reputationPoints.GetReputationPoints() <= requiredPoints)
            {

            }
            else
            {

            }
        }

    }
}

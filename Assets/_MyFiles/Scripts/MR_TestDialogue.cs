using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
    public class MR_TestDialogue : MonoBehaviour
    {
        [SerializeField] private MR_DialogueBehavior dialogueBehaviour;
        [SerializeField] private MR_DialogueNodeGraph dialogueGraph;

        private void OnEnable()
        {

            dialogueBehaviour.StartDialogue(dialogueGraph);
        }
    }
}

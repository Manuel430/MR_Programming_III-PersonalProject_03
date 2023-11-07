using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MR
{
    public class MR_DialogueBehavior : MonoBehaviour
    {
        [SerializeField] private float dialogueCharDelay;
        [SerializeField] private KeyCode nextSentenceKeyCode;

        [SerializeField] private UnityEvent onDialogueStart;
        [SerializeField] private UnityEvent onDialogueFinished;

        private MR_DialogueNodeGraph currentNodeGraph;
        private MR_Node currentNode;

        [SerializeField] static event Action OnSentenceNodeActive;
        [SerializeField] static event Action OnDialogSentenceEnd;
        [SerializeField] static event Action<string, Sprite> OnSentenceNodeActiveWithParameter;
        [SerializeField] static event Action OnAnswerNodeActive;
        [SerializeField] static event Action<int, MR_AnswerNode> OnAnswerButtonSetUp;
        [SerializeField] static event Action<int> OnAnswerNodeActiveWithParameter;
        [SerializeField] static event Action<int, string> OnAnswerNodeSetUp;
        [SerializeField] static event Action<char> OnDialogTextCharWrote;

        public void StartDialogue(MR_DialogueNodeGraph dialogueNodeGraph)
        {
            if(dialogueNodeGraph.nodesList == null)
            {
                Debug.LogWarning("Dialog Graph's node list is empty");
                return;
            }
        }
    }
}

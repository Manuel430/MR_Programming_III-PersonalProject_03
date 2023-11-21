using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        [SerializeField] MR_NPCDialogueScript npcDialogue;

        public static event Action OnSentenceNodeActive;
        public static event Action OnDialogueSentenceEnd;
        public static event Action<string, Sprite> OnSentenceNodeActiveWithParameter;
        public static event Action OnAnswerNodeActive;
        public static event Action<int, MR_AnswerNode> OnAnswerButtonSetUp;
        public static event Action<int> OnAnswerNodeActiveWithParameter;
        public static event Action<int, string> OnAnswerNodeSetUp;
        public static event Action<char> OnDialogueTextCharWrote;
        public void StartDialogue(MR_DialogueNodeGraph dialogueNodeGraph)
        {
            if(dialogueNodeGraph.nodesList == null)
            {
                Debug.LogWarning("Dialog Graph's node list is empty");
                return;
            }

            onDialogueStart?.Invoke();

            currentNodeGraph = dialogueNodeGraph;
            currentNode = currentNodeGraph.nodesList[0];

            HandleDialogGraphCurrentNode(currentNode);
        }

        private void HandleDialogGraphCurrentNode(MR_Node currentNode)
        {
            StopAllCoroutines();

            if (currentNode.GetType() == typeof(MR_SentenceNode))
            {
                MR_SentenceNode sentenceNode = (MR_SentenceNode)currentNode;

                OnSentenceNodeActive?.Invoke();
                OnSentenceNodeActiveWithParameter?.Invoke(sentenceNode.GetSentenceCharacterName(), sentenceNode.GetCharacterSprite());

                WriteDialogueText(sentenceNode.GetSentenceText());
            }
            else if(currentNode.GetType() == typeof(MR_AnswerNode))
            {
                MR_AnswerNode answerNode = (MR_AnswerNode)currentNode;
                int amountOfActiveButtons = 0;

                OnAnswerNodeActive?.Invoke();

                for(int i = 0; i < answerNode.childSentenceNodes.Length; i++)
                {
                    if (answerNode.childSentenceNodes[i] != null)
                    {
                        OnAnswerNodeSetUp?.Invoke(i, answerNode.answers[i]);
                        OnAnswerButtonSetUp?.Invoke(i, answerNode);

                        amountOfActiveButtons++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (amountOfActiveButtons == 0)
                {
                    onDialogueFinished?.Invoke();
                    return;
                }

                OnAnswerNodeActiveWithParameter?.Invoke(amountOfActiveButtons);
            }
        }

        public void SetCurrentNodeAndHandleDialogGraph(MR_Node node)
        {
            currentNode = node;
            HandleDialogGraphCurrentNode(this.currentNode);
        }

        private void WriteDialogueText(string text)
        {
            StartCoroutine(WriteDialogueTextRoutine(text));
        }

        private IEnumerator WriteDialogueTextRoutine(string text)
        {
            foreach (char textChar in text)
            {
                yield return new WaitForSeconds(dialogueCharDelay);
                OnDialogueTextCharWrote?.Invoke(textChar);
            }

            yield return new WaitUntil(() => Input.GetKeyDown(nextSentenceKeyCode));

            OnDialogueSentenceEnd?.Invoke();
            CheckForDialogueNextNode();
        }

        private void CheckForDialogueNextNode()
        {
            if(currentNode.GetType() == typeof(MR_SentenceNode))
            {
                MR_SentenceNode sentenceNode = (MR_SentenceNode)currentNode;

                if(sentenceNode.childNode != null)
                {
                    currentNode = sentenceNode.childNode;
                    HandleDialogGraphCurrentNode(currentNode);
                    npcDialogue.SetCutsceneAndInteraction(false);
                }
                else
                {
                    onDialogueFinished?.Invoke();
                    npcDialogue.SetCutsceneAndInteraction(true);
                }
            }
        }
        
        public void AddListenerToOnDialogueFinished(UnityAction action)
        {
            onDialogueFinished.AddListener(action);
        }
    }
}

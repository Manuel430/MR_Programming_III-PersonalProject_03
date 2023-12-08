
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MR
{
    public class MR_DialogueDisplayer : MonoBehaviour
    {
        [SerializeField] private MR_SentencePanel dialogueSentencePanel;
        [SerializeField] private MR_AnswerPanel dialogueAnswerPanel;
        [SerializeField] private MR_DialogueBehavior dialogueBehavior;



        private void OnEnable()
        {
            dialogueBehavior.AddListenerToOnDialogueFinished(DisableDialoguePanel);

            MR_DialogueBehavior.OnAnswerButtonSetUp += SetUpAnswerButtonsClickEvent;
            MR_DialogueBehavior.OnDialogueSentenceEnd += dialogueSentencePanel.ResetDialogueText;
            MR_DialogueBehavior.OnDialogueTextCharWrote += dialogueSentencePanel.AddCharToDialogueText;
            MR_DialogueBehavior.OnSentenceNodeActive += EnableDialogueSentencePanel;
            MR_DialogueBehavior.OnSentenceNodeActive += DisableDialogueAnswerPanel;
            MR_DialogueBehavior.OnSentenceNodeActiveWithParameter += dialogueSentencePanel.AssignDialogueNameTextAndSprite;
            MR_DialogueBehavior.OnAnswerNodeActive += EnableDialogueAnswerPanel;
            MR_DialogueBehavior.OnAnswerNodeActive += DisableDialogueSentencePanel;
            MR_DialogueBehavior.OnAnswerNodeActiveWithParameter += dialogueAnswerPanel.EnableCertainAmountOfButtons;
            MR_DialogueBehavior.OnAnswerNodeSetUp += SetUpAnswerDialoguePanel;
        }

        private void OnDisable()
        {
            MR_DialogueBehavior.OnAnswerButtonSetUp -= SetUpAnswerButtonsClickEvent;
            MR_DialogueBehavior.OnDialogueSentenceEnd -= dialogueSentencePanel.ResetDialogueText;
            MR_DialogueBehavior.OnDialogueTextCharWrote -= dialogueSentencePanel.AddCharToDialogueText;
            MR_DialogueBehavior.OnSentenceNodeActive -= EnableDialogueSentencePanel;
            MR_DialogueBehavior.OnSentenceNodeActive -= DisableDialogueAnswerPanel;
            MR_DialogueBehavior.OnSentenceNodeActiveWithParameter -= dialogueSentencePanel.AssignDialogueNameTextAndSprite;
            MR_DialogueBehavior.OnAnswerNodeActive -= EnableDialogueAnswerPanel;
            MR_DialogueBehavior.OnAnswerNodeActive -= DisableDialogueSentencePanel;
            MR_DialogueBehavior.OnAnswerNodeActiveWithParameter -= dialogueAnswerPanel.EnableCertainAmountOfButtons;
            MR_DialogueBehavior.OnAnswerNodeSetUp -= SetUpAnswerDialoguePanel;
        }



        public void DisableDialoguePanel()
        {
            DisableDialogueAnswerPanel();
            DisableDialogueSentencePanel();
        }


        public void EnableDialogueAnswerPanel()
        {
            ActiveGameObject(dialogueAnswerPanel.gameObject, true);
            dialogueAnswerPanel.DisableAllButtons();
        }


        public void DisableDialogueAnswerPanel()
        {
            ActiveGameObject(dialogueAnswerPanel.gameObject, false);
        }

        public void EnableDialogueSentencePanel()
        {
            ActiveGameObject(dialogueSentencePanel.gameObject, true);
        }

        public void DisableDialogueSentencePanel()
        {
            ActiveGameObject(dialogueSentencePanel.gameObject, false);
        }

        public void ActiveGameObject(GameObject gameObject, bool isActive)
        {
            if(gameObject == null)
            {
                Debug.LogWarning("Game object is null");
                return;
            }

            gameObject.SetActive(isActive);
        }

        public void SetUpAnswerButtonsClickEvent(int index, MR_AnswerNode answerNode)
        {
            dialogueAnswerPanel.GetButtonByIndex(index).onClick.AddListener(() =>
            {
                dialogueBehavior.SetCurrentNodeAndHandleDialogGraph(answerNode.childSentenceNodes[index]);
            }
            );
        }

        public void SetUpAnswerDialoguePanel(int index, string answerText)
        {
            dialogueAnswerPanel.GetButtonTextByIndex(index).text = answerText;
        }
    }

}
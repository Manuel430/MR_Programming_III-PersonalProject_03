using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MR
{
    public class MR_SentencePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI dialogueNameText;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Image dialogueCharacterImage;

        private void Awake()
        {
            dialogueText.text = string.Empty;
        }

        public void ResetDialogueText()
        {
            dialogueText.text = string.Empty;
        }

        public void AssignDialogueNameTextAndSprite(string name, Sprite sprite)
        {
            dialogueText.text = name;

            if(sprite == null)
            {
                dialogueCharacterImage.color = new Color(dialogueCharacterImage.color.r, dialogueCharacterImage.color.g,
                    dialogueCharacterImage.color.b, 0);
                return;
            }

            dialogueCharacterImage.color = new Color(dialogueCharacterImage.color.r, dialogueCharacterImage.color.g,
                dialogueCharacterImage.color.b, 255);
            dialogueCharacterImage.sprite = sprite;
        }

        public void AddCharToDialogueText(char textChar)
        {
            dialogueText.text += textChar;
        }
    }
}

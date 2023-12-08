using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MR
{
    public class MR_AnswerPanel : MonoBehaviour
    {
        [SerializeField] private List<Button> buttons = new List<Button>();
        [SerializeField] private List<TextMeshProUGUI> buttonTexts;

        public Button GetButtonByIndex(int index)
        {
            return buttons[index];
        }

        public TextMeshProUGUI GetButtonTextByIndex(int index)
        {
            return buttonTexts[index];
        }

        public void AddButtonOnClickListener(int index, UnityAction action)
        {
            buttons[index].onClick.AddListener(action);
        }

        public void EnableCertainAmountOfButtons(int amount)
        {
            if(buttons.Count == 0)
            {
                Debug.LogWarning("Please assign button list!");
                return;
            }

            for(int i = 0; i < amount; i++)
            {
                buttons[i].gameObject.SetActive(true);
            }
        }

        public void DisableAllButtons()
        {
            foreach (Button button in buttons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}

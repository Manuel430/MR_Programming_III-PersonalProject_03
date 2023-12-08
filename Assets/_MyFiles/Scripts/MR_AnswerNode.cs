
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MR
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Nodes/Answer Node", fileName = "New Answer Node")]
    public class MR_AnswerNode : MR_Node
    {
        private const int amountOfAnswers = 4;

        public List<string> answers = new List<string>();
        
        public MR_SentenceNode parentSentenceNode;
        public MR_SentenceNode[] childSentenceNodes;

        private const float lableFieldSpace = 15f;
        private const float textFieldWidth = 120f;

        private const float answerNodeWidth = 190f;
        private const float answerNodeHeight = 145f;

        public override void Initialize(Rect setRect, string nodeName, MR_DialogueNodeGraph setNodeGraph)
        {
            base.Initialize(setRect, nodeName, setNodeGraph);
            
            childSentenceNodes = new MR_SentenceNode[amountOfAnswers];

            for (int i = 0; i < amountOfAnswers; i++)
            {
                answers.Add(string.Empty);
            }
        }

#if UNITY_EDITOR
        public override void Draw(GUIStyle nodeStyle, GUIStyle lableStyle)
        {
            base.Draw(nodeStyle, lableStyle);

            rect.size = new Vector2(answerNodeWidth, answerNodeHeight);

            GUILayout.BeginArea(rect, nodeStyle);
            EditorGUILayout.LabelField("Answer Node", lableStyle);

            DrawAnswerLine(1, MR_EditorIcons.GreenDot);
            DrawAnswerLine(2, MR_EditorIcons.GreenDot);
            DrawAnswerLine(3, MR_EditorIcons.GreenDot);
            DrawAnswerLine(4, MR_EditorIcons.GreenDot);

            GUILayout.EndArea();
        }
//#endif

        private void DrawAnswerLine(int answerNumber, string iconPathOrName)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{answerNumber}.", GUILayout.Width(lableFieldSpace));
            answers[answerNumber - 1] = EditorGUILayout.TextField(answers[answerNumber - 1], GUILayout.Width(textFieldWidth));
            EditorGUILayout.LabelField(EditorGUIUtility.IconContent(iconPathOrName), GUILayout.Width(lableFieldSpace));
            EditorGUILayout.EndHorizontal();
        }
//#endif

        public override bool AddToParentConnectedNode(MR_Node nodeToAdd)
        {
            if(nodeToAdd.GetType() == typeof(MR_SentenceNode))
            {
                 parentSentenceNode = (MR_SentenceNode)nodeToAdd;

                return true;
            }

            return false;
        }
//#endif

        public override bool AddToChildConnectedNode(MR_Node nodeToAdd)
        {
            MR_SentenceNode sentenceNodeToAdd;
            
            if(nodeToAdd.GetType() != typeof(MR_AnswerNode))
            {
                sentenceNodeToAdd = (MR_SentenceNode)nodeToAdd;
            }
            else
            {
                return false;
            }

            for(int i = 0; i < amountOfAnswers; i++)
            {
                if (childSentenceNodes[i] == null && sentenceNodeToAdd.parentNode == null)
                {
                    childSentenceNodes[i] = (MR_SentenceNode)nodeToAdd;

                    return true;
                }
            }

            return false;
        }
#endif
    }
}
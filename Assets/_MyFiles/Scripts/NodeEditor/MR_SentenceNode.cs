using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MR
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Nodes/Sentence Node", fileName = "New Sentence Node")]
    public class MR_SentenceNode : MR_Node
    {
        [SerializeField] private MR_Sentence sentence;

        [Space(10)]
        public MR_Node parentNode;

        public MR_Node childNode;

        private const float lableFieldSpace = 40f;
        private const float textFieldWidth = 90f;

        public string GetSentenceCharacterName()
        {
            return sentence.characterName;
        }

        public string GetSentenceText()
        {
            return sentence.text;
        }

        public Sprite GetCharacterSprite()
        {
            return sentence.characterSprite;
        }

        public override void Draw(GUIStyle nodeStyle, GUIStyle lableStyle)
        {
            base.Draw(nodeStyle, lableStyle);

            GUILayout.BeginArea(rect, nodeStyle);

            EditorGUILayout.LabelField("Sentence Node", lableStyle);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Name ", GUILayout.Width(lableFieldSpace));
            sentence.characterName = EditorGUILayout.TextField(sentence.characterName, GUILayout.Width(textFieldWidth));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Text ", GUILayout.Width(lableFieldSpace));
            sentence.text = EditorGUILayout.TextField(sentence.text, GUILayout.Width(textFieldWidth));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Sprite ", GUILayout.Width(lableFieldSpace));
            sentence.characterSprite = (Sprite)EditorGUILayout.ObjectField(sentence.characterSprite,
                typeof(Sprite), false, GUILayout.Width(textFieldWidth));
            EditorGUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        public override bool AddToChildConnectedNode(MR_Node nodeToAdd)
        {
            MR_SentenceNode sentenceNodeToAdd;

            if(nodeToAdd.GetType() == typeof(MR_SentenceNode))
            {
                nodeToAdd = (MR_SentenceNode)nodeToAdd;

                if(nodeToAdd == this)
                {
                    return false;
                }
            }

            if(nodeToAdd.GetType() == typeof(MR_SentenceNode))
            {
                sentenceNodeToAdd = (MR_SentenceNode)nodeToAdd;

                if(sentenceNodeToAdd != null && sentenceNodeToAdd.childNode == this)
                {
                    return false;
                }
            }

            childNode = nodeToAdd;
            return true;
        }

        public override bool AddToParentConnectedNode(MR_Node nodeToAdd)
        {
            MR_SentenceNode sentenceNodeToAdd;

            if(nodeToAdd.GetType() == typeof(MR_SentenceNode))
            {
                nodeToAdd = (MR_SentenceNode)nodeToAdd;

                if(nodeToAdd == this)
                {
                    return false;
                }
            }

            parentNode = nodeToAdd;

            if(nodeToAdd.GetType() == typeof(MR_SentenceNode))
            {
                sentenceNodeToAdd = (MR_SentenceNode)nodeToAdd;

                if(sentenceNodeToAdd.childNode == this)
                {
                    return false;
                }
                else
                {
                    parentNode = null;
                }
            }

            return true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MR
{
    public class MR_Node : MonoBehaviour
    {
        [SerializeField][HideInInspector] List<MR_Node> connectedNodesList;
        [SerializeField][HideInInspector] Rect rect = new Rect();
        [SerializeField][HideInInspector] MR_DialogueNodeGraph nodeGraph;

        [HideInInspector] public bool isDragging;
        [HideInInspector] public bool isSelected;

        public virtual void Initialize(Rect setRect, string nodeName, MR_DialogueNodeGraph setNodeGraph)
        {
            name = nodeName;
            rect = setRect;
            nodeGraph = setNodeGraph;
        }

        public virtual void Draw(GUIStyle nodeStyle, GUIStyle lableStyle) { }

        public virtual bool AddToParentConnectedNode(MR_Node nodeToAdd) { return true; }
        public virtual bool AddToChildConnectedNode(MR_Node nodeToAdd) { return true; }

        public  void ProcessNodeEvents(Event currentEvent)
        {

        }

        public void DragNode(Vector2 delta)
        {
            rect.position += delta;
            EditorUtility.SetDirty(this);
        }
    }
}

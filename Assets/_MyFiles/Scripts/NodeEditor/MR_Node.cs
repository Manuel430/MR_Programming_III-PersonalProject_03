using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MR
{
    public class MR_Node : ScriptableObject
    {
        [HideInInspector] public List<MR_Node> connectedNodesList;
        [HideInInspector] public MR_DialogueNodeGraph nodeGraph;

        [HideInInspector] public Rect rect = new Rect();
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
            switch(currentEvent.type)
            {
                case EventType.MouseDown:
                    ProcessMouseDownEvent(currentEvent);
                    break;
                case EventType.MouseUp:
                    ProcessMouseUpEvent(currentEvent);
                    break;
                case EventType.MouseDrag:
                    ProcessMouseDragEvent(currentEvent);
                    break;
                default:
                    break;
            }
        }

        private void ProcessMouseDownEvent(Event currentEvent)
        {
            if(currentEvent.button == 0)
            {
                ProcessLeftMouseDownEvent(currentEvent);
            }
            else if(currentEvent.button == 1)
            {
                ProcessRightMouseDownEvent(currentEvent);
            }
        }

        private void ProcessLeftMouseDownEvent(Event currentEvent)
        {
            OnNodeLeftClick();
        }

        private void ProcessRightMouseDownEvent(Event currentEvent)
        {
            nodeGraph.SetNodeToDrawLineFromAndLinePosition(this, currentEvent.mousePosition);
        }

        private void ProcessMouseUpEvent(Event currentEvent)
        {
            if(currentEvent.button == 0)
            {
                ProcessLeftMouseUpEvent(currentEvent);
            }
        }

        private void ProcessLeftMouseUpEvent(Event currentEvent)
        {
            isDragging = false;
        }

        private void ProcessMouseDragEvent(Event currentEvent)
        {
            if(currentEvent.button == 0)
            {
                ProcessLeftMouseDragEvent(currentEvent);
            }
        }

        private void ProcessLeftMouseDragEvent(Event currentEvent)
        {
            isDragging = true;
            DragNode(currentEvent.delta);
            GUI.changed = true;
        }

        public void OnNodeLeftClick()
        {
            Selection.activeObject = this;
            if(isSelected)
            {
                isSelected = false;
            }
            else
            {
                isSelected = true;
            }
        }

        public void DragNode(Vector2 delta)
        {
            rect.position += delta;
            EditorUtility.SetDirty(this);
        }
    }
}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MR
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Nodes/Node Graph", fileName = "New Node Graph")]
    public class MR_DialogueNodeGraph : ScriptableObject
    {
        public List <MR_Node> nodesList = new List<MR_Node>();

        [HideInInspector] public MR_Node nodeToDrawLineFrom = null;
        [HideInInspector] public Vector2 linePosition = Vector2.zero;

#if UNITY_EDITOR
        public void  SetNodeToDrawLineFromAndLinePosition(MR_Node setNodeDraw, Vector2 setLinePosition)
        {
            nodeToDrawLineFrom = setNodeDraw;
            linePosition = setLinePosition;
        }

        public void DragAllSelectedNodes(Vector2 delta)
        {
            foreach (var node in nodesList)
            {
                if (node.isSelected)
                {
                    node.DragNode(delta);
                }
            }
        }

        public int GetAmountOfSelectedNodes()
        {
            int amount = 0;

            foreach (MR_Node node in nodesList)
            {
                if (node.isSelected)
                {
                    amount++;
                }
            }

            return amount;
        }
#endif
    }
}
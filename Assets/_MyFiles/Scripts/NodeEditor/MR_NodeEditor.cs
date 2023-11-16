using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;

namespace MR
{
    public class MR_NodeEditor : EditorWindow
    {
        private static MR_DialogueNodeGraph currentNodeGraph;
        private MR_Node currentNode;

        private GUIStyle nodeStyle;
        private GUIStyle selectedNodeStyle;
        private GUIStyle lableStyle;

        private Rect selectionRect;

        private Vector2 mouseScrollClickPosition;
        private Vector2 graphOffset;
        private Vector2 graphDrag;

        private const float nodeWidth = 170f;
        private const float nodeHeight = 115f;
        private const float connectingLineWidth = 2f;
        private const float connectingLineArrowSize = 4f;

        private const int lableFontSize = 12;
        private const int nodePadding = 20;
        private const int nodeBorder = 10;

        private const float gridLargeLineSpacing = 100f;
        private const float gridSmallLineSpacing = 25;

        private bool isScrollWheelDragging = false;

        private void OnEnable()
        {
            Selection.selectionChanged += ChangeEditorWindowOnSelection;

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load(MR_EditorIcons.Node) as Texture2D;
            nodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
            nodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = EditorGUIUtility.Load(MR_EditorIcons.SelectedNode) as Texture2D;
            selectedNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
            selectedNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

            lableStyle = new GUIStyle();
            lableStyle.alignment = TextAnchor.MiddleLeft;
            lableStyle.fontSize = lableFontSize;
            lableStyle.normal.textColor = Color.white;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= ChangeEditorWindowOnSelection;

            AssetDatabase.SaveAssets();
            SaveChanges();
        }

        [OnOpenAsset(0)]
        public static bool OnDoubleClickAsset(int instanceID, int line)
        {
            MR_DialogueNodeGraph nodeGraph = EditorUtility.InstanceIDToObject(instanceID) as MR_DialogueNodeGraph;

            if(nodeGraph != null )
            {
                OpenWindow();
                
                currentNodeGraph = nodeGraph;
                
                return true;
            }

            return false;
        }

        [MenuItem("Dialogue Node Based Editor", menuItem = "Window/Dialogue Node Base Editor")]
        private static void OpenWindow()
        {
            MR_NodeEditor window = (MR_NodeEditor)GetWindow(typeof(MR_NodeEditor));
            window.Show();
        }

        private void OnGUI()
        {
            if(currentNodeGraph != null )
            {
                DrawDraggedLine();
                DrawNodeConnection();
                DrawGridBackground(gridSmallLineSpacing, 0.2f, Color.gray);
                DrawGridBackground(gridLargeLineSpacing, 0.2f, Color.gray);
                ProcessEvents(Event.current);
                DrawNodes();
            }

            if(GUI.changed)
                Repaint();
        }

        private void DrawDraggedLine()
        {
            if(currentNodeGraph.linePosition != Vector2.zero)
            {
                Handles.DrawBezier(currentNodeGraph.nodeToDrawLineFrom.rect.center, currentNodeGraph.linePosition,
                    currentNodeGraph.nodeToDrawLineFrom.rect.center, currentNodeGraph.linePosition,
                    Color.white, null, connectingLineWidth);
            }
        }

        private void DrawNodeConnection()
        {
            if(currentNodeGraph.nodesList == null)
            {
                return;
            }

            foreach (MR_Node node in currentNodeGraph.nodesList)
            {
                MR_Node parentNode = null;
                MR_Node childNode = null;

                if (node.GetType() == typeof(MR_AnswerNode))
                {
                    MR_AnswerNode answerNode = (MR_AnswerNode)node;

                    for (int i = 0; i < answerNode.childSentenceNodes.Length; i++)
                    {
                        if (answerNode.childSentenceNodes[i] != null)
                        {
                            parentNode = node;
                            childNode = answerNode.childSentenceNodes[i];

                            DrawConnectionLine(parentNode, childNode);
                        }
                    }
                }
                else if (node.GetType() == typeof(MR_SentenceNode))
                {
                    MR_SentenceNode sentenceNode = (MR_SentenceNode)node;

                    if(sentenceNode.childNode != null)
                    {
                        parentNode = node;
                        childNode = sentenceNode.childNode;

                        DrawConnectionLine(parentNode, childNode);
                    }
                }
            }
        }

        private void DrawConnectionLine(MR_Node parentNode, MR_Node childNode)
        {
            Vector2 startPosition = parentNode.rect.center;
            Vector2 endPosition = childNode.rect.center;
            Vector2 midPosition = (startPosition + endPosition) / 2;
            Vector2 direction = endPosition - startPosition;
            Vector2 arrowTailPoint1 = midPosition - new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;
            Vector2 arrowTailPoint2 = midPosition + new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;
            Vector2 arrowHeadPoint = midPosition + direction.normalized * connectingLineArrowSize;

            Handles.DrawBezier(arrowHeadPoint, arrowTailPoint1, arrowHeadPoint, arrowTailPoint1,
                Color.white, null, connectingLineWidth);
            Handles.DrawBezier(arrowHeadPoint, arrowTailPoint2, arrowHeadPoint, arrowTailPoint2,
                Color.white, null, connectingLineWidth);
            Handles.DrawBezier(startPosition, endPosition, startPosition, endPosition,
                Color.white, null, connectingLineWidth);

            GUI.changed = true;
        }

        private void DrawGridBackground(float gridSize, float gridOpacity, Color color)
        {
            int verticalLineCount = Mathf.CeilToInt((position.width + gridSize) / gridSize);
            int horizontalLineCount = Mathf.CeilToInt((position.height + gridSize) / gridSize);

            Handles.color = new Color(color.r, color.g, color.b, gridOpacity);

            graphOffset += graphDrag * 0.5f;

            Vector3 gridOffset = new Vector3(graphOffset.x % gridSize, graphOffset.y % gridSize, 0);

            for (int i = 0; i < verticalLineCount; i++)
            {
                Handles.DrawLine(new Vector3(gridSize * i, -gridSize, 0) + gridOffset, new Vector3(gridSize * i, position.height + gridSize, 0f) + gridOffset);
            }

            for (int j = 0; j < horizontalLineCount; j++)
            {
                Handles.DrawLine(new Vector3(-gridSize, gridSize * j, 0) + gridOffset, new Vector3(position.width + gridSize, gridSize * j, 0f) + gridOffset);
            }

            Handles.color = Color.white;
        }

        private void DrawNodes()
        {
            if (currentNodeGraph.nodesList == null)
            {
                return;
            }

            foreach (MR_Node node in currentNodeGraph.nodesList)
            {
                if(!node.isSelected)
                {
                    node.Draw(nodeStyle, lableStyle);
                }
                else
                {
                    node.Draw(selectedNodeStyle, lableStyle);
                }
            }

            GUI.changed = true;
        }

        private void ProcessEvents(Event currentEvent)
        {
            graphDrag = Vector2.zero;

            if(currentNode == null || currentNode.isDragging == false)
            {
                currentNode = GetHighlightedNode(currentEvent.mousePosition);
            }
            if(currentNode == null || currentNodeGraph.nodeToDrawLineFrom != null)
            {
                ProcessNodeEditorEvents(currentEvent);
            }
            else
            {
                currentNode.ProcessNodeEvents(currentEvent);
            }
        }

        private void ProcessNodeEditorEvents(Event currentEvent)
        {
            switch (currentEvent.type)
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
                case EventType.Repaint:
                    SelectNodesBySelectionRect(currentEvent.mousePosition);
                    break;
                default:
                    break;
            }
        }

        private void ProcessMouseDownEvent(Event currentEvent)
        {
            if(currentEvent.button == 1)
            {
                ProcessRightMouseDownEvent(currentEvent);
            }
            else if (currentEvent.button == 0 && currentNodeGraph.nodesList != null)
            {
                ProcessLeftMouseDownEvent(currentEvent);
            }
            else if(currentEvent.button == 2)
            {
                ProcessScrollWheelDownEvent(currentEvent);
            }
        }

        private void ProcessRightMouseDownEvent(Event currentEvent)
        {
            if(GetHighlightedNode(currentEvent.mousePosition) == null)
            {
                ShowContextMenu(currentEvent.mousePosition);
            }
        }

        private void ProcessLeftMouseDownEvent(Event currentEvent)
        {
            ProcessNodeSelection(currentEvent.mousePosition);
        }

        private void ProcessScrollWheelDownEvent(Event currentEvent)
        {
            mouseScrollClickPosition = currentEvent.mousePosition;
            isScrollWheelDragging = true;
        }

        private void ProcessMouseUpEvent(Event currentEvent)
        {
            if (currentEvent.button == 1)
            {
                ProcessRightMouseUpEvent(currentEvent);
            }
            else if (currentEvent.button == 2)
            {
                ProcessScrollWheelUpEvent(currentEvent);
            }
        }

        private void ProcessRightMouseUpEvent(Event currentEvent)
        {
            if (currentNodeGraph.nodeToDrawLineFrom != null)
            {
                CheckLineConnection(currentEvent);
                ClearDraggedLine();
            }
        }

        private void ProcessScrollWheelUpEvent(Event currentEvent)
        {
            selectionRect = new Rect(0, 0, 0, 0);
            isScrollWheelDragging = false;
        }

        private void ProcessMouseDragEvent(Event currentEvent)
        {
            if (currentEvent.button == 0)
            {
                ProcessLeftMouseDragEvent(currentEvent);
            }
            else if (currentEvent.button == 1)
            {
                ProcessRightMouseDragEvent(currentEvent);
            }
        }

        private void ProcessLeftMouseDragEvent(Event currentEvent)
        {
            SelectNodesBySelectionRect(currentEvent.mousePosition);

            graphDrag = currentEvent.delta;

            foreach (var node in currentNodeGraph.nodesList)
            {
                node.DragNode(graphDrag);
            }

            GUI.changed = true;
        }

        private void ProcessRightMouseDragEvent(Event currentEvent)
        {
            if (currentNodeGraph.nodeToDrawLineFrom != null)
            {
                DragConnectingLine(currentEvent.delta);
                GUI.changed = true;
            }
        }

        private void DragConnectingLine(Vector2 delta)
        {
            currentNodeGraph.linePosition += delta;
        }

        private void CheckLineConnection(Event currentEvent)
        {
            if (currentNodeGraph.nodeToDrawLineFrom != null)
            {
                MR_Node node = GetHighlightedNode(currentEvent.mousePosition);

                if (node != null)
                {
                    currentNodeGraph.nodeToDrawLineFrom.AddToChildConnectedNode(node);
                    node.AddToParentConnectedNode(currentNodeGraph.nodeToDrawLineFrom);
                }
            }
        }

        private void ClearDraggedLine()
        {
            currentNodeGraph.nodeToDrawLineFrom = null;
            currentNodeGraph.linePosition = Vector2.zero;
            GUI.changed = true;
        }

        private void ProcessNodeSelection(Vector2 mouseClickPosition)
        {
            MR_Node clickedNode = GetHighlightedNode(mouseClickPosition);

            if (clickedNode == null)
            {
                foreach (MR_Node node in currentNodeGraph.nodesList)
                {
                    if (node.isSelected)
                    {
                        node.isSelected = false;
                    }
                }

                return;
            }
        }

        private void SelectNodesBySelectionRect(Vector2 mousePosition)
        {
            if (!isScrollWheelDragging)
            {
                return;
            }

            selectionRect = new Rect(mouseScrollClickPosition.x, mouseScrollClickPosition.y,
                mousePosition.x - mouseScrollClickPosition.x, mousePosition.y - mouseScrollClickPosition.y);

            EditorGUI.DrawRect(selectionRect, new Color(0, 0, 0, 0.5f));


            foreach (MR_Node node in currentNodeGraph.nodesList)
            {
                if (selectionRect.Contains(node.rect.position))
                {
                    node.isSelected = true;
                }
            }
        }

        private MR_Node GetHighlightedNode(Vector2 mousePosition)
        {
            if (currentNodeGraph.nodesList.Count == 0)
            {
                return null;
            }

            foreach (MR_Node node in currentNodeGraph.nodesList)
            {
                if (node.rect.Contains(mousePosition))
                {
                    return node;
                }
            }

            return null;
        }

        private void ShowContextMenu(Vector2 mousePosition)
        {
            GenericMenu contextMenu = new GenericMenu();

            contextMenu.AddItem(new GUIContent("Create Sentence Node"), false, CreateSentenceNode, mousePosition);
            contextMenu.AddItem(new GUIContent("Create Answer Node"), false, CreateAnswerNode, mousePosition);
            contextMenu.AddSeparator("");
            contextMenu.AddItem(new GUIContent("Select All Nodes"), false, SelectAllNodes, mousePosition);
            contextMenu.AddItem(new GUIContent("Remove Selected Nodes"), false, RemoveSelectedNodes, mousePosition);
            contextMenu.ShowAsContext();
        }

        private void CreateSentenceNode(object mousePositionObject)
        {
            MR_SentenceNode sentenceNode = ScriptableObject.CreateInstance<MR_SentenceNode>();
            InitializeNode(mousePositionObject, sentenceNode, "Sentence Node");
        }

        private void CreateAnswerNode(object mousePositionObject)
        {
            MR_AnswerNode answerNode = ScriptableObject.CreateInstance<MR_AnswerNode>();
            InitializeNode(mousePositionObject, answerNode, "Answer Node");
        }

        private void SelectAllNodes(object userData)
        {
            foreach (MR_Node node in currentNodeGraph.nodesList)
            {
                node.isSelected = true;
            }

            GUI.changed = true;
        }

        private void RemoveSelectedNodes(object userData)
        {
            Queue<MR_Node> nodeDeletionQueue = new Queue<MR_Node>();

            foreach (MR_Node node in currentNodeGraph.nodesList)
            {
                if (node.isSelected)
                {
                    nodeDeletionQueue.Enqueue(node);
                }
            }

            while (nodeDeletionQueue.Count > 0)
            {
                MR_Node nodeTodelete = nodeDeletionQueue.Dequeue();

                currentNodeGraph.nodesList.Remove(nodeTodelete);

                DestroyImmediate(nodeTodelete, true);
                AssetDatabase.SaveAssets();
            }
        }

        private void InitializeNode(object mousePositionObject, MR_Node node, string nodeName)
        {
            Vector2 mousePosition = (Vector2)mousePositionObject;

            currentNodeGraph.nodesList.Add(node);

            node.Initialize(new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)), nodeName, currentNodeGraph);

            AssetDatabase.AddObjectToAsset(node, currentNodeGraph);
            AssetDatabase.SaveAssets();
        }

        private void ChangeEditorWindowOnSelection()
        {
            MR_DialogueNodeGraph nodeGraph = Selection.activeObject as MR_DialogueNodeGraph;

            if (nodeGraph != null)
            {
                currentNodeGraph = nodeGraph;
                GUI.changed = true;
            }
        }
    }
}

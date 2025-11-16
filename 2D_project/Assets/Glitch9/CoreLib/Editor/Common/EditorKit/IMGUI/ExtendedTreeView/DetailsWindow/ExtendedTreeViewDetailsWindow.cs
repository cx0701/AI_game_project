using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorKit.IMGUI
{
    public abstract partial class ExtendedTreeViewWindow<TTreeViewWindow, TTreeView, TTreeViewItem, TTreeViewDetailsWindow, TTreeViewData, TTreeViewItemFilter, TTreeViewContextMenuHandler>
    {
        /// <summary>
        /// Base class for child windows of the TreeViewWindow
        /// </summary>
        public abstract class ExtendedTreeViewDetailsWindow : PaddedEditorWindow
        {
            private static class GUIContents
            {
                internal static readonly GUIContent kToolMenu = new(EditorIcons.Menu, "Open the tool menu");
            }

            protected const string kFallbackTitle = "Unknown Item";
            protected const float MIN_TEXT_FIELD_HEIGHT = 20;
            public TTreeViewItem Item { get; set; }
            public TTreeView TreeView { get; set; }
            public TTreeViewContextMenuHandler EventHandler { get; set; }

            public TTreeViewData Data => Item?.Data;
            public TTreeViewData UpdatedData { get; set; }
            public bool IsDirty => !Data.Equals(UpdatedData);
            public GUIContent Title { get; set; }

            private bool _isInitialized = false;
            private Vector2 _scrollPosition;

            public void SetData(TTreeViewItem item, TTreeView treeView, TTreeViewContextMenuHandler eventHandler)
            {
                Item = item;
                TreeView = treeView;
                EventHandler = eventHandler;
                Initialize();
            }

            protected virtual void Initialize()
            {
                if (_isInitialized) return;
                _isInitialized = true;
                Title = CreateTitle();
                UpdatedData = Data;
            }

            protected virtual GUIContent CreateTitle() => new(!string.IsNullOrEmpty(Data.Name) ? Data.Name : kFallbackTitle);
            protected abstract void DrawSubtitle();
            protected abstract void DrawBody();


            protected override void DrawGUI()
            {
                Initialize();

                if (Data == null)
                {
                    EditorGUILayout.LabelField("Data is null");
                    return;
                }

                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(Title, TreeViewStyles.DetailsWindowTitle, GUILayout.MaxWidth(position.width - 44));
                        DrawToolMenuButton();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5);

                    DrawSubtitle();

                    GUILayout.BeginVertical(TreeViewStyles.EditWindowBody);
                    {
                        DrawBody();
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndScrollView();
            }

            public void RevertChanges()
            {
                UpdatedData = Data;
            }

            private void RepaintWindow(IResult result)
            {
                if (result.IsSuccess)
                {
                    Repaint();
                    TreeView.UpdateData(Item.Data);
                }
            }

            private void DrawToolMenuButton()
            {
                if (GUILayout.Button(GUIContents.kToolMenu, ExEditorStyles.miniButton))
                {
                    if (EventHandler == null) return;
                    EventHandler.ShowDetailsWindowMenu(Item, RepaintWindow);
                }
            }
        }
    }
}
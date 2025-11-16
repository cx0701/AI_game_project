using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Glitch9.EditorKit.IMGUI
{
    public enum TreeViewContextMenuType
    {
        None,
        Add,
        Save,
        Remove,
        Revert,
        Copy,
        Paste,
        Custom
    }

    public abstract partial class ExtendedTreeViewWindow<TTreeViewWindow, TTreeView, TTreeViewItem, TTreeViewDetailsWindow, TTreeViewData, TTreeViewItemFilter, TTreeViewContextMenuHandler>
    {
        public abstract partial class TreeViewContextMenuHandler
        {
            public List<TreeViewContextMenu> ContextMenus { get; private set; }
            private bool _isSorted = false;
            protected readonly TTreeView _treeView;

            public TreeViewContextMenuHandler(TTreeView treeView)
            {
                _treeView = treeView;
                ContextMenus = CreateContextMenus().ToList();
            }

            public abstract IEnumerable<TreeViewContextMenu> CreateContextMenus();

            public void ShowRightClickMenu(IList<TTreeViewItem> items, Action<bool> refreshTreeView)
            {
                if (ContextMenus.IsNullOrEmpty()) return;
                SortEvents();

                GenericMenu menu = new();

                foreach (TreeViewContextMenu contextMenu in ContextMenus)
                {
                    Util.ShowRightClickMenu(ref menu, items, refreshTreeView, contextMenu);
                }

                menu.ShowAsContext();
            }

            public void ShowDetailsWindowMenu(TTreeViewItem item, Action<IResult> refreshWindow)
            {
                if (ContextMenus.IsNullOrEmpty()) return;
                SortEvents();

                GenericMenu menu = new();

                foreach (TreeViewContextMenu contextMenu in ContextMenus)
                {
                    Util.ShowDetailsWindowMenu(ref menu, item, refreshWindow, contextMenu);
                }

                menu.ShowAsContext();
            }

            private void SortEvents()
            {
                if (_isSorted) return;

                ContextMenus.Sort((a, b) => a.Index.CompareTo(b.Index));
                _isSorted = true;
            }

            public class TreeViewContextMenu : IListEntry
            {
                public int Index { get; set; }
                public bool IsEmpty => Action == null;
                public bool ShowInRightClickMenu { get; set; } = true;
                public bool ShowInDetailsWindowMenu { get; set; } = true;
                public bool ShowConfirmationMessage { get; set; } = false;
                public string Name { get; set; }
                public string ConfirmationMessage { get; set; }
                public TreeViewContextMenuType Type { get; set; } = TreeViewContextMenuType.None;
                public Action<TTreeViewItem[], Action<bool>> Action { get; set; }
                public Action<TTreeViewItem[], bool> Callback { get; set; }
                public Func<TTreeViewItem[], bool> Condition { get; set; }


                public TreeViewContextMenu() { }

                public TreeViewContextMenu(string name = null, string confirmationMessage = null)
                {
                    Type = TreeViewContextMenuType.Custom;
                    Name = name;
                    ConfirmationMessage = confirmationMessage;
                }

                public TreeViewContextMenu(TreeViewContextMenuType type, string name = null, string confirmationMessage = null)
                {
                    Type = type;
                    Name = name ?? Util.GetDefaultMenuName(type);
                    ConfirmationMessage = confirmationMessage ?? Util.GetDefaultConfirmationMessage(type);
                }

                public bool IsVisible(params TTreeViewItem[] items) => Condition == null || Condition(items);
                public void Execute(Action<bool> onSuccess, params TTreeViewItem[] items)
                {
                    if (ShowConfirmationMessage)
                    {
                        if (!ShowDialog.Confirm(ConfirmationMessage))
                        {
                            return;
                        }
                    }

                    Action?.Invoke(items, (success) =>
                    {
                        Callback?.Invoke(items, success);
                        onSuccess(success);
                    });
                }
            }
        }
    }
}
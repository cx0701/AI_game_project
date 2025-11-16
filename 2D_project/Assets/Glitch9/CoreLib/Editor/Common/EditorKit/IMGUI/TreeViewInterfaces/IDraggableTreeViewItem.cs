namespace Glitch9.EditorKit.IMGUI
{
    public interface IDraggableTreeViewItem
    {
        int Index { get; set; }
        bool IsDraggable { get; }
    }

}
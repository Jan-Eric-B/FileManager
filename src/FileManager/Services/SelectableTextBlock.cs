using System.Windows;
using System.Windows.Controls;

namespace FileManager.Services
{
    /// <summary>
    /// SelectableTextBlock
    /// Custom TextBLock for selecting Text in List
    /// https://stackoverflow.com/a/45627524/12273101
    /// </summary>
    public class SelectableTextBlock : TextBlock
    {
        private readonly TextEditorWrapper _editor;

        static SelectableTextBlock()
        {
            FocusableProperty.OverrideMetadata(typeof(SelectableTextBlock), new FrameworkPropertyMetadata(true));
            TextEditorWrapper.RegisterCommandHandlers(typeof(SelectableTextBlock), true, true, true);

            // remove the focus rectangle around the control
            FocusVisualStyleProperty.OverrideMetadata(typeof(SelectableTextBlock), new FrameworkPropertyMetadata((object)null));
        }
        public SelectableTextBlock()
        {
            _editor = TextEditorWrapper.CreateFor(this);
        }
    }
}
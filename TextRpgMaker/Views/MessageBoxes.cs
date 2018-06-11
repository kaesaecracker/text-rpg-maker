using Eto.Forms;

namespace TextRpgMaker.Views
{
    public static class MessageBoxes
    {
        public static void LoadFailedExceptionBox(LoadFailedException ex) => MessageBox.Show(
            parent: null,
            text: "The project could not be loaded.\n\n" +
                  $"Description:\n{ex.Message}\n\n" +
                  $"StackTrace:\n{ex.StackTrace}",
            caption: "Load failed",
            type: MessageBoxType.Error,
            buttons: MessageBoxButtons.OK
        );
    }
}
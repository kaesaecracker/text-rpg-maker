using Eto.Forms;

namespace TextRpgMaker.Views
{
    public static class MessageBoxes
    {
        public static void LoadFailedExceptionBox(LoadFailedException ex) => MessageBox.Show(
            parent: AppState.Ui,
            text: "The project could not be loaded.\n\n" +
                  $"Description:\n{ex.Message}\n\n" +
                  $"StackTrace:\n{ex.StackTrace}",
            caption: "Load failed",
            type: MessageBoxType.Error,
            buttons: MessageBoxButtons.OK
        );

        public static void InfoAboutLoadedProject() => MessageBox.Show(
            parent: AppState.Ui,
            text: $"Project path: \"{AppState.LoadedProject.ProjectDir}\"\n" +
                  $"Loaded elements: {AppState.LoadedProject.TopLevelElements.Count}\n" +
                  $"- ProjectInfo.Title: {AppState.LoadedProject.Info.Title}\n" +
                  $"- Weapons: {AppState.LoadedProject.WeaponTypes.Count}\n" +
                  $"- Ammo: {AppState.LoadedProject.AmmoTypes.Count}\n" +
                  $"- Armor: {AppState.LoadedProject.ArmorTypes.Count}\n" +
                  $"- Consumables: {AppState.LoadedProject.ConsumableTypes.Count}\n" +
                  $"- Start Characters: {AppState.LoadedProject.StartCharacters.Count}",
            caption: "Loaded Project",
            type: MessageBoxType.Information,
            buttons: MessageBoxButtons.OK
        );
    }
}
using System;
using System.Linq;
using Eto.Forms;

namespace TextRpgMaker.Views
{
    public static class MessageBoxes
    {
        public static void LoadFailedExceptionBox(Exception ex)
        {
            string msg = "The project could not be loaded.\n\n" +
                         $"Description: {ex.Message}\n\n" +
                         $"At: {ex.TargetSite.Name}\n\n";

            if (ex.InnerException != null)
            {
                msg += $"By : {ex.InnerException.GetType().Name} - {ex.InnerException.Message}";
            }

            MessageBox.Show(
                parent: AppState.Ui,
                text: msg,
                caption: "Load failed",
                type: MessageBoxType.Error,
                buttons: MessageBoxButtons.OK
            );
        }

        public static void InfoAboutLoadedProject() => MessageBox.Show(
            parent: AppState.Ui,
            text: $"Project path: \"{AppState.LoadedProject.ProjectDir}\"\n" +
                  $"Loaded elements: {AppState.LoadedProject.TopLevelElements.Count}\n" +
                  $"- ProjectInfo.Title: {AppState.LoadedProject.Info.Title}\n" +
                  $"- Weapons: {AppState.LoadedProject.WeaponTypes.Count}\n" +
                  $"- Ammo: {AppState.LoadedProject.AmmoTypes.Count}\n" +
                  $"- Armor: {AppState.LoadedProject.ArmorTypes.Count}\n" +
                  $"- Consumables: {AppState.LoadedProject.ConsumableTypes.Count}\n" +
                  $"- Characters: {AppState.LoadedProject.Characters.Count}\n" +
                  $"- Start Info:\n" +
                  $"  - Scene: {AppState.LoadedProject.Info.StartInfo.SceneId}" +
                  $"  - Dialog: {AppState.LoadedProject.Info.StartInfo.DialogId}" +
                  $"  - Characters: {AppState.LoadedProject.Info.StartInfo.CharacterIds.Aggregate((c, s) => $"{c}, {s}")}\n",
            caption: "Loaded Project",
            type: MessageBoxType.Information,
            buttons: MessageBoxButtons.OK
        );
    }
}
using System;
using System.Linq;
using Eto.Forms;

namespace TextRpgMaker.Views
{
    /// <summary>
    /// Helper MessageBox methods
    /// </summary>
    public static class MessageBoxes
    {
        public static void LoadFailedExceptionBox(Exception ex)
        {
            string msg = "The project could not be loaded.\n\n" +
                         $"Description: {ex.Message}\n\n" +
                         $"At: {ex.TargetSite.Name}\n\n";

            if (ex.InnerException != null)
                msg += $"By : {ex.InnerException.GetType().Name} - {ex.InnerException.Message}";

            MessageBox.Show(
                AppState.Ui,
                msg,
                "Load failed",
                type: MessageBoxType.Error,
                buttons: MessageBoxButtons.OK
            );
        }

        public static void InfoAboutLoadedProject()
        {
            MessageBox.Show(
                AppState.Ui,
                AppState.IsProjectLoaded
                    ? $"- Path: \"{AppState.Project.ProjectDir}\"\n" +
                      $"- ProjectInfo.Title: {AppState.Project.Info.Title}\n" +
                      $"- Elements: {AppState.Project.TopLevelElements.Count}\n" +
                      $"- Weapons: {AppState.Project.WeaponTypes.Count}\n" +
                      $"- Ammo: {AppState.Project.GenericItemTypes.Count}\n" +
                      $"- Armor: {AppState.Project.ArmorTypes.Count}\n" +
                      $"- Consumables: {AppState.Project.ConsumableTypes.Count}\n" +
                      $"- Characters: {AppState.Project.Characters.Count}\n" +
                      $"- Start Info:\n" +
                      $"  - Scene: {AppState.Project.StartInfo.SceneId}\n" +
                      $"  - Dialog: {AppState.Project.StartInfo.DialogId}\n" +
                      $"  - Characters: {AppState.Project.StartInfo.CharacterIds.Aggregate((c, s) => $"{c}, {s}")}\n"
                    : "No project loaded",
                "Loaded Project",
                type: MessageBoxType.Information,
                buttons: MessageBoxButtons.OK
            );
        }
    }
}
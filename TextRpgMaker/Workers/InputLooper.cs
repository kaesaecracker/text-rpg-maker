using System;
using System.Linq;
using TextRpgMaker.IO;
using TextRpgMaker.Models;
using static Serilog.Log;
using static TextRpgMaker.AppState;
using Dialog = TextRpgMaker.Models.Dialog;

namespace TextRpgMaker.Workers
{
    public class InputLooper
    {
        private IOutput Output { get; } = AppState.Ui.And(new LogOutput());
        private IInput Input { get; } = AppState.Ui;

        public InputLooper()
        {
            if (!IsProjectLoaded)
            {
                throw new InvalidOperationException(
                    "Cannot create InputLooper when no project is loaded");
            }

            this.CurrentScene = LoadedProject.Scenes
                                             .First(s => s.Id == LoadedProject.StartInfo.SceneId);
            this.HandleDialog(LoadedProject.StartInfo.DialogId);
        }

        private void HandleDialog(string dlgId)
            => this.HandleDialog(LoadedProject.Dialogs.First(s => s.Id == dlgId));

        private void HandleDialog(Dialog dlg)
        {
            this.Output.Write(dlg.Text);

            if (dlg.GotoId != null)
            {
                this.HandleDialog(dlg.GotoId);
                return;
            }

            // todo only allow choices that meet the requirements
            this.Input.GetChoiceAsync(dlg.Choices, choice =>
            {
                this.Output.Write($" >> {choice.Text}");
                this.HandleChoice(choice);
            });
        }

        private void HandleChoice(Choice choice)
        {
            // todo remove required items
            // todo give reward items
            if (choice.GotoId == null) return;

            this.HandleDialog(choice.GotoId);
        }

        public Scene CurrentScene { get; set; }
    }
}
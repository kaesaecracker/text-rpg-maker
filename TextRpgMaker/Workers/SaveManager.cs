using System.IO;
using System.Text;
using TextRpgMaker.Views;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Workers
{
    public static class SaveManager
    {
        private static readonly ISerializer serializer = new SerializerBuilder()
                                                         .EnsureRoundtrip()
                                                         .WithTagMapping("!Character", typeof(ProjectModels.Character))
                                                         .WithTagMapping("!Dialog", typeof(ProjectModels.Dialog))
                                                         .WithTagMapping("!Item", typeof(ProjectModels.Item))
                                                         .WithTagMapping("!ProjectInfo", typeof(ProjectModels.ProjectInfo))
                                                         .WithTagMapping("!Scene", typeof(ProjectModels.Scene))
                                                         .WithTagMapping("!Weapon", typeof(ProjectModels.Weapon))
                                                         .Build();

        public static void Save(string saveName)
        {
            var saveFolder = Path.Combine(AppState.Project.ProjectDir, "saves", saveName);

            // if it exists and the user confirms, delete the old folder
            if (Directory.Exists(saveFolder))
            {
                if (!new ConfirmationDialog
                {
                    Title = "Save",
                    Text = $"The save {saveName} already exists. Do you want to overwrite it?"
                }.ShowModal())
                {
                    return;
                }

                Directory.Delete(saveFolder, recursive: true);
            }

            Directory.CreateDirectory(saveFolder);
            Serialize(AppState.Project, Path.Combine(saveFolder, "project.yaml"));
            Serialize(AppState.Game, Path.Combine(saveFolder, "game.yaml"));
        }

        private static void Serialize(object obj, string pathToFile)
        {
            var writer = new StreamWriter(pathToFile, false, Encoding.UTF8);
            serializer.Serialize(writer, obj, obj.GetType());
        }
    }
}
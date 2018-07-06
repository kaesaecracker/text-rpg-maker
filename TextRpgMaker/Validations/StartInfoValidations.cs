using System.Linq;
using TextRpgMaker.ProjectModels;
using TextRpgMaker.Workers;

namespace TextRpgMaker.Validations
{
    [ValidatorClass]
    public static class StartInfoValidations
    {
        public static void StartCharactersExist(ProjectModel p)
        {
            var missingChars = (
                from charId in p.StartInfo.CharacterIds
                let character = p.Characters.FirstOrDefault(c => c.Id == charId)
                where character == null
                select charId
            ).ToList();

            if (missingChars.Count != 0)
                throw new ValidationFailedException(
                    "Start characters do not exist: " +
                    missingChars.Aggregate((curr, id) => $"{curr}, {id}")
                );
        }

        public static void StartSceneExists(ProjectModel p)
        {
            if (p.Scenes.All(s => s.Id != p.StartInfo.SceneId))
                throw new ValidationFailedException($"Start scene does not exist");
        }

        public static void StartDialogExists(ProjectModel p)
        {
            if (p.Dialogs.All(d => d.Id != p.StartInfo.DialogId))
                throw new ValidationFailedException("Start dialog does not exist");
        }
    }
}
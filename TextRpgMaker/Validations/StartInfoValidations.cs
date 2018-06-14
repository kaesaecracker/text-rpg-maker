using System.Linq;
using TextRpgMaker.Models;
using TextRpgMaker.Workers;

namespace TextRpgMaker.Validations
{
    public class StartInfoValidations
    {
        private void StartInfoIdsExist(Project p)
        {
            // start characters
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

            // start scene
            if (p.Scenes.All(s => s.Id != p.StartInfo.SceneId))
                throw new ValidationFailedException($"Start scene does not exist");

            // start dialog
            if (p.Dialogs.All(d => d.Id != p.StartInfo.DialogId))
                throw new ValidationFailedException("Start dialog does not exist");
        }
    }
}
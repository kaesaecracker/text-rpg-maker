using System.Linq;
using TextRpgMaker.Models;
using TextRpgMaker.Workers;

namespace TextRpgMaker.Validations
{
    [ValidatorClass]
    public static class CharacterValidations
    {
        public static void CharacterDropsExist(Project p)
        {
            // todo character drops exist
        }

        public static void CharacterTalkDialogExists(Project p)
        {
            var errors = (
                from character in p.Characters
                where character.TalkDialog != null
                      && p.Dialogs.All(d => d.Id != character.TalkDialog)
                select character
            ).ToList();
            if (!errors.Any()) return;

            string msg = "The following characters have a talk dialog that does not exist: ";
            foreach (var character in errors)
            {
                msg += $"\n- {character.Id} ({character.TalkDialog})";
            }
            
            throw new ValidationFailedException(msg);
        }
    }
}
﻿using System.Linq;
using TextRpgMaker.ProjectModels;
using TextRpgMaker.Workers;

namespace TextRpgMaker.Validations
{
    /// <summary>
    /// Validations for character
    /// </summary>
    [ValidatorClass]
    public static class CharacterValidations
    {
        // todo character drops exist
        // todo inventory items exist

        public static void CharacterTalkDialogExists(ProjectModel p)
        {
            var errors = (
                from character in p.Characters
                where character.TalkDialog != null
                      && p.Dialogs.All(d => d.Id != character.TalkDialog)
                select character
            ).ToList();
            if (!errors.Any()) return;

            string msg = "The following characters have a talk dialog that does not exist: ";
            foreach (var character in errors) msg += $"\n- {character.Id} ({character.TalkDialog})";

            throw new ValidationFailedException(msg);
        }
    }
}
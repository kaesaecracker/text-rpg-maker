using System;
using TextRpgMaker.Helpers;
using TextRpgMaker.Models;
using static TextRpgMaker.AppState;

namespace TextRpgMaker.Workers
{
    public static class GameInitializer
    {
        public static void StartNewGame()
        {
            Game = new GameState
            {
                CurrentScene = Project.ById<Scene>(Project.StartInfo.SceneId),
                CurrentDialog = Project.Dialogs.GetId(Project.StartInfo.DialogId)
            };

            var looper = new InputLooper();
            looper.StartFromNewGame();
        }

        public static void StartSavedGame(string pathToSave)
        {
            throw new NotImplementedException("TODO load game");
        }
    }
}
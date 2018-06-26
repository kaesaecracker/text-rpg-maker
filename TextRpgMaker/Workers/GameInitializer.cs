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
            // todo choose character
            
            
            Game = new GameState()
            {
                CurrentScene = Project.ById<Scene>(Project.StartInfo.SceneId),
                CurrentDialog = Project.Dialogs.GetId(Project.StartInfo.DialogId)
            };

            var looper = new InputLooper();
            looper.StartFromNewGame();
        }

        public static void StartSavedGame(string pathToSave)
        {
            // todo load game
            throw new NotImplementedException();
        }
    }
}
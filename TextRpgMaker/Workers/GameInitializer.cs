using System;
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
                CurrentScene = AppState.Project.ById<Scene>(AppState.Project.StartInfo.SceneId)
            };

            new InputLooper();
        }

        public static void StartSavedGame(string pathToSave)
        {
            // todo load game
            throw new NotImplementedException();
        }
    }
}
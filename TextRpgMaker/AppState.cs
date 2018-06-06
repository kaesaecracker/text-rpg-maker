using System.IO;
using TextRpgMaker.Models;

namespace TextRpgMaker
{
    public class AppState
    {
        public string PathToProject { get; private set; }

        public Project LoadedProject { get; private set; }

        public AppState(string pathToProject)
        {
            this.PathToProject = Path.GetDirectoryName(pathToProject);
            this.LoadedProject = new Project(this.PathToProject);
        }
    }
}
namespace TextRpgMaker.Models
{
    public class State
    {
        public string PathToProject { get; private set; }

        public Project LoadedProject { get; private set; }

        public State(string pathToProject)
        {
            this.PathToProject = pathToProject;
            this.LoadedProject = new Project(this.PathToProject);
        }
    }
}
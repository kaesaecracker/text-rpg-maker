namespace TextRpgMaker.Models
{
    public class State
    {
        private const string PathToProject = "../ExampleProject";
        
        public Project LoadedProject { get; set; } = new Project(PathToProject);
    }
}
using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    public class Element
    {
        [YamlMember(Alias = "id")]
        [YamlProperties(required: true)]
        public virtual string Id { get; set; }

        [YamlMember(Alias = "based-on")]
        public string BasedOnId { get; set; }

        [YamlIgnore]
        public LoadStep LoadStepDone { get; set; } = LoadStep.LoadFile;

        // TODO remember file in which it is defined
    }

    public enum LoadStep
    {
        LoadFile,
        RealizeInheritance,
        SetDefaultValues
    }
}
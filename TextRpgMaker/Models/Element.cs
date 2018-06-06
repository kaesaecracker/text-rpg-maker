using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    public class Element
    {
        [YamlMember(Alias = "id")]
        [Required]
        public virtual string Id { get; set; }

        [YamlMember(Alias = "based-on")]
        [DefaultValue(null)]
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
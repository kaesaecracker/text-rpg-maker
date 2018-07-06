using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.FileModels
{
    [DocumentedType]
    public class ElementFileModel
    {
        private string _name;

        [YamlMember(Alias = "id")]
        [YamlProperties(true)]
        public virtual string Id { get; set; }

        [YamlMember(Alias = "based-on")]
        public string BasedOnId { get; set; }

        [YamlMember(Alias = "name")]
        public virtual string Name
        {
            get => this._name ?? this.Id;
            set => this._name = value;
        }

        [YamlMember(Alias = "look-text")]
        public string LookText { get; set; }

        [YamlIgnore]
        public string OriginalFilePath { get; set; }

        public override string ToString() => $"[Element Id={this.Id}]";
    }
}
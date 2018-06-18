using System.ComponentModel;
using System.Runtime.CompilerServices;
using TextRpgMaker.Annotations;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    [DocumentedType]
    public class Element : INotifyPropertyChanged
    {
        [YamlMember(Alias = "id")]
        [YamlProperties(required: true)]
        public virtual string Id { get; set; }

        [YamlMember(Alias = "based-on")]
        public string BasedOnId { get; set; }

        [YamlMember(Alias = "name")]
        public virtual string Name { get; set; }

        [YamlMember(Alias = "look-text")]
        public string LookText { get; set; }

        [YamlIgnore]
        public string OriginalFilePath { get; set; }

        public override string ToString()
        {
            return $"[Element Id={this.Id}]";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
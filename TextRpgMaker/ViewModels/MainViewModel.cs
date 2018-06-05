using System.ComponentModel;
using System.Runtime.CompilerServices;
using TextRpgMaker.Annotations;

namespace TextRpgMaker
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        public string Output { get; } = "some output";

        public string SceneText { get; } = "Some information about where you are currently";

        public string PeopleText { get; } = "Some information about the people you can see";

        public string ObjectsText { get; } = "Some information about the items you can see";

        public int MaxHp { get; } = 100;

        public int Hp { get; } = 80;

        public string Weapon = "Sword";

        public string Head = "Helmet";

        public string Body = "Chainmail";

        public string Hand = "Gloves";

        public string Legs = "Pants";

        public string Shoes = "Shoes";

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
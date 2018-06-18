using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    [LoadFromProjectFile("characters.yaml", required: true, isList: true)]
    public class Character : Element
    {
        private double _currentHp;
        private double _health;
        private double _evade;
        private double _attack;
        private double _speed;

        [YamlMember(Alias = "current-hp")]
        [YamlProperties(required: false, defaultValue: 100)]
        public double CurrentHp
        {
            get => this._currentHp;
            set
            {
                if (value.Equals(this._currentHp)) return;
                this._currentHp = value;
                this.OnPropertyChanged();
            }
        }

        [YamlMember(Alias = "name")]
        [YamlProperties(required: true)]
        public override string Name { get; set; }

        [YamlMember(Alias = "health")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double Health
        {
            get => this._health;
            set
            {
                if (value.Equals(this._health)) return;
                this._health = value;
                this.OnPropertyChanged();
            }
        }

        [YamlMember(Alias = "evade")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double Evade
        {
            get => this._evade;
            set
            {
                if (value.Equals(this._evade)) return;
                this._evade = value;
                this.OnPropertyChanged();
            }
        }

        [YamlMember(Alias = "attack")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double Attack
        {
            get => this._attack;
            set
            {
                if (value.Equals(this._attack)) return;
                this._attack = value;
                this.OnPropertyChanged();
            }
        }

        [YamlMember(Alias = "speed")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double Speed
        {
            get => this._speed;
            set
            {
                if (value.Equals(this._speed)) return;
                this._speed = value;
                this.OnPropertyChanged();
            }
        }

        [YamlMember(Alias = "items")]
        public List<ItemGrouping> StartItems { get; set; } = new List<ItemGrouping>();

        [YamlMember(Alias = "drops")]
        public List<Drop> Drops { get; set; }
        
        [YamlMember(Alias = "talk-dialog")]
        public string TalkDialog { get; set; }
    }

    [DocumentedType]
    public class Drop
    {
        [YamlMember(Alias = "id")]
        [YamlProperties(required: true)]
        public string ItemId { get; set; }

        [YamlMember(Alias = "count")]
        [YamlProperties(required: false, defaultValue: 1)]
        public int Count { get; set; }

        [YamlMember(Alias = "chance")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double Chance { get; set; }
    }
}
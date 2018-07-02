using System;

namespace TextRpgMaker.Models
{
    public class AppConfig
    {
        private bool _debug;

        public bool Debug
        {
            get => this._debug;
            set
            {
                this.ValueChangedEvent?.Invoke(this, EventArgs.Empty);
                this._debug = value;
            }
        }

        public event EventHandler<EventArgs> ValueChangedEvent;
    }
}
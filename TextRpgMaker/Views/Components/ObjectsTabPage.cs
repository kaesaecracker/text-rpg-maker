﻿using Eto.Forms;

namespace TextRpgMaker.Views.Components
{
    public class ObjectsTabPage : TabPage
    {
        public ObjectsTabPage()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Objects";
            this.Content = new Label {Text = "objects panel goes here"};
        }
    }
}
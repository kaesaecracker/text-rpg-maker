﻿using Eto.Forms;

namespace TextRpgMaker.Views.Components
{
    public class StatsTabPage : TabPage
    {
        public StatsTabPage()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Stats";
            this.Content = new Label {Text = "equip panel goes here"};
        }
    }
}
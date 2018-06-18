﻿using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using GLib;
using Gtk;
using static Serilog.Log;
using TextRpgMaker.Models;
using Button = Eto.Forms.Button;
using ComboBox = Eto.Forms.ComboBox;
using Label = Eto.Forms.Label;

namespace TextRpgMaker.Views.Components
{
    public class InputPanel : Panel
    {
        public InputPanel()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Content = new GroupBox
            {
                Text = "Input",
                Font = UiConstants.GroupBoxTitleFont,
                Padding = 3,
                Content = new Label
                {
                    Text = "1. Load a game with [Project]->[Load]\n" +
                           "2. Start a new game with [Game]->[Start]"
                }
            };
        }

        public void GetChoiceAsync(List<Choice> dlgChoices, Action<Choice> action)
        {
            var combo = new ComboBox {DataStore = dlgChoices};
            var btn = new Button((s, a) =>
            {
                if (combo.SelectedIndex != -1)
                {
                    this.Content = null;
                    action.Invoke(dlgChoices[combo.SelectedIndex]);
                }
            })
            {
                Text = "Enter"
            };

            var layout = new DynamicLayout();
            layout.BeginHorizontal();
            layout.Add(combo);
            layout.Add(btn);
            this.Content = layout;
        }

        public void GetTextInput(Action<string> action)
        {
            var field = new TextBox {Width = 350};
            var btn = new Button((s, a) =>
            {
                this.Content = null;
                action.Invoke(field.Text);
            })
            {
                Text = "Enter"
            };

            var layout = new DynamicLayout();
            layout.BeginHorizontal();
            layout.Add(field);
            layout.Add(btn);
            this.Content = layout;
        }
    }
}
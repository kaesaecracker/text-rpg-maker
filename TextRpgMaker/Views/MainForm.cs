using System;
using System.ComponentModel.DataAnnotations;
using Eto.Drawing;
using Eto.Forms;

namespace TextRpgMaker.Views
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Title = "No project loaded [TextRpgMaker]";
            this.DataContext = new MainViewModel();
            this.Menu = InitializeMenu();
            
            var layout = new DynamicLayout
            {
                Padding = 3,
                DefaultSpacing = new Size(3, 3)
            };

            layout.BeginHorizontal();
            {
                layout.BeginVertical();
                {
                    layout.Add(new OutputPanel());
                    layout.Add(new InputPanel());
                }

                layout.EndBeginVertical();
                {
                    layout.Add(new InventoryPanel());
                    layout.Add(new InfoTabsPanel());
                }

                layout.EndBeginVertical();
                {
                    layout.Add(new VitalsPanel());
                    layout.Add(new CharacterPanel());
                }

                layout.EndVertical();
            }

            layout.EndHorizontal();
            this.Content = layout;
        }
        
        private void ExitClick(object sender, EventArgs e) => Application.Instance.Quit();

        internal static void UnimplementedClick(object sender, EventArgs e)
            => MessageBox.Show("Not implemented yet");
        
        
    }
}
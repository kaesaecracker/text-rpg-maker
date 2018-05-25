using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.ComTypes;
using Eto.Drawing;
using Eto.Forms;

namespace TextRpgMaker
{
    public partial class MainForm
    {
        private DynamicLayout InitializeLayout()
        {
            var layout = new DynamicLayout
            {
                Padding = 3,
                DefaultSpacing = new Size(3, 3)
            };
            layout.BeginHorizontal();

            this.AddCenter(layout);
            this.AddRight(layout);
            this.AddLeft(layout);

            layout.EndHorizontal();
            return layout;
        }

        private void AddLeft(DynamicLayout layout)
        {
            layout.Add(new GroupBox
            {
                Text = "Character",
                Padding = 3,
                Content = new StackLayout
                {
                    Orientation = Orientation.Vertical,
                    Padding = 3,
                    Items =
                    {
                        new StackLayout
                        {
                            Orientation = Orientation.Horizontal,
                            Items =
                            {
                                new Label {Text = "HP"},
                                new ProgressBar
                                {
                                    MinValue = 0,
                                    MaxValue = 100,
                                    Value = 90
                                }
                            }
                        },
                        new TabControl
                        {
                            Pages =
                            {
                                this.EquipTabPage(),
                                this.StatsTabPage()
                            }
                        }
                    }
                }
            });
        }

        private TabPage EquipTabPage()
        {
            return new TabPage
            {
                Text = "Equip",
                Padding = 3,
                Content = new TableLayout
                {
                    Rows =
                    {
                        new TableRow()
                        {
                            Cells =
                            {
                                new Label {Text = "Weapon"},
                                new Label {Text = "Some weapon"}
                            }
                        },
                        new TableRow()
                        {
                            Cells =
                            {
                                new Label {Text = "Head"},
                                new Label {Text = ""}
                            }
                        },
                        new TableRow()
                        {
                            Cells =
                            {
                                new Label {Text = "Body"},
                                new Label {Text = ""}
                            }
                        },
                        new TableRow()
                        {
                            Cells =
                            {
                                new Label {Text = "Hand"},
                                new Label {Text = "Simple leather gloves"}
                            }
                        },
                        new TableRow()
                        {
                            Cells =
                            {
                                new Label {Text = "Legs"},
                                new Label {Text = ""}
                            }
                        },
                        new TableRow()
                        {
                            Cells =
                            {
                                new Label {Text = "Shoes"},
                                new Label {Text = "Simple leather shoes of speed"}
                            }
                        },
                    }
                }
            };
        }

        private TabPage StatsTabPage()
        {
            return new TabPage
            {
                Text = "Stats",
                Content = new Label {Text = "equip panel goes here"}
            };
        }

        private void AddCenter(DynamicLayout layout)
        {
            layout.BeginVertical();

            layout.Add(new GroupBox
            {
                Text = "Output",
                Padding = 3,
                Content = new TextArea
                {
                    Height = 250
                }
            });
            layout.Add(new GroupBox
            {
                Text = "Input",
                Padding = 3,
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Items =
                    {
                        new ComboBox(),
                        new Button
                        {
                            Text = "Enter",
                            Command = new Command(UnimplementedClick)
                        }
                    }
                }
            });

            layout.EndVertical();
        }

        private void AddRight(DynamicLayout layout)
        {
            layout.BeginVertical();

            layout.Add(new GroupBox
            {
                Text = "Inventory",
                Padding = 3,
                Content = new ListBox
                {
                    Items =
                    {
                        "[5] Potion",
                        "[3] Gold"
                    }
                }
            });

            layout.Add(new TabControl
            {
                Pages =
                {
                    new TabPage
                    {
                        Text = "Scene",
                        Content = new Label {Text = "scene panel goes here"}
                    },
                    new TabPage
                    {
                        Text = "People",
                        Content = new Label {Text = "Persons panel goes here"}
                    },
                    new TabPage
                    {
                        Text = "Objects",
                        Content = new Label {Text = "objects panel goes here"}
                    }
                }
            });

            layout.EndVertical();
        }
    }
}
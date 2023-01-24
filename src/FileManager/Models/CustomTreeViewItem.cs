using FileManager.Resources.Settings;
using System.Collections.Generic;
using System.ComponentModel;

//https://www.codeproject.com/Articles/28306/Working-with-Checkboxes-in-the-WPF-TreeView

namespace FileManager.Models
{
    public class CustomTreeViewItem : INotifyPropertyChanged
    {
        private bool? _isChecked = false;
        private CustomTreeViewItem _parent;

        public static List<CustomTreeViewItem> CreateFoos()
        {
            Presets presets = new Presets();

            CustomTreeViewItem root = new CustomTreeViewItem("Save Presets", 23, "SavePresets")
            {
                IsInitiallySelected = false,
                IsChecked = presets.SavePresets,
                Children =
                {
                    
                    new CustomTreeViewItem("Search Options", 21, "SearchOptions")
                    {
                        IsChecked = presets.SearchOptions,
                        Children =
                        {
                            new CustomTreeViewItem("Path", 14, "SearchOptionsPath"){IsChecked = presets.SearchOptionsPath},
                            new CustomTreeViewItem("Search Input", 14, "SearchOptionsSearchInput"){IsChecked = presets.SearchOptionsSearchInput},
                            new CustomTreeViewItem("Subdirectories", 14, "SearchOptionsSubdirectory"){IsChecked = presets.SearchOptionsSubdirectory},
                            new CustomTreeViewItem("Case-Sensitive", 14, "SearchOptionsCaseSensitive"){IsChecked = presets.SearchOptionsCaseSensitive   },
                            new CustomTreeViewItem("File Name", 14, "SearchOptionsFileName"){IsChecked = presets.SearchOptionsFileName},
                            new CustomTreeViewItem("File Content", 14, "SearchOptionsFileContent"){IsChecked = presets.SearchOptionsFileContent},
                        }
                    },
                    new CustomTreeViewItem("General", 21, "General")
                    {
                        IsChecked = presets.General,
                        Children =
                        {
                            new CustomTreeViewItem("Card Collapse", 17, "GeneralCardCollapse")
                            {
                                IsChecked = presets.GeneralCardCollapse,
                                Children =
                                {
                                    new CustomTreeViewItem("Move", 14, "GeneralCardCollapseMove"){IsChecked = presets.GeneralCardCollapseMove},
                                    new CustomTreeViewItem("Delete", 14, "GeneralCardCollapseDelete"){IsChecked = presets.GeneralCardCollapseDelete},
                                    new CustomTreeViewItem("Rename", 14, "GeneralCardCollapseRename"){IsChecked = presets.GeneralCardCollapseRename},
                                }
                            },
                            new CustomTreeViewItem("Move", 17, "GeneralMove")
                            {
                                IsChecked = presets.GeneralMove,
                                Children =
                                {
                                    new CustomTreeViewItem("Subdirectory Name Input", 14, "GeneralMoveSubdirectoryNameInput"){IsChecked = presets.GeneralMoveSubdirectoryNameInput},
                                    new CustomTreeViewItem("Name of File / Count up", 14, "GeneralMoveNameOfFileOrCountUps"){IsChecked = presets.GeneralMoveNameOfFileOrCountUps},
                                }
                            },
                            new CustomTreeViewItem("Rename", 17, "GeneralRename")
                            {
                                IsChecked = presets.GeneralRename,
                                Children =
                               {
                                    new CustomTreeViewItem("Replace from", 14, "GeneralRenameReplaceFrom"){IsChecked = presets.GeneralRenameReplaceFrom},
                                    new CustomTreeViewItem("Replace to", 14, "GeneralRenameReplaceTo"){IsChecked = presets.GeneralRenameReplaceTo},
                                    new CustomTreeViewItem("Replace start index", 14, "GeneralRenameReplaceStartIndex"){IsChecked = presets.GeneralRenameReplaceStartIndex},
                                    new CustomTreeViewItem("Replace lenght", 14, "GeneralRenameReplaceLength"){IsChecked = presets.GeneralRenameReplaceLength},
                                    new CustomTreeViewItem("Replace Index / String", 14, "GeneralRenameReplaceIndexOrString"){IsChecked = presets.GeneralRenameReplaceIndexOrString},
                                    new CustomTreeViewItem("Swap Input", 14, "GeneralRenameSwapInput"){IsChecked = presets.GeneralRenameSwapInput},
                                    new CustomTreeViewItem("Swap part 1", 14, "GeneralRenameSwapPartOne"){IsChecked = presets.GeneralRenameSwapPartOne},
                                    new CustomTreeViewItem("Swap part 2", 14, "GeneralRenameSwapPartTwo"){IsChecked = presets.GeneralRenameSwapPartTwo},
                                    new CustomTreeViewItem("Insert Input", 14, "GeneralRenameInsertInput"){IsChecked = presets.GeneralRenameInsertInput},
                                    new CustomTreeViewItem("Insert Count up", 14, "GeneralRenameInsertCountUp"){IsChecked = presets.GeneralRenameInsertCountUp},
                                }
                            }
                        }
                    },
                }
            };

            root.Initialize();
            return new List<CustomTreeViewItem> { root };
        }

        public CustomTreeViewItem(string name, double? fontSize, string settingsKey)
        {
            this.Name = name;
            this.FontSize = fontSize;
            this.SettingsKey = settingsKey;
            this.Children = new List<CustomTreeViewItem>();
        }

        public void Initialize()
        {
            foreach (CustomTreeViewItem child in this.Children)
            {
                child._parent = this;
                child.Initialize();
            }
        }

        public List<CustomTreeViewItem> Children { get; private set; }

        public bool IsInitiallySelected { get; private set; }

        public string Name { get; private set; }

        public string SettingsKey { get; private set; }

        public double? FontSize { get; private set; }


        /// <summary>
        /// Gets/sets the state of the associated UI toggle (ex. CheckBox).
        /// The return value is calculated based on the check state of all
        /// child CustomTreeViewItems.  Setting this property to true or false
        /// will set all children to the same check state, and setting it
        /// to any value will cause the parent to verify its check state.
        /// </summary>
        public bool? IsChecked
        {
            get { return _isChecked; }
            set { this.SetIsChecked(value, true, true); }
        }

        private void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked)
                return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue)
                this.Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));

            if (updateParent && _parent != null)
                _parent.VerifyCheckState();

            this.OnPropertyChanged("IsChecked");
        }

        private void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < this.Children.Count; ++i)
            {
                bool? current = this.Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            this.SetIsChecked(state, false, true);
        }

        private void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));


                //if (this.IsChecked == true)
                //{
                //    Presets presets = new Presets();

                //    presets[this.SettingsKey] = true;
                //    presets.Save();
                //}
                //else if(this.IsChecked == false)
                //{
                //    Presets presets = new Presets();

                //    presets[this.SettingsKey] = false;
                //    presets.Save();
                //}
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
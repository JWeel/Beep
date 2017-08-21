using Beep.Rules;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace Beep.RuleUI {
    public abstract class BeepRuleUserControl : UserControl {

        public event EventHandler SelectedRule;
        public event EventHandler Deleting;

        protected StackPanel panelExpanded;
        protected DockPanel panelCollapsed;

        protected abstract void SetPanels();

        private bool collapsed;

        public bool Collapsed {
            get { return collapsed; }
            set {
                collapsed = value;
                if (value) {
                    panelExpanded.Visibility = Visibility.Collapsed;
                    panelCollapsed.Visibility = Visibility.Visible;
                }
                else {
                    panelExpanded.Visibility = Visibility.Visible;
                    panelCollapsed.Visibility = Visibility.Collapsed;
                }
            }
        }

        public abstract void UpdateColorPickers(ObservableCollection<ColorItem> usedColorItems);
        public abstract void PrepareColorPickers(ObservableCollection<ColorItem> standardColorItems);

        protected string ruleName;
        public string RuleName { get => ruleName; }

        public BeepRule Rule { get; set; }

        public virtual string SelectedRuleName { get; }

        public BeepRuleUserControl() { }

        //
        public static BeepRuleUserControl Create(BeepRule rule) {
            switch (rule.RuleName) {
                case BeepRule.RULE_CHANGE_COLOR:
                    return new ChangeColorRuleUserControl(rule as ChangeColorRule);
                case BeepRule.RULE_CHANGE_NEIGHBOR_COLOR:
                    return new ChangeNeighborColorRuleUserControl(rule as ChangeNeighborColorRule);
                case BeepRule.RULE_RANDOM_CHANGE:
                    return new RandomChangeRuleUserControl(rule as RandomChangeRule);
                case BeepRule.RULE_VIRUS:
                    return new VirusRuleUserControl(rule as VirusRule);
                default:
                    return null;
            }
        }

        protected void ComboBoxRuleSelected(object sender, RoutedEventArgs e) {
            SelectedRule?.Invoke(this, e);
        }
        protected void DeleteButtonClick(object sender, RoutedEventArgs e) {
            Deleting?.Invoke(this, e);
        }
        protected void BtnCollapseOrExpandClick(object sender, RoutedEventArgs e) {
            Collapsed = !Collapsed;
        }
    }
}

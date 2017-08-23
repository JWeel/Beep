using Beep.Rules;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace Beep.RuleUI {
    public abstract class BeepRuleUserControl : UserControl {

        public event EventHandler SelectedRule;
        public event EventHandler Deleting;
        public event EventHandler<MouseEventArgs> Dragging;

        protected Grid panelExpanded;
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
                case BeepRule.RULE_VINCENT:
                    return new VincentRuleUserControl(rule as VincentRule);
                case BeepRule.RULE_LIFE:
                    return new LifeRuleUserControl(rule as LifeRule);
                default:
                    return null;
            }
        }

        // reverse the state of the Collapsed property, which has a setter that handles UI visibility
        protected void BtnCollapseOrExpandClick(object sender, RoutedEventArgs e) {
            Collapsed = !Collapsed;
        }

        //
        protected void ComboBoxRuleSelected(object sender, RoutedEventArgs e) {
            if (SelectedRuleName != null) SelectedRule?.Invoke(this, e);
        }

        //
        protected void DeleteButtonClick(object sender, RoutedEventArgs e) {
            Deleting?.Invoke(this, e);
        }

        //
        protected void BtnDragMouseDown(object sender, MouseEventArgs e) {
            Dragging?.Invoke(this, e);
        }
    }
}

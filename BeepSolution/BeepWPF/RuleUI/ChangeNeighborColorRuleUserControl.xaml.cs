using Beep.Rules;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Beep.RuleUI {
    /// <summary>
    /// Interaction logic for ChangeNeighbor.xaml
    /// </summary>
    public partial class ChangeNeighborColorRuleUserControl : BeepRuleUserControl {

        public override string SelectedRuleName {
            get { return comboBoxRulePicker.SelectedItem as string; }
        }

        public override void UpdateColorPickers(ObservableCollection<ColorItem> usedColors) {
            clrPickMatch.AvailableColors = usedColors;
            //clrPickTarget.AvailableColors = usedColors;
        }

        protected override void SetPanels() {
            panelCollapsed = pnlCollapsed;
            panelExpanded = pnlExpanded;
        }

        public ChangeNeighborColorRuleUserControl(ChangeNeighborColorRule rule) {
            this.Rule = rule;
            this.ruleName = rule.RuleName;

            InitializeComponent();
            SetPanels();

            clrPickMatch.SelectedColor = rule.MatchColor;
            clrPickTarget.SelectedColor = rule.TargetColor;
        }
        
        private void AmountChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            (Rule as ChangeNeighborColorRule).AmountAffectedNeighbors = (int) e.NewValue;
        }

        private void ClrPickMatchChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as ChangeNeighborColorRule).MatchColor = (Color) e.NewValue;
        }

        private void ClrPickTargetChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as ChangeNeighborColorRule).TargetColor = (Color) e.NewValue;
        }
    }
}

using Beep.Rules;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Beep.RuleUI {
    /// <summary>
    /// Interaction logic for ChangeNeighbor.xaml
    /// </summary>
    public partial class VincentRuleUserControl : BeepRuleUserControl {

        public override string SelectedRuleName {
            get { return comboBoxRulePicker.SelectedItem as string; }
        }

        public override void UpdateColorPickers(ObservableCollection<ColorItem> usedColorItems) {
            clrPickMatch.AvailableColors = usedColorItems;
            clrPickTarget.AvailableColors = usedColorItems;
            clrPickIgnore.AvailableColors = usedColorItems;
        }

        public override void PrepareColorPickers(ObservableCollection<ColorItem> standardColorItems) {
            clrPickMatch.StandardColors = standardColorItems;
            clrPickTarget.StandardColors = standardColorItems;
            clrPickIgnore.StandardColors = standardColorItems;
        }

        protected override void SetInheritedComponents() {
            panelCollapsed = pnlCollapsed;
            panelExpanded = pnlExpanded;
            panelOptions = pnlOptions;
            checkEnabledCollapsed = chkEnabled1;
            checkEnabledExpanded = chkEnabled2;
        }

        public VincentRuleUserControl(VincentRule rule) {
            this.Rule = rule;
            this.ruleName = rule.RuleName;

            InitializeComponent();
            SetInheritedComponents();

            clrPickMatch.SelectedColor = rule.MatchColor;
            clrPickTarget.SelectedColor = rule.TargetColor;
            //clrPickIgnore.SelectedColor = rule.IgnoreColor;
        }

        private void ClrPickMatchChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as VincentRule).MatchColor = (Color)e.NewValue;
        }

        private void ClrPickTargetChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as VincentRule).TargetColor = (Color)e.NewValue;
        }

        private void ClrPickIgnoreChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as VincentRule).IgnoreColor = (Color)e.NewValue;
        }
    }
}

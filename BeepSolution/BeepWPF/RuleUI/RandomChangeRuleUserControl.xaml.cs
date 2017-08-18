using Beep.Rules;
using System.Windows;
using System.Windows.Media;

namespace Beep.RuleUI {
    /// <summary>
    /// Interaction logic for RandomChangeRuleUserControl.xaml
    /// </summary>
    public partial class RandomChangeRuleUserControl : BeepRuleUserControl {


        public override string SelectedRuleName {
            get { return comboBoxRulePicker.SelectedItem as string; }
        }

        protected override void SetPanels() {
            panelCollapsed = pnlCollapsed;
            panelExpanded = pnlExpanded;
        }

        public RandomChangeRuleUserControl(RandomChangeRule rule) {
            this.Rule = rule;
            this.ruleName = rule.RuleName;
            InitializeComponent();
            SetPanels();
            clrPickTarget.SelectedColor = rule.TargetColor;
        }

        private void AmountChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            (Rule as RandomChangeRule).PermillageAffected = (int)e.NewValue;
        }

        private void ClrPickTargetChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as RandomChangeRule).TargetColor = (Color)e.NewValue;
        }
    }
}

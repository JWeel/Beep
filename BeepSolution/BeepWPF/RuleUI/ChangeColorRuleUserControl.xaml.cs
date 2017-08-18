using Beep.Rules;
using System.Windows;
using System.Windows.Media;

namespace Beep.RuleUI {
    /// <summary>
    /// Interaction logic for ChangeColorRuleUserControl.xaml
    /// </summary>
    public partial class ChangeColorRuleUserControl : BeepRuleUserControl {

        public override string SelectedRuleName {
            get { return comboBoxRulePicker.SelectedItem as string; }
        }

        public string ViewType { get; set; }

        public ChangeColorRuleUserControl(ChangeColorRule rule) {
            this.Rule = rule;
            this.ruleName = rule.RuleName;

            InitializeComponent();

            clrPickMatch.SelectedColor = rule.MatchColor;
            clrPickTarget.SelectedColor = rule.TargetColor;
        }

        private void ClrPickMatchChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as ChangeColorRule).MatchColor = (Color)e.NewValue;
        }

        private void ClrPickTargetChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as ChangeColorRule).TargetColor = (Color)e.NewValue;
        }
    }
}

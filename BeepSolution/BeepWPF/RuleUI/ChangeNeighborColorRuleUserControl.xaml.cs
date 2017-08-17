using System.Windows;
using System.Windows.Controls;
using Beep.Rules;
using System.Diagnostics;
using System.Windows.Media;
using System;

namespace Beep.RuleUI {
    /// <summary>
    /// Interaction logic for ChangeNeighbor.xaml
    /// </summary>
    public partial class ChangeNeighborColorRuleUserControl : BeepRuleUserControl {

        public override string SelectedRuleName {
            get { return comboBoxRulePicker.SelectedItem as string; }
        }

        public ChangeNeighborColorRuleUserControl(ChangeNeighborColorRule rule) {
            this.Rule = rule;
            this.ruleName = rule.RuleName;

            InitializeComponent();

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

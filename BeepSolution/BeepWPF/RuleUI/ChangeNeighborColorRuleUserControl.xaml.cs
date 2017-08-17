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
        
        //public string RuleName;
        //public string Name;

        public ChangeNeighborColorRuleUserControl(ChangeNeighborColorRule rule) {
            this.Rule = rule;
            InitializeComponent();
            clrPickMatch.SelectedColor = rule.MatchColor;
            clrPickTarget.SelectedColor = rule.TargetColor;

            this.ruleName = rule.RuleName; // TODO doesnt work
            //this.Name = rule.RuleName;
        }

        //ChangeNeighborColorRule rule = new ChangeNeighborColorRule();

        private void BtnDeleteRuleClick(object sender, RoutedEventArgs e) {
            //rule.Run();
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

        //private void MnuRulePick_Selected(object sender, RoutedEventArgs e) {
        //    SelectedOtherRule(this, e);
        //}

        public override string SelectedRuleName {
            get { return mnuRulePick.SelectedItem as string; }
        }
        //public event EventHandler SelectedOtherRule;
    }
}

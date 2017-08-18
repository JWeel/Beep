using Beep.Rules;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Beep.RuleUI {
    public class BeepRuleUserControl : UserControl {

        public event EventHandler SelectedRule;
        public event EventHandler Deleting;

        protected string ruleName;
        public string RuleName { get => ruleName; }

        public BeepRule Rule { get; set; }

        public virtual string SelectedRuleName { get; }

        public BeepRuleUserControl() { }

        //
        public static BeepRuleUserControl CreateBeepRuleUserControl(BeepRule rule) {
            switch (rule.RuleName) {
                case BeepRule.RULE_CHANGE_COLOR:
                    return null;
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
    }
}

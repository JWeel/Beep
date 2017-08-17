using Beep.Rules;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Beep.RuleUI {
    public class BeepRuleUserControl : UserControl {

        public event EventHandler SelectedRule;

        protected string ruleName;
        public string RuleName { get => ruleName; }

        public BeepRule Rule { get; set; }

        public virtual string SelectedRuleName { get; }

        public BeepRuleUserControl() { }

        protected void MnuRulePick_Selected(object sender, RoutedEventArgs e) {
            SelectedRule(this, e);
        }

    }
}

using Beep.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Diagnostics;

namespace Beep.RuleUI
{
    /// <summary>
    /// Interaction logic for VirusRuleUserControl.xaml
    /// </summary>
    public partial class VirusRuleUserControl : BeepRuleUserControl {

        public override string SelectedRuleName {
            get { return comboBoxRulePicker.SelectedItem as string; }
        }

        public VirusRuleUserControl(VirusRule rule)
        {
            this.Rule = rule;
            this.ruleName = rule.RuleName;
                       
            InitializeComponent();
            clrPickMatch.SelectedColor = rule.MatchColor;
        }
        private void AmountChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            (Rule as VirusRule).ContagionRate = (int)e.NewValue;
        }

        private void ClrPickMatchChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as VirusRule).MatchColor = (Color)e.NewValue;
        }
    }
}

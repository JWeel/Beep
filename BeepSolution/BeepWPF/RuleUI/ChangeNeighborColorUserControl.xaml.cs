using System.Windows;
using System.Windows.Controls;
using Beep.Rules;
using System.Diagnostics;
using System.Windows.Media;

namespace Beep.RuleUI {
    /// <summary>
    /// Interaction logic for ChangeNeighbor.xaml
    /// </summary>
    public partial class ChangeNeighborColorUserControl : UserControl {

        private ChangeNeighborColorRule rule;

        public string RuleName;
        public string Name;

        public ChangeNeighborColorUserControl(ChangeNeighborColorRule rule) {
            this.rule = rule;
            InitializeComponent();
            clrPickMatch.SelectedColor = rule.MatchColor;
            clrPickTarget.SelectedColor = rule.TargetColor;

            this.RuleName = rule.RuleName; // TODO doesnt work
            this.Name = rule.RuleName;
        }

        //ChangeNeighborColorRule rule = new ChangeNeighborColorRule();

        private void BtnDeleteRuleClick(object sender, RoutedEventArgs e) {
            //rule.Run();
        }



        private void AmountChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            rule.AmountAffectedNeighbors = (int) e.NewValue;
        }

        private void ClrPickMatchChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            Debug.WriteLine("Here");
            Debug.WriteLine(rule.MatchColor);
            rule.MatchColor = (Color) e.NewValue;
            Debug.WriteLine(rule.MatchColor);
        }

        private void ClrPickTargetChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            Debug.WriteLine(rule.TargetColor);
            rule.TargetColor = (Color) e.NewValue;
            Debug.WriteLine(rule.TargetColor);
        }
    }
}

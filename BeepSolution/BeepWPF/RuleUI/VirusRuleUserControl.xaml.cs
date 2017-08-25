using Beep.Rules;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Beep.RuleUI {
    /// <summary>
    /// Interaction logic for VirusRuleUserControl.xaml
    /// </summary>
    public partial class VirusRuleUserControl : BeepRuleUserControl {

        public override string SelectedRuleName {
            get { return comboBoxRulePicker.SelectedItem as string; }
        }

        public override void UpdateColorPickers(ObservableCollection<ColorItem> usedColorItems) {
            clrPickMatch.AvailableColors = usedColorItems;
        }

        public override void PrepareColorPickers(ObservableCollection<ColorItem> standardColorItems) {
            clrPickMatch.StandardColors = standardColorItems;
        }

        protected override void SetInheritedComponents() {
            panelCollapsed = pnlCollapsed;
            panelExpanded = pnlExpanded;
            panelOptions = pnlOptions;
            checkEnabledCollapsed = chkEnabled1;
            checkEnabledExpanded = chkEnabled2;
        }

        public VirusRuleUserControl(VirusRule rule) {
            this.Rule = rule;
            this.ruleName = rule.RuleName;
                       
            InitializeComponent();
            SetInheritedComponents();
            clrPickMatch.SelectedColor = rule.MatchColor;
            rule.MouseColor = MainWindow.MouseClickColor;
            rule.ColorNeighboringMatchers = ChangeVirusColor.IsChecked.Value;
        }
        private void AmountChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            (Rule as VirusRule).ContagionRate = (int)e.NewValue;
        }

        private void ClrPickMatchChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as VirusRule).MatchColor = (Color)e.NewValue;
        }

        private void ChangeVirusColorClick(object sender, RoutedEventArgs e) {
            
           (Rule as VirusRule).ColorNeighboringMatchers = ChangeVirusColor.IsChecked.Value;
        }
    }
}

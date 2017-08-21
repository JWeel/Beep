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

        protected override void SetPanels() {
            panelCollapsed = pnlCollapsed;
            panelExpanded = pnlExpanded;
        }

        public VirusRuleUserControl(VirusRule rule) {
            this.Rule = rule;
            this.ruleName = rule.RuleName;
                       
            InitializeComponent();
            SetPanels();
            clrPickMatch.SelectedColor = rule.MatchColor;
            rule.MouseColor = MainWindow.MOUSE_CLICK_COLOR;
        }
        private void AmountChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            (Rule as VirusRule).ContagionRate = (int)e.NewValue;
        }

        private void ClrPickMatchChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as VirusRule).MatchColor = (Color)e.NewValue;
        }
    }
}

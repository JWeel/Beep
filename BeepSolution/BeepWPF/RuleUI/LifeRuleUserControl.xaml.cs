using Beep.Rules;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Beep.RuleUI {
    /// <summary>
    /// Interaction logic for ChangeNeighbor.xaml
    /// </summary>
    public partial class LifeRuleUserControl : BeepRuleUserControl {

        public override string SelectedRuleName {
            get { return comboBoxRulePicker.SelectedItem as string; }
        }

        public override void UpdateColorPickers(ObservableCollection<ColorItem> usedColorItems) {
            clrPickLife.AvailableColors = usedColorItems;
            clrPickDead.AvailableColors = usedColorItems;
        }

        public override void PrepareColorPickers(ObservableCollection<ColorItem> standardColorItems) {
            clrPickLife.StandardColors = standardColorItems;
            clrPickDead.StandardColors = standardColorItems;
        }

        protected override void SetInheritedComponents() {
            panelCollapsed = pnlCollapsed;
            panelExpanded = pnlExpanded;
        }

        public LifeRuleUserControl(LifeRule rule) {
            this.Rule = rule;
            this.ruleName = rule.RuleName;

            InitializeComponent();
            SetInheritedComponents();

            clrPickLife.SelectedColor = rule.LifeColor;
            clrPickDead.SelectedColor = rule.DeadColor;
            //clrPickIgnore.SelectedColor = rule.IgnoreColor;
        }

        private void ClrPickLifeChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as LifeRule).LifeColor = (Color)e.NewValue;
        }

        private void ClrPickDeadChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as LifeRule).DeadColor = (Color)e.NewValue;
        }

        private void OnIgnoreChecked(object sender, RoutedEventArgs e) {
            (Rule as LifeRule).ConvertUnrelatedCells = false;
        }

        private void OnIgnoreUnchecked(object sender, RoutedEventArgs e) {
            (Rule as LifeRule).ConvertUnrelatedCells = true;
        }
    }
}

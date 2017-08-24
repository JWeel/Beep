using Beep.Rules;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Beep.RuleUI {
    /// <summary>
    /// Interaction logic for RandomChangeRuleUserControl.xaml
    /// </summary>
    public partial class RandomChangeRuleUserControl : BeepRuleUserControl {


        public override string SelectedRuleName {
            get { return comboBoxRulePicker.SelectedItem as string; }
        }

        public override void UpdateColorPickers(ObservableCollection<ColorItem> usedColorItems) {
            clrPickTarget.AvailableColors = usedColorItems;
        }

        public override void PrepareColorPickers(ObservableCollection<ColorItem> standardColorItems) {
            clrPickTarget.StandardColors = standardColorItems;
        }

        protected override void SetInheritedComponents() {
            panelCollapsed = pnlCollapsed;
            panelExpanded = pnlExpanded;
            panelOptions = pnlOptions;
            checkEnabledCollapsed = chkEnabled1;
            checkEnabledExpanded = chkEnabled2;
        }

        public RandomChangeRuleUserControl(RandomChangeRule rule) {
            this.Rule = rule;
            this.ruleName = rule.RuleName;
            InitializeComponent();
            SetInheritedComponents();
            clrPickTarget.SelectedColor = rule.TargetColor;
        }

        private void AmountChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            (Rule as RandomChangeRule).PermillageAffected = (int)e.NewValue;
        }

        private void ClrPickTargetChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            (Rule as RandomChangeRule).TargetColor = (Color)e.NewValue;
        }

        private void OnFullRandomUnchecked(object sender, RoutedEventArgs e) {
            (Rule as RandomChangeRule).FullyRandomizeColors = false;
        }

        private void OnFullRandomChecked(object sender, RoutedEventArgs e) {
            (Rule as RandomChangeRule).FullyRandomizeColors = true;
        }

        private void OnAlwaysSameUnchecked(object sender, RoutedEventArgs e) {
            if (this.IsInitialized) {
                (Rule as RandomChangeRule).UseFixedColor = false;
                txtRule3.Visibility = Visibility.Collapsed;
                clrPickTarget.Visibility = Visibility.Collapsed;
                txtRule4.Visibility = Visibility.Visible;
                chkFullRandom.Visibility = Visibility.Visible;
            }
        }

        private void OnAlwaysSameChecked(object sender, RoutedEventArgs e) {
            if (this.IsInitialized) {
                (Rule as RandomChangeRule).UseFixedColor = true;
                txtRule3.Visibility = Visibility.Visible;
                clrPickTarget.Visibility = Visibility.Visible;
                txtRule4.Visibility = Visibility.Collapsed;
                chkFullRandom.Visibility = Visibility.Collapsed;
            }
        }
    }
}

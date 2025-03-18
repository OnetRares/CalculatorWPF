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
using System.Windows.Shapes;

namespace CalculatorWPF
{
  
    public partial class ExpressionInputWindow : Window
    {
        public string themeName {  get; set; }
        public ExpressionInputWindow()
        {
            InitializeComponent();
            var viewModel = new ExpressionInputViewModel();
            viewModel.CalculationCompleted += OnCalculationCompleted;
            themeName = "BlueTheme";
            DataContext = viewModel;
        }

        public void ChangeTheme(string themeName)
        {
            var theme = new ResourceDictionary { Source = new Uri($"Themes/{themeName}.xaml", UriKind.Relative) };

            this.Resources.MergedDictionaries.Clear();
            this.Resources.MergedDictionaries.Add(theme);

            this.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Themes/Style.xaml", UriKind.Relative) });
        }
        private void OnCalculationCompleted(string result)
        {
            if (result != null)
            {
                MessageBox.Show($"Result: {result}", "Calculate completed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.Close();
        }
    }
}

using Alkambia.App.LoanMonitoring.Helper.Runtime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Alkambia.WPF.LoanMonitoring
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var path = Configuration.Instance.ThemeUri;
            if (!string.IsNullOrWhiteSpace(path))
            {
                var currentTheme = this.Resources.MergedDictionaries
                .FirstOrDefault(c => c.Source == new Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.DeepPurple.xaml"));
                if (currentTheme != null)
                {
                    this.Resources.MergedDictionaries.Remove(currentTheme);

                    //new resource
                    var resourceDic = new ResourceDictionary();
                    resourceDic.Source = new Uri(path);
                    this.Resources.MergedDictionaries.Add(resourceDic);
                }
            }
        }
    }
}

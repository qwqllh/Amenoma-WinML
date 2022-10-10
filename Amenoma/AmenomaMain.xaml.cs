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
using System.Globalization;
using System.Resources;
using Amenoma.Properties;

namespace Amenoma
{
    /// <summary>
    /// AmenomaMain.xaml 的交互逻辑
    /// </summary>
    public partial class AmenomaMain : Window
    {
        private Dictionary<string, ResourceDictionary> _resourceDictionaryMap = new Dictionary<string, ResourceDictionary>();

        private void LoadI18nSettings()
        {
            foreach (var dir in Application.Current.Resources.MergedDictionaries)
            {
                _resourceDictionaryMap.Add(dir.Source.OriginalString, dir);
            }

            CultureInfo cultureInfo = CultureInfo.InstalledUICulture;
            string resourceDictionaryPath = string.Format(@"i18n\{0}.xaml", cultureInfo.Name);
            if (_resourceDictionaryMap.TryGetValue(resourceDictionaryPath, out var resourceDictionary))
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                LanguageComboBox.Text = cultureInfo.Name;
                Settings.Default.Language = cultureInfo.Name;
            }
            else if (_resourceDictionaryMap.TryGetValue(@"i18n\en-US.xaml", out resourceDictionary))
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                LanguageComboBox.Text = "en-US";
                Settings.Default.Language = "en-US";
            }
        }

        /// <summary>
        /// Change program UI language to the target language.
        /// </summary>
        /// <param name="language">
        /// The target language. For example, pass "zh-CN" to use Simplified Chinese.
        /// </param>
        /// <returns>
        /// <c>true</c> if succeeded. If the specified i18n file doesn't exist, return false.
        /// </returns>
        public bool TryChangeLanguage(string language)
        {
            if (_resourceDictionaryMap.TryGetValue(string.Format(@"i18n\{0}.xaml", language), out var resourceDictionary))
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                Settings.Default.Language = language;
                LanguageComboBox.Text = language;
                return true;
            }
            return false;
        }

        public AmenomaMain()
        {
            InitializeComponent();
            LoadI18nSettings();
        }

        private void ArtifactLevel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool check = ushort.TryParse(e.Text, out _);
            if (!check)
                e.Handled = true;
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow();
            about.Owner = this;
            about.ShowDialog();
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var item = (ComboBoxItem)e.AddedItems[0];
                TryChangeLanguage(item.Content.ToString());
            }
        }
    }
}

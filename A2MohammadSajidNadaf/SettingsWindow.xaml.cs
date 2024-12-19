using Microsoft.Azure.Amqp.Framing;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace A2MohammadSajidNadaf
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void SaveTextColor_Click(object sender, RoutedEventArgs e)
        {
            if (colorComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedColor = selectedItem.Content.ToString();

                using (var db = new AppDbContext())
                {
                    var textColorSetting = db.Settings.FirstOrDefault(s => s.Key == "TextColor");
                    if (textColorSetting != null)
                    {
                        textColorSetting.Value = selectedColor;
                    }
                    else
                    {
                        db.Settings.Add(new Setting { Key = "TextColor", Value = selectedColor });
                    }
                    db.SaveChanges();
                }

                MessageBox.Show($"Text color saved as {selectedColor}.");
            }
            else
            {
                MessageBox.Show("Please select a color.");
            }
        }

        private void ClearHistory_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new AppDbContext())
            {
                db.Histories.RemoveRange(db.Histories);
                db.SaveChanges();
            }
            MessageBox.Show("Calculation history cleared.");
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

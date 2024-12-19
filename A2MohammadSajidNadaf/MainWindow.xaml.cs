using Microsoft.Azure.Amqp.Framing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace A2MohammadSajidNadaf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Username { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ApplyTextColor();
        }

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            tb.Text += b.Content.ToString();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            tb.Text = "";
        }

        private void btnR_Click(object sender, RoutedEventArgs e)
        {
            if (tb.Text.Length > 0)
            {
                tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
            }
        }

        private void btnOff_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnEqualsTo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                result();
            }
            catch (Exception)
            {
                tb.Text = "Error!";
            }
        }

        private void result()
        {
            String op;
            int iOp = 0;
            double result = 0;

            if (tb.Text.Contains("+"))
            {
                iOp = tb.Text.IndexOf("+");
            }
            else if (tb.Text.Contains("-"))
            {
                iOp = tb.Text.IndexOf("-");
            }
            else if (tb.Text.Contains("*"))
            {
                iOp = tb.Text.IndexOf("*");
            }
            else if (tb.Text.Contains("/"))
            {
                iOp = tb.Text.IndexOf("/");
            }
            else
            {
                tb.Text = "Error!";
                return;
            }

            op = tb.Text.Substring(iOp, 1);
            try
            {
                double op1 = Convert.ToDouble(tb.Text.Substring(0, iOp));
                double op2 = Convert.ToDouble(tb.Text.Substring(iOp + 1, tb.Text.Length - iOp - 1));

                if (op == "+")
                {
                    tb.Text += "=" + (op1 + op2);
                }
                else if (op == "-")
                {
                    tb.Text += "=" + (op1 - op2);
                }
                else if (op == "*")
                {
                    tb.Text += "=" + (op1 * op2);
                }
                else if (op == "/")
                {
                    if (op2 == 0)
                    {
                        tb.Text = "∞";
                    }
                    else
                    {
                        tb.Text += "=" + (op1 / op2);
                    }
                }

                using (var db = new AppDbContext())
                {
                    _ = db.Histories.Add(new CalculationHistory
                    {
                        Username = CurrentUser.Username,
                        Expression = tb.Text,
                        Result = int.Parse(result.ToString())
                    });
                    db.SaveChanges();
                }
            }
            catch (FormatException)
            {
                tb.Text = "Invalid Input!";
            }
            catch (Exception)
            {
                tb.Text = "Error!";
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();

            ApplyTextColor();
        }

        private void ApplyTextColor()
        {
            using (var db = new AppDbContext())
            {
                var textColorSetting = db.Settings.FirstOrDefault(s => s.Key == "TextColor");

                if (textColorSetting != null)
                {
                    tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(textColorSetting.Value));
                }
                else
                {
                    tb.Foreground = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void ViewHistory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentUser.Username))
            {
                MessageBox.Show("Please log in first to view history.", "Authentication Required");
                return;
            }

            using (var db = new AppDbContext())
            {
                var history = db.Histories
                                .Where(h => h.Username == CurrentUser.Username)
                                .ToList();

                if (history.Any())
                {
                    string historyText = string.Join("\n", history.Select(h => $"{h.Expression} = {h.Result} ({h.Timestamp})"));
                    MessageBox.Show(historyText, "Calculation History");
                }
                else
                {
                    MessageBox.Show("No history available.", "Calculation History");
                }
            }
        }
    }
}
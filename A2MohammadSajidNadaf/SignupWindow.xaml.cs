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

namespace A2MohammadSajidNadaf
{
    /// <summary>
    /// Interaction logic for SignupWindow.xaml
    /// </summary>
    public partial class SignupWindow : Window
    {
        public SignupWindow()
        {
            InitializeComponent();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            using (var db = new AppDbContext())
            {
                if (db.Users.Any(u => u.Username == username))
                {
                    MessageBox.Show("Username already exists.");
                }
                else
                {
                    User newUser = new User
                    {
                        Username = username,
                        Password = password
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();
                    MessageBox.Show("Signup successful! Please login.");
                    this.Close();
                }
            }
        }
    }
}

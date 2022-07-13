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

namespace M14_Task
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        public event Action<string, string, string> Create;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (newFamilyName.Text == "" ||
                 newFirstName.Text == "" ||
                 newPatronymicName.Text == ""
                 )
                MessageBox.Show("все поля должны быть заполнены");
            else
            {
                Create(newFamilyName.Text,
                    newFirstName.Text, newPatronymicName.Text);
                this.Hide();
            }
        }

        private void OnDeactivate(object sender, DependencyPropertyChangedEventArgs e)
        {
            newFamilyName.Text = "";
            newFirstName.Text = "";
            newPatronymicName.Text = "";
        }
    }
}

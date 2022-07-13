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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        public event Action<string, string, string> Create;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (newOrganizationName.Text == "" ||
                newINN.Text == "" ||
                newRepresentative.Text == ""
                )
                MessageBox.Show("все поля должны быть заполнены");
            else 
            {
                Create(newOrganizationName.Text,
                    newINN.Text, newRepresentative.Text);
                this.Hide();
            }

        }

        private void OnDeactivate(object sender, DependencyPropertyChangedEventArgs e)
        {
            newOrganizationName.Text = "";
            newINN.Text = "";
            newRepresentative.Text = "";
        }
    }
}

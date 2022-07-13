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
using System.Windows.Navigation;
using System.Windows.Shapes;
using М13_Task1;

namespace M14_Task
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BankSystem bankA = new BankSystem();  // банк
        public ManagerForNewClient user1;     // вносит данные о новых клиентах
        AccountManager user2;                 // работает со счетами
        DepositManager user3;                 // работает с депозитами
        Consultant user4;                     // консультант

        History history;                      // журналы

        Window1 createOrganisationForm = new Window1();
        Window2 createPersonForm = new Window2();

 

        public MainWindow()
        {
           
            bankA.MakeCash();   // открыть кассу
            
            user1 = new ManagerForNewClient("Алексей", bankA);
            createOrganisationForm.Create += user1.MFNewOrganisationClient;
            createPersonForm.Create += user1.MFNewPersonClient; 

            user2 = new AccountManager("Андрей", bankA);
            user1.Create += user2.OnNewClientAdd;

            user3 = new DepositManager("Артем", bankA);
            user4 = new Consultant("Антон", bankA);

            history = new History(user1, user2, user3,user4);

            user1.MFNewPersonClient("Иванов", "Иван", "Иванович");
            user1.TheClient.Phone = "123456";
    

            user1.MFNewOrganisationClient("OOO Ромашка", "112", "Романов Роман Романович");
            user1.TheClient.Phone = "978564";
        


            InitializeComponent();

            ClientList.ItemsSource = bankA.Clients;
            cash.DataContext = user2.Cash;
            managerForNewClient.DataContext = user1;
            accountManager.DataContext = user2;
            depositManager.DataContext = user3;
            consultant.DataContext = user4;
            consultant.IsSelected = true;
            clientChangeHistory.ItemsSource = history.addClient;
            transfers.ItemsSource = history.transfers;
            
        }


        private void OnClosed(object sender, EventArgs e)
        {
            createOrganisationForm.Close();
            createPersonForm.Close();
        }


        // ФУНКЦИОНАЛ КОНСУЛЬТАНТА


        /// <summary>
        /// выбран клиент в списке клиентов - с ним поработает консультант
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientList.SelectedItem != null) 
            {
                AccountList.UnselectAll();
                Client selectedClient = ClientList.SelectedItem as Client;
                AccountList.ItemsSource = selectedClient.Accounts;
                user4.TheClient = selectedClient;
            }
            
         }

        /// <summary>
        /// выбор исполнителя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl x = sender as TabControl;
            foreach(TabItem item in x.Items)
                if(item.IsSelected == true) 
                {
                    User user = item.DataContext as User;
                    helloMessage.Text = user.HelloMessage();
                }
            AccountList.UnselectAll();
            ClientList.UnselectAll();
        }

        /// <summary>
        /// перенаправление клиента консультантом к менеджеру по работе со счетами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonToAccountManager_Click(object sender, RoutedEventArgs e)
        {
            accountManager.IsSelected = true;
            user2.TheClient = user4.TheClient; user4.TheClient = null;
            MessageBox.Show($"Вам поможет менеджер {user2.Name}");
        }

        /// <summary>
        /// перенаправление клиента консультантом к менеджеру по работе с депозитами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonToDepositManager_Click(object sender, RoutedEventArgs e)
        {
            depositManager.IsSelected=true;
            user3.TheClient = user4.TheClient; user4.TheClient = null;
            MessageBox.Show($"Вам поможет менеджер {user3.Name}");
        }

        /// <summary>
        /// перенаправление клиента консультантом к менеджеру по работе с данными клиентов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonToNewClientManager_Click(object sender, RoutedEventArgs e)
        {
            managerForNewClient.IsSelected = true;
            MessageBox.Show($"Вам поможет менеджер {user1.Name}");
        }

        //ФУНКЦИОНАЛ МЕНЕДЖЕРА ПО СЧЕТАМ

        /// <summary>
        /// выбор счета для работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accountListOfAccountManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (accountListOfAccountManager.SelectedItem != null) 
            {
                Account x = accountListOfAccountManager.SelectedItem as Account;
                user2.AccountInWork = x;
                selectedAccount.Text = x.AccountNumber;
            }
        }

        /// <summary>
        /// внесение денег в кассу банка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCashAdd_Click(object sender, RoutedEventArgs e)
        {
            float sum = 0;
            if (float.TryParse(amount.Text, out sum)) 
            {
                user2.MPutMoneyToCash(sum);
                transfers.Items.Refresh();
            }
                
            else MessageBox.Show("не указана сумма");
        }


        /// <summary>
        /// перевод денег из кассы на счет клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPutMoneyToAccount_Click(object sender, RoutedEventArgs e)
        {
            if (user2.AccountInWork != null) 
            {
                float sum = 0;
                Account x = accountListOfAccountManager.SelectedItem as Account;
                if (float.TryParse(amount.Text, out sum))
                {
                    if(user2.MPutMoney(sum)) 
                    { 
                        accountListOfAccountManager.Items.Refresh();
                        AccountList.Items.Refresh();
                        transfers.Items.Refresh();
                    }
                    else MessageBox.Show("не удалось внести деньги на счет");
                }
                else MessageBox.Show("не указана сумма");
            }
            else MessageBox.Show("не выбран счет");
        }

        /// <summary>
        /// снятие денег со счета клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTakeMoneyFromAccount_Click(object sender, RoutedEventArgs e)
        {
            if(user2.AccountInWork!=null)
            {
                float sum = 0;
                if (float.TryParse(amount.Text, out sum))
                {
                    if (user2.MTakeMoney(sum))
                    {
                        accountListOfAccountManager.Items.Refresh();
                        AccountList.Items.Refresh();
                        transfers.Items.Refresh();
                        user2.MTakeMoneyFromCash(sum);
                        MessageBox.Show($"выданы наличные в сумме {sum}");
                    }
                    else MessageBox.Show("не удалось снять со счета");
                }
                else MessageBox.Show("не указана сумма");
            }
            else MessageBox.Show("не выбран счет");
        }


        /// <summary>
        /// перевод денег другому клиенту
        /// !!! putAccount.Text должно быть введено
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (user2.AccountInWork != null)
            {
                Account put = bankA.accounts[putAccount.Text];
                if (put != null)
                {
                    float sum = 0;
                    if (float.TryParse(amount.Text, out sum))
                    {
                        user2.MTransfer(user2.AccountInWork, put, sum);
                        accountListOfAccountManager.Items.Refresh();
                        AccountList.Items.Refresh();
                        transfers.Items.Refresh();
                    }
                    else MessageBox.Show("не указана сумма");
                }
                else MessageBox.Show("не верный счет получателя");
            }
            else MessageBox.Show("не выбран счет отправителя");
        }

        /// <summary>
        /// открыть новый счет клиенту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNewAccount_Click(object sender, RoutedEventArgs e)
        {
            user2.MNewAccount();
            accountListOfAccountManager.Items.Refresh();
            AccountList.Items.Refresh();
        }

        /// <summary>
        /// закрыть счет клиенту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCloseAccount_Click(object sender, RoutedEventArgs e)
        {
            if (user2.AccountInWork != null)
            {
                user2.MCloseAccount();
                accountListOfAccountManager.UnselectAll();
                selectedAccount.Text = "";
            }
            else MessageBox.Show("не выбран счет");

        }

        /// <summary>
        /// выбрать клиента для работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAccountManagerStart_Click(object sender, RoutedEventArgs e)
        {
            if (ClientList.SelectedItem == null)
                MessageBox.Show("клиент не выбран");
            else
            {
                user2.TheClient = ClientList.SelectedItem as Client;
            }
        }

        /// <summary>
        /// закончить работу с клиентом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAccountManagerFinish_Click(object sender, RoutedEventArgs e)
        {
            user2.TheClient = null;
        }


        //ФУНКЦИОНАЛ МЕНЕДЖЕРА ПО ДЕПОЗИТАМ


        /// <summary>
        /// выбор счета и депозита для работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accountListOfDepositManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (accountListOfDepositManager.SelectedItem != null)
            {
                Account x = accountListOfDepositManager.SelectedItem as Account;
                if (accountListOfDepositManager.SelectedItem.GetType() == typeof(Account))
                {
                    selectedAccount2.Text = x.AccountNumber;
                    user3.AccountInWork = x;
                }

                if (accountListOfDepositManager.SelectedItem.GetType() == typeof(DepositAccount))
                {
                    selectedDeposit.Text = x.AccountNumber;
                    user3.DepositInWork = x as DepositAccount;
                }

            }

        }

        /// <summary>
        /// открыть депозит клиенту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNewDeposit_Click(object sender, RoutedEventArgs e)
        {
            user3.MNewDepositAccount();
            accountListOfDepositManager.Items.Refresh();
            AccountList.Items.Refresh();

        }

        /// <summary>
        /// закрыть депозит
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCloseDeposit_Click(object sender, RoutedEventArgs e)
        {
            if (user3.DepositInWork != null)
            {
                user3.MCloseDeposit();
                accountListOfDepositManager.UnselectAll();
                selectedDeposit.Text = "";
            }
            else MessageBox.Show("не выбран депозит");

        }

        /// <summary>
        /// положить вклад на депозит
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPutMoneyToDeposit_Click(object sender, RoutedEventArgs e)
        {
            float sum = 0;
            if (float.TryParse(amount2.Text, out sum))
            {
                if (user3.MPutMoneyToDeposit(sum))
                {
                    accountListOfDepositManager.Items.Refresh();
                    AccountList.Items.Refresh();
                    transfers.Items.Refresh();
                }
                else MessageBox.Show("не удалось совершить перевод");
            }
            else MessageBox.Show("не указана сумма");
        }

        /// <summary>
        /// снять деньги с депозита
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTakeMoneyFromDeposit_Click(object sender, RoutedEventArgs e)
        {
            float sum = 0;
            if (float.TryParse(amount2.Text, out sum))
            {
                if (user3.MGetMoneyFromDeposit(sum))
                {
                    accountListOfDepositManager.Items.Refresh();
                    AccountList.Items.Refresh();
                    transfers.Items.Refresh();
                }
                else MessageBox.Show("не удалось совершить перевод");
            }
            else MessageBox.Show("не указана сумма");
        }


        /// <summary>
        /// выбрать клиента для работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDepositManagerStart_Click(object sender, RoutedEventArgs e)
        {
            if (ClientList.SelectedItem == null)
                MessageBox.Show("клиент не выбран");
            else 
            {
                user3.TheClient = ClientList.SelectedItem as Client;
            }
        }

        /// <summary>
        /// закончить работу с клиентом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDepositManagerFinish_Click(object sender, RoutedEventArgs e)
        {
            accountListOfAccountManager.UnselectAll();
            user3.TheClient = null;
        }


        //ФУНКЦИОНАЛ МЕНЕДЖЕРА ПО РАБОТЕ С ДАННЫМИ КЛИЕНТОВ
 
        /// <summary>
        /// выбрать клиента для работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonManagerForNewClientStart_Click(object sender, RoutedEventArgs e)
        {
            if (ClientList.SelectedItem == null)
                MessageBox.Show("клиент не выбран");
            else
            {
                user1.TheClient = ClientList.SelectedItem as Client;
            }
        }

        /// <summary>
        /// закончить работу с клиентом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonManagerForNewClientFinish_Click(object sender, RoutedEventArgs e)
        {
            user1.TheClient = null;
        }

        /// <summary>
        /// окно для ввода данных о клиенте - организации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNewOrganisation_Click(object sender, RoutedEventArgs e)
        {    
            createOrganisationForm.Show();
        }

        /// <summary>
        /// окно для ввода данных о клиенте - физическом лице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNewPerson_Click(object sender, RoutedEventArgs e)
        {
            createPersonForm.Show();
        }



    }
}

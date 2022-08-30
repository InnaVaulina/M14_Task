using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M13_Library;
using System.Collections.ObjectModel;

namespace M14_Library
{
    /// <summary>
    /// журналы
    /// </summary>
    public class History
    {
        public ObservableCollection<Log1> transfers;
        public ObservableCollection<Log2> addClient;
        ManagerForNewClient manager1;
        AccountManager manager2;
        DepositManager manager3;
        Consultant consultant1;



        public History(
            ManagerForNewClient m1,
            AccountManager m2,
            DepositManager m3,
            Consultant c1)
        {
            this.manager1 = m1;
            this.manager2 = m2;
            this.manager3 = m3;
            this.consultant1 = c1;
            transfers = new ObservableCollection<Log1>();
            addClient = new ObservableCollection<Log2>();

            manager1.ClientChangesNotify += ClientCange2;
            manager1.ClientAddNotify += ClientCange2;
            manager2.ClientChangesNotify += ClientCange2;
            manager3.ClientChangesNotify += ClientCange2;
            consultant1.ClientChangesNotify += ClientCange2;

            manager2.AccountTransferNitify += TransferAdd;
            manager3.AccountTransferNitify += TransferAdd;

        }

        /// <summary>
        /// запись в журнале переводов
        /// </summary>
        /// <param name="manager">исполнитель</param>
        /// <param name="get">счет отправителя</param>
        /// <param name="put">счет получателя</param>
        /// <param name="amount">сумма</param>
        //public void TransferAdd(string manager, Account get, Account put, float amount)
        public void TransferAdd(User manager, AccountTransferEventArgs e)
        {
            string getType = "";
            string putType = "";
            if (e.From.GetType() != typeof(DepositAccount)) getType = "Account";
            else getType = "Deposit";
            if (e.To.GetType() != typeof(DepositAccount)) putType = "Account";
            else putType = "Deposit";
            //System.Windows.Application.Current.Dispatcher.Invoke(() =>
            //{
                transfers.Add(new Log1(
                    DateTime.Now,
                    manager.MName,
                    e.From.Client.ClientId,
                    e.From.Client.Name(),
                    getType,
                    e.From.AccountNumber,
                    e.To.Client.ClientId,
                    e.To.Client.Name(),
                    putType,
                    e.To.AccountNumber,
                    e.Sum));
            //}
            //);
        }



        public void ClientCange2(User manager, ClientChangesEventArgs e)
        {
            //System.Windows.Application.Current.Dispatcher.Invoke(() =>
            //{
                addClient.Add(new Log2(
                DateTime.Now,
                manager.MName,
                e.Client.ClientId,
                e.Client.Name(),
                e.PropertyName,
                e.PropertyValue
                ));
            //});
        }

    }


    /// <summary>
    /// журнал переводов
    /// </summary>
    public class Log1
    {
        DateTime time;
        string managerName;
        int getClientId;
        string getClientName;
        string getAccountType;
        string getAccount;
        int putClientId;
        string putClientName;
        string putAccountType;
        string putAccount;
        float amount;

        public Log1(
            DateTime time,
            string managerName,
            int getClientId,
            string getClientName,
            string getAccountType,
            string getAccount,
            int putClientId,
            string putClientName,
            string putAccountType,
            string putAccount,
            float amount
            )
        {
            this.time = time;
            this.managerName = managerName;
            this.getClientId = getClientId;
            this.getClientName = getClientName;
            this.getAccount = getAccount;
            this.getAccountType = getAccountType;
            this.putClientId = putClientId;
            this.putClientName = putClientName;
            this.putAccount = putAccount;
            this.putAccountType = putAccountType;
            this.amount = amount;

        }

        public string Time { get { return time.ToString(); } }
        public string ManagerName { get { return managerName; } }
        public int GetClientId { get { return getClientId; } }
        public string GetClientName { get { return getClientName; } }
        public string GetAccount { get { return getAccount; } }
        public string GetAccountType { get { return getAccountType; } }
        public int PutClientId { get { return putClientId; } }
        public string PutClientName { get { return putClientName; } }
        public string PutAccount { get { return putAccount; } }
        public string PutAccountType { get { return putAccountType; } }
        public float Ammount { get { return amount; } }

        public override string ToString()
        {
            return $"{time} {managerName,7} {getClientId,3} {getClientName,30} " +
                $"{getAccountType,7} {getAccount}" +
                $"{putClientId,5} {putClientName,30} " +
                $"{putAccountType,7} {putAccount}" +
                $"{amount,10}";
        }

    }

    /// <summary>
    /// журнал изменений данных о клиентах
    /// </summary>
    public class Log2
    {
        DateTime time;
        string managerName;
        int clientId;
        string clientName;
        string property;
        string propertyNewValue;

        public Log2(
            DateTime time,
            string managerName,
            int clientId,
            string clientName
            )
        {
            this.time = time;
            this.managerName = managerName;
            this.clientId = clientId;
            this.clientName = clientName;
            this.property = "";
            this.propertyNewValue = "";
        }

        public Log2(
            DateTime time,
            string managerName,
            int clientId,
            string clientName,
            string property,
            string propertyNewValue
            )
        {
            this.time = time;
            this.managerName = managerName;
            this.clientId = clientId;
            this.clientName = clientName;
            this.property = property;
            this.propertyNewValue = propertyNewValue;
        }

        public string Time { get { return time.ToString(); } }
        public string ManagerName { get { return managerName; } }
        public int ClientId { get { return clientId; } }
        public string ClientName { get { return clientName; } }
        public string Property { get { return property; } }
        public string PropertyNewValue { get { return propertyNewValue; } }
        public override string ToString()
        {
            return $"{time} {managerName,7} {clientId,3} {clientName}";
        }
    }
}

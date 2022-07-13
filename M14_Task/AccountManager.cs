using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using М13_Task1;

namespace M14_Task
{
    public class AccountManager : User
    {
 
        public event Action<string, Account, Account, float> ToLog;
        public new event Action<string, Client, string, string> ToLog3;

        Account accountInWork; 

        public AccountManager(string thisName, BankSystem bank) : 
            base(thisName, bank)
        { accountInWork = null; }

        /// <summary>
        /// клиент в работе
        /// </summary>
        public override Client TheClient 
        {
            get { return base.TheClient; }
            set 
            { 
                base.TheClient = value;
                accountInWork = null;
            }
        }

        /// <summary>
        /// счет в работе
        /// </summary>
        public Account AccountInWork 
        { 
            get { return accountInWork; }
            set 
            { 
                if(value.GetType() == typeof(Account))
                if(client != null)
                    foreach(Account account in client.Accounts)
                        if(account == value)
                            accountInWork = value; 
            }
        }

        /// <summary>
        /// касса банка
        /// </summary>
        public Account Cash 
        { 
            get { return bank.cash; }
            set 
            { 
                bank.cash = value;
                OnPropertyChanged("Cash");
            }
        }

        /// <summary>
        /// приветсвенное сообщение
        /// </summary>
        public override string HelloMessage()
        {
            return $"Здравствуйте! Меня зовут {MName}. " +
                "Я работаю со счетами. Я готов Вам помочь!";
        }


        /// <summary>
        /// открыть новый счет
        /// </summary>
        /// <returns>новый счет</returns>
        public Account MNewAccount()
        {
            Account x;
            if (client != null)
            {
                x = bank.NewAccount(ref client);
                ToLog3(MName, client, "AccountAdd", x.AccountNumber);               
                return x;
            }
            return null;
        }

        /// <summary>
        /// закрыть счет
        /// </summary>
        /// <returns></returns>
        public bool MCloseAccount()
        {

            if (accountInWork != null)
            {
                accountInWork.CloseAccount();
                ToLog3(MName, client, "AccountClose", accountInWork.AccountNumber);
                accountInWork = null;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// внести деньги в кассу
        /// </summary>
        /// <param name="sum">сумма</param>
        /// <returns></returns>
        public bool MPutMoneyToCash(float sum)
        {
            return Cash.PutMoney(bank.manageClient as IClient, sum);
        }

        /// <summary>
        /// забрать деньги из кассы
        /// </summary>
        /// <param name="sum">сумма</param>
        /// <returns></returns>
        public bool MTakeMoneyFromCash(float sum)
        {
            return Cash.GetMoney(bank.manageClient as IClient, sum);
        }

        /// <summary>
        /// перевести деньги из кассы на счет клиента
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        public bool MPutMoney(float sum)
        {
            if (accountInWork != null) 
            {
                bool x = bank.TransferContr(Cash, accountInWork, sum);
                if (x == true) ToLog(MName, Cash, accountInWork, sum);
                return x;
            }
            return false;            
        }

        /// <summary>
        /// перевести деньги со счета клиента в кассу банка
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        public bool MTakeMoney(float sum)
        {
            if (accountInWork != null) 
            {
                bool x = bank.TransferContr(accountInWork, Cash, sum);
                if (x == true) ToLog(MName, accountInWork, Cash, sum);
                return x;
            }
            return false;   
        }

       
        /// <summary>
        /// перевод между счетами разных клиентов
        /// </summary>
        /// <param name="get"></param>
        /// <param name="put"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public bool MTransfer(Account get, Account put, float sum)
        {
            bool x = bank.TransferContr(get, put, sum);
            if (x == true) ToLog(MName, get, put, sum);
            return x;
        }

        /// <summary>
        /// открыть счет для нового клиента
        /// </summary>
        /// <param name="newClient"></param>
        public void OnNewClientAdd(Client newClient) 
        {
            Client x = this.TheClient;
            this.TheClient = newClient;
            this.MNewAccount();
            this.TheClient = x;

        }

       

    }
}

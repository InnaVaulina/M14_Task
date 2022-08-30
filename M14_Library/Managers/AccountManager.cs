using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M13_Library;

namespace M14_Library
{
    public class AccountManager : User
    {

        public new event ClientChangesHendler ClientChangesNotify;
        public event AccountTransferHandler AccountTransferNitify;

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
                AccountInWork = null;
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
                if (value == null) accountInWork = value;
                else
                if (value.GetType() == typeof(Account))
                    //      if(client != null)
                    foreach (Account account in client.Accounts)
                        if (account == value)
                            accountInWork = value;
                OnPropertyChanged("AccountInWork");
            }
        }

        /// <summary>
        /// касса банка
        /// </summary>
        public Account Cash
        {
            get { return bank.Cash; }
            set
            {
                bank.Cash = value;
                OnPropertyChanged("Cash");
            }
        }

        public Dictionary<string, Account> Accounts { get { return bank.accounts; } }

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
                ClientChangesNotify?.Invoke(this, new ClientChangesEventArgs(client, "AccountAdd", x.AccountNumber));
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
                ClientChangesNotify?.Invoke(this, new ClientChangesEventArgs(client, "AccountClose", accountInWork.AccountNumber));
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
            bool x = Cash.PutMoney(bank.manageClient as IClient, sum);
            OnPropertyChanged("Cash");
            return x;
        }

        /// <summary>
        /// забрать деньги из кассы
        /// </summary>
        /// <param name="sum">сумма</param>
        /// <returns></returns>
        public bool MTakeMoneyFromCash(float sum)
        {
            bool x = Cash.GetMoney(bank.manageClient as IClient, sum);
            OnPropertyChanged("Cash");
            return x;
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
                if (x == true)
                {
                    AccountTransferNitify?.Invoke(this,
                        new AccountTransferEventArgs(Cash, accountInWork, sum));
                }

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
                if (x == true) AccountTransferNitify?.Invoke(this,
                                  new AccountTransferEventArgs(accountInWork, Cash, sum));
                return x;
            }
            return false;
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

        public void OnNewClientAdd2(User user, ClientChangesEventArgs e)
        {
            User y = user;
            y.TheClient = null;
            Client x = this.TheClient;
            this.TheClient = e.Client;
            this.MNewAccount();
            this.TheClient = x;
            y.TheClient = e.Client;

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
            if (x == true) AccountTransferNitify?.Invoke(this,
                                  new AccountTransferEventArgs(get, put, sum));
            return x;
        }


        public bool MTransfer(Account get, string putNumber, float sum)
        {
            Account put = null;
            try
            {
                put = bank.accounts[putNumber];
            }
            catch
            {
                throw new TransferExeption("Не верный счет получателя.");
            }
            if (put != null) return MTransfer(get, put, sum);
            else return false;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using М13_Task1;

namespace M14_Task
{
    public class DepositManager : User
    {

        public event Action<string, Account, Account, float> ToLog;
        public new event Action<string, Client, string, string> ToLog3;

        Account accountInWork;
        DepositAccount depositInWork;

        public DepositManager(string thisName, BankSystem bank) : 
            base(thisName, bank)
        { }

        /// <summary>
        /// приветсвенное сообщение
        /// </summary>
        /// <returns></returns>
        public override string HelloMessage()
        {
            return $"Здравствуйте! Меня зовут {MName}. " +
                "Я работаю с депозитными счетами. Я готов Вам помочь!";
        }

        /// <summary>
        /// счет в работе
        /// </summary>
        public Account AccountInWork
        {
            get { return accountInWork; }
            set
            {
                if (value.GetType() == typeof(Account))
                    if (client != null)
                        foreach (Account account in client.Accounts)
                            if (account == value)
                                accountInWork = value;
            }
        }

        /// <summary>
        /// депозит в работе
        /// </summary>
        public DepositAccount DepositInWork
        {
            get { return depositInWork; }
            set
            {
                if (value.GetType() == typeof(DepositAccount))
                    if (client != null)
                        foreach (Account account in client.Accounts)
                            if (account == value)
                                depositInWork = value;
            }
        }

        /// <summary>
        /// открыть депозит
        /// </summary>
        /// <returns></returns>
        public DepositAccount MNewDepositAccount()
        {
            DepositAccount x;
            if (client != null)
            {
                x = bank.NewDepositAccount(ref client);
                ToLog3(MName, client, "DepositAdd", x.AccountNumber);
                return x;
            }
            return null;
        }

        /// <summary>
        /// закрыть депозит
        /// </summary>
        /// <returns></returns>
        public bool MCloseDeposit()
        {
            if (depositInWork != null) 
            {
                if (depositInWork.CloseAccount())
                {
                    ToLog3(MName, client, "DepositClose", depositInWork.AccountNumber);
                    depositInWork = null;
                    return true;
                }
                else return false;         
            }       
            else return false;
        }

        /// <summary>
        /// перевод денег со счета клиента на депозит клиента
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        public bool MPutMoneyToDeposit(float sum)
        {
            if (accountInWork != null && depositInWork != null)
            {
                bool x = bank.TransferContr(accountInWork, depositInWork, sum);
                if (x == true) ToLog(MName, accountInWork, depositInWork, sum);
                return x;
            }
            else return false;
        }


        /// <summary>
        /// перевод денег с депозита клиента на счет клиента
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        public bool MGetMoneyFromDeposit(float sum)
        {
            if (accountInWork != null && depositInWork != null)
            {
                bool x = bank.TransferContr(depositInWork, accountInWork, sum);
                if (x == true) ToLog(MName, depositInWork, accountInWork, sum);
                return x;
            }
            else return false;   
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using М13_Task1;

namespace M14_Task
{
    public class ManagerForNewClient : User
    {
        public event Action<string, Client> ToLog2;
        public event Action<Client> Create;


        public ManagerForNewClient(string thisName, BankSystem bank) : 
            base(thisName, bank) { }

        /// <summary>
        /// приветсвенное сообщение
        /// </summary>
        /// <returns></returns>
        public override string HelloMessage() 
        {
            return $"Здравствуйте! Меня зовут {MName}. " +
                "Я работаю с новыми клиентами. Буду рад Вам помочь!";
        }

        /// <summary>
        /// создание нового клиента - физическое лицо
        /// </summary>
        /// <param name="familyName"></param>
        /// <param name="firstName"></param>
        /// <param name="patronymicName"></param>
        public void MFNewPersonClient(
            string familyName,
            string firstName,
            string patronymicName)
        {
            TheClient = bank.NewPersonClient(familyName, firstName, patronymicName);
            ToLog2(this.MName, this.TheClient);
            Create(TheClient);
        }


        /// <summary>
        /// создание нового клиента - юридическое лицо
        /// </summary>
        /// <param name="name"></param>
        /// <param name="inn"></param>
        /// <param name="representative"></param>
        public void MFNewOrganisationClient(
            string name,
            string inn,
            string representative)
        {
            TheClient = bank.NewOrganisationClient(name, inn, representative);
            ToLog2(this.MName, this.TheClient);
            Create(TheClient);
        }
    }
}

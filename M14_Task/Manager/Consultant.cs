using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using М13_Task1;

namespace M14_Task
{
    public class Consultant : User
    {
        public Consultant(string thisName, BankSystem bank) : 
            base(thisName, bank) 
        { }

        /// <summary>
        /// приветственное сообщение
        /// </summary>
        /// <returns></returns>
        public override string HelloMessage()
        {
            return $"Здравствуйте! Меня зовут {MName}. " +
                "Я консультант. Буду рад Вам помочь!";
        }
        
    }
}

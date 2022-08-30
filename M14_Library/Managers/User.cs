using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using M13_Library;

namespace M14_Library
{
    public class User : INotifyPropertyChanged
    {
        public string MName;
        protected BankSystem bank;
        protected Client client;


        public event ClientChangesHendler ClientChangesNotify;



        /// <summary>
        /// клиент
        /// </summary>
        public virtual Client TheClient
        {
            get { return client; }
            set
            {
                if (client != value && client != null)
                    client.PropertyChange -= ClientPropertyChange;
                if (value != null)
                {
                    client = value;
                    client.PropertyChange += ClientPropertyChange;
                }
                else client = null;
                OnPropertyChanged("TheClient");
            }

        }

        /// <summary>
        /// исполнитель
        /// </summary>
        /// <param name="thisName"></param>
        /// <param name="bank"></param>
        public User(string thisName, BankSystem bank)
        {
            this.MName = thisName;
            this.bank = bank;
            client = default(Client);
        }

        /// <summary>
        /// имя исполнителя
        /// </summary>
        public string Name { get { return this.MName; } }



        /// <summary>
        /// приветсвенное сообщение
        /// </summary>
        /// <returns></returns>
        public virtual string HelloMessage() { return "Здравствуйте!"; }



        /// <summary>
        /// событие - изменились сведения о клиенте
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void ClientPropertyChange(string propertyName, string value)
        {
            ClientChangesNotify?.Invoke(this, new ClientChangesEventArgs(client, propertyName, value));
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}

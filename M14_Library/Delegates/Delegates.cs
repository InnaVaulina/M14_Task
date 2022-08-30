using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M13_Library;

namespace M14_Library
{
    public class AccountTransferEventArgs
    {
        Account from;
        Account to;
        float sum;

        public Account From { get { return from; } }
        public Account To { get { return to; } }
        public float Sum { get { return sum; } }

        public AccountTransferEventArgs(
            Account from, Account to, float sum)
        {
            this.from = from; this.to = to; this.sum = sum;
        }

    }
    public class ClientChangesEventArgs
    {
        Client client;
        string propertyName;
        string propertyValue;
        public Client Client { get { return client; } }
        public string PropertyName { get { return propertyName; } }
        public string PropertyValue { get { return propertyValue; } }

        public ClientChangesEventArgs(
            Client client, string propertyName, string propertyValue)
        {
            this.client = client;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
        }

    }

    public delegate void ClientChangesHendler(User user, ClientChangesEventArgs e);
    public delegate void AccountTransferHandler(User user, AccountTransferEventArgs e);

}

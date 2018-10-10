using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business;

namespace Data
{
    /*
     * Author name: Andrea Silvestro Ortino
     * Data.CustomerDict is a class which hold a customer dictionary and a hashset of chalets ids to be stored by the SingletonStorage class in a binary file.
     * Date last modified: 22/11/2017 16:46
     */ 

    [Serializable]
    public class CustomerDict
    {
        private SortedDictionary<int, Customer> _storeCustomers = new SortedDictionary<int, Customer>();
        private HashSet<int> _chaletsBooked = new HashSet<int>();

        // StoreCustomers property with get method to return the corrispective private dictionary.
        public SortedDictionary <int, Customer> StoreCustomers
        {
            get
            {
                return _storeCustomers;
            }
        }

        // ChaletsBooked property with get/set methods to return and/or set the corrispective private attribute (chalet id booked by the system).
        public HashSet<int> ChaletsBooked
        {
            get
            {
                return _chaletsBooked;
            }
            set
            {
                value = _chaletsBooked;
            }
        }

        // This method add to the _storeCustomers dictionary the Customer's CustRefNo property as a key and a Customer object as a value.
        public void addCustomer(Customer dictCustomer)
        {
            StoreCustomers.Add(dictCustomer.CustRefNo, dictCustomer);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /*
     * Author name: Andrea Silvestro Ortino
     * Business.Customer is a class which helds customer properties/attributes and methods to add bookings to a Dictionary
     * Date last modified: 22/11/2017 16:39
     */ 

    [Serializable]
    public class Customer
    {
        private string _name;
        private string _address;
        private int _custRefNo;
        private SortedDictionary<int,Booking> _dictBookings = new SortedDictionary<int,Booking>();

        // Customer constructor which takes custRefNo, name and address as a parameters in order to instantiate an object.
        public Customer(int custRefNo, string name, string address)
        {
            this._custRefNo = custRefNo;
            this.Name = name;
            this.Address = address;
        }

        // Name property with get/set method (with proper validation check) to return and/or set the corrispective private attribute.
        public string Name 
        {
            get 
            {
                return this._name;
            }

            set
            {
                if (value.Length == 0)
                {
                    throw new ArgumentException("Insert a name.");
                }
                this._name = value;
            }
        }

        // Address property with get/set method (with proper validation check) to return and/or set the corrispective private attribute.
        public string Address 
        {
            get
            {
                return this._address;
            }

            set
            {
                if (value.Length == 0)
                {
                    throw new ArgumentException("Insert an address.");
                }
                this._address = value;
            }  
        }

        // CustRefNo property with get method to return the corrispective private attribute.
        public int CustRefNo
        {
            get
            {
                return this._custRefNo;
            }
        }

        // DictBookings property with get method to return the corrispective private dictionary.
        public SortedDictionary<int,Booking> DictBookings
        {
            get
            {
                return this._dictBookings;
            }
        }

        // This method add to the _dictBookings dictionary the Booking's BookingRefNo property as a key and a Booking object as a value.
        public void addBooking(Booking booking)
        {
            _dictBookings.Add(booking.BookingRefNo, booking);
        }

        // This method override the toString method in order to print a customer object directly with the return of this method.
        public override string ToString()
        {
            return this.CustRefNo.ToString() + " - " + this.Name + ", " + this.Address + ".";
        }
    }
}

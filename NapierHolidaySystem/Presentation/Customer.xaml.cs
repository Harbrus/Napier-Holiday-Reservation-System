using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Business;
using Data;

namespace Presentation
{
    /*
    * Author name: Andrea Silvestro Ortino
    * Presentation.Customer is a WPF used for creating/editing/delete a customer and bookings assigned to it. It also show invoice with related cost of the selected booking.
    * Date last modified: 23/11/2017 09:51
    * This class makes use of the SingletonStorage class instance (Singleton Design Patter).
    */ 

    public partial class Customer : Window
    {
        SingletonStorage data = SingletonStorage.Instance;
        private int _custID;

        // This constructor initializes the window for a new customer, giving an unused customer reference number, and displaying information on the forms.
        public Customer(SingletonStorage data)
        {
            InitializeComponent();

            if (data.CustDict.StoreCustomers.Count == 0)
            {
                _custID = 1;
            }
            else
            {
                _custID = GetFirstUnusedKey(data.CustDict.StoreCustomers);
            }
            
            txbCustRefNo.Text = _custID.ToString();
        }

        // This constructor initializes the window for dysplaing/editing an existing customer's details and booking assigned to it.
        public Customer(SingletonStorage data, int custRefNo)
        {
            InitializeComponent();
            _custID = custRefNo;
            txbCustName.Text = data.CustDict.StoreCustomers[custRefNo].Name;
            txbCustAddress.Text = data.CustDict.StoreCustomers[custRefNo].Address;
            txbCustRefNo.Text = custRefNo.ToString();
            populateListBox(data, custRefNo);
        }

        // This method is called to iterate the customer dictionary in order to find the lowest key (customer reference number) available.
        public static int GetFirstUnusedKey(SortedDictionary<int, Business.Customer> dict)
        {
            using (var count = dict.GetEnumerator())
            {
                if (!count.MoveNext())
                    return 1;

                var nextKeyInSequence = count.Current.Key + 1;

                if (nextKeyInSequence < 2)
                    throw new InvalidOperationException("The dictionary contains keys less than 1");

                if (nextKeyInSequence != 2)
                    return 1;

                while (count.MoveNext())
                {
                    var key = count.Current.Key;
                    if (key > nextKeyInSequence)
                        return nextKeyInSequence;

                    ++nextKeyInSequence;
                }

                return nextKeyInSequence;
            }
        }

        // This method populates a booking list box with Booking bookingRefNo values.
        public void populateListBox(SingletonStorage data, int custRefNo)
        {
            lbxBookingList.Items.Clear();

            foreach (int bookingRefNo in data.CustDict.StoreCustomers[custRefNo].DictBookings.Keys)
            {
                if (bookingRefNo != 0)
                {
                    lbxBookingList.Items.Add(bookingRefNo);
                }
            }
        }

        // This method is called on SAVE button click. It saves and update the binary files with customer's details.
        private void btnSaveCust_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (data.CustDict.StoreCustomers.ContainsKey(_custID))
                {
                    data.CustDict.StoreCustomers[_custID].Name = txbCustName.Text;
                    data.CustDict.StoreCustomers[_custID].Address = txbCustAddress.Text;
                }
                else
                {
                    Business.Customer newCust = new Business.Customer(_custID, txbCustName.Text, txbCustAddress.Text);
                    data.CustDict.addCustomer(newCust);
                }
            }
            catch(Exception excep)
            {
                MessageBox.Show(excep.Message);
            }

            data.updateFile();
        }

        // This method is called on DELETE button click. It deletes the customer from the data only if it has no active bookings.
        private void btnDeleteCust_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (data.CustDict.StoreCustomers[_custID].DictBookings.Count == 0)
                {
                    data.CustDict.StoreCustomers.Remove(_custID);
                    data.updateFile();
                    txbCustName.Text = null;
                    txbCustAddress.Text = null;
                }
                else
                    throw new ArgumentException("Customers with active bookings cannot be deleted. Please, delete any active bookings first.");
            }
            catch(Exception excep)
            {
                MessageBox.Show(excep.Message);
            }
        }

        // This method is called on NEW BOOKING button click. It saves the customer and open a Booking page window passing the customer reference number, and closing this window.
        private void btnNewBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(txbCustName.Text.Length == 0 || txbCustName.Text.Length == 0)
                {
                    throw new ArgumentException("Register Customer's details first.");
                }
                btnSaveCust_Click(sender, e);
                Booking bookingPage = new Booking(data, _custID);
                bookingPage.Show();
                this.Close();
            }
            catch(Exception excep)
            {
                MessageBox.Show(excep.Message);
            }
        }

        // This method is called on EDIT button click. It saves the customer and open a Booking page window passing the customer reference number, and the booking reference number
        // of the booking to be edited.
        private void btnEditBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!lbxBookingList.HasItems)
                {
                    throw new ArgumentException("Cannot edit a booking because you have not any active bookings.");
                } 

                int bookingRefNo = Int32.Parse(lbxBookingList.SelectedItem.ToString());
                int custRefNo = Int32.Parse(txbCustRefNo.Text);

                if (!data.CustDict.StoreCustomers[custRefNo].DictBookings.ContainsKey(bookingRefNo))
                {
                    throw new ArgumentException("Error: Booking Reference Number not in the data.");
                }

                btnSaveCust_Click(sender, e);
                Booking bookingPage = new Booking(data, custRefNo, bookingRefNo);
                bookingPage.Show();
                this.Close();
                
            }
            catch (Exception excep)
            {
                MessageBox.Show(excep.Message);
            }
        }

        // This method is called on DELETE button click. It deletes from the booking list box and from the data the selected booking using the booking reference number. 
        // Then updates the data.
        private void btnDeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!lbxBookingList.HasItems)
                {
                    throw new ArgumentException("Cannot delete a booking because you have not any active bookings.");
                }

                int custID = Int32.Parse(txbCustRefNo.Text);
                int bookingRefNo = Int32.Parse(lbxBookingList.SelectedItem.ToString());

            
                if (!data.CustDict.StoreCustomers[custID].DictBookings.ContainsKey(bookingRefNo))
                {
                    throw new ArgumentException("Error: Booking Reference Number not in the data.");
                }

                data.CustDict.ChaletsBooked.Remove(data.CustDict.StoreCustomers[custID].DictBookings[bookingRefNo].ChaletID);
                data.CustDict.StoreCustomers[custID].DictBookings.Remove(bookingRefNo);
                data.updateFile();

                if (data.CustDict.StoreCustomers[custID].DictBookings.ContainsKey(bookingRefNo))
                {
                    throw new ArgumentException("Error: Booking not deleted.");
                }

                populateListBox(data, custID);
            }
            catch (Exception excep)
            {
                MessageBox.Show(excep.Message);
            }
        }

        // This method is called on INVOICE button click. It shows an invoice with cost details of the selected booking.
        private void btnInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 if (!lbxBookingList.HasItems)
                 {
                    throw new ArgumentException("Cannot show an invoice because you have not any active bookings.");
                 }

                 if(lbxBookingList.SelectedIndex != -1)
                 {
                     int custID = Int32.Parse(txbCustRefNo.Text);
                     int bookingRefNo = Int32.Parse(lbxBookingList.SelectedItem.ToString());
                     MessageBox.Show(data.CustDict.StoreCustomers[custID] + "\n" + data.CustDict.StoreCustomers[custID].DictBookings[bookingRefNo] + "\n" + data.CustDict.StoreCustomers[custID].DictBookings[bookingRefNo].getCost() 
                         + "\n" + data.CustDict.StoreCustomers[custID].DictBookings[bookingRefNo].getTotalCost());
                 }                 
            }
            catch(Exception excep)
            {
                MessageBox.Show(excep.Message);
            }
        }

        // This method ensure that the selection in the listbox is unselected while operating in other wpf controls.
        private void lbxBookingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxBookingList.SelectedIndex == -1)
            {
                return;
            }
        }

    }
}

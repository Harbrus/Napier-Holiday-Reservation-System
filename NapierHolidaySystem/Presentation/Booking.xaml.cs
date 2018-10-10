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
using Data;
using Business;

namespace Presentation
{
    /*
    * Author name: Andrea Silvestro Ortino
    * Presentation.Booking is a WPF used for creating/editing/deleting booking, extra options and guests. 
    * Date last modified: 23/11/2017 09:16
    * This class makes use of the SingletonStorage class instance (Singleton Design Patter).
    */
    public partial class Booking : Window
    {
        private SingletonStorage data = SingletonStorage.Instance;
        private int _bookingID;
        private int _custID;
        private Business.Booking _newBooking;
        private SortedDictionary<int,Business.Booking> _tempBookingDict = new SortedDictionary<int,Business.Booking>();
        private SortedDictionary<string, Guest> _tempGuestDict = new SortedDictionary<string, Guest>();
        private int tempChaletID;
        string oldPassportNo;

        // This constructor initializes the window for a new booking assigned to the current customer, giving an unused booking reference number, and displaying information on the forms.
        public Booking(SingletonStorage data, int custRefNo)
        {
            InitializeComponent();

            _custID = custRefNo;

            foreach(KeyValuePair<int, Business.Customer> entry in data.CustDict.StoreCustomers)
            {
                foreach(KeyValuePair<int, Business.Booking> bookingEntry in entry.Value.DictBookings)
                    _tempBookingDict.Add(bookingEntry.Key, bookingEntry.Value);
            }

            if(_tempBookingDict.Count == 0)
            {
                _bookingID = 1;
            }
            else
                _bookingID = GetFirstUnusedKey(_tempBookingDict);

            tbxBookingRefNo.Text = _bookingID.ToString();
        }

        // This constructor initializes the window for editing an existing booking assigned to the current customer, and displaying information on the forms.
        public Booking(SingletonStorage data, int custRefNo, int bookingRefNo)
        {
            InitializeComponent();

            _custID = custRefNo;
            _bookingID = bookingRefNo;
            tbxBookingRefNo.Text = bookingRefNo.ToString();
            clrArrival.SelectedDate = data.CustDict.StoreCustomers[custRefNo].DictBookings[bookingRefNo].ArrivalDate;
            clrDeparture.SelectedDate = data.CustDict.StoreCustomers[custRefNo].DictBookings[bookingRefNo].DepartureDate;
            tbxChaletID.Text = data.CustDict.StoreCustomers[custRefNo].DictBookings[bookingRefNo].ChaletID.ToString();
            _tempGuestDict.Clear();
            _tempGuestDict = data.CustDict.StoreCustomers[custRefNo].DictBookings[bookingRefNo].GuestDict;
            populateGuestListBox(data, custRefNo, bookingRefNo);
            
            if (data.CustDict.StoreCustomers[custRefNo].DictBookings[bookingRefNo].ExtraDict.ContainsKey("Breakfast"))
            {
                ckbBreakfast.IsChecked = true;
            }

            if (data.CustDict.StoreCustomers[custRefNo].DictBookings[bookingRefNo].ExtraDict.ContainsKey("Evening Meal"))
            {
                ckbDinner.IsChecked = true;
            }

            if (data.CustDict.StoreCustomers[custRefNo].DictBookings[bookingRefNo].ExtraDict.ContainsKey("Car Hire"))
            {
                ckbCarHire.IsChecked = true;
                clrStartHire.SelectedDate = data.CustDict.StoreCustomers[custRefNo].DictBookings[bookingRefNo].ExtraDict["Car Hire"].StartHire;
                clrEndHire.SelectedDate = data.CustDict.StoreCustomers[custRefNo].DictBookings[bookingRefNo].ExtraDict["Car Hire"].EndHire;
                tbxDriver.Text = data.CustDict.StoreCustomers[custRefNo].DictBookings[bookingRefNo].ExtraDict["Car Hire"].DriverName;
            }
            
        }

        // This method is called to iterate the booking dictionary in order to find the lowest key (bookingRefNo) available.
        public static int GetFirstUnusedKey(SortedDictionary<int, Business.Booking> dict)
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

        // This method populates a guest list box with Guest passportNo values.
        public void populateGuestListBox(SingletonStorage data, int custRefNo, int bookingRefNo)
        {
            lbxGuests.Items.Clear();

            if (data.CustDict.StoreCustomers[custRefNo].DictBookings.ContainsKey(bookingRefNo) && data.CustDict.StoreCustomers[custRefNo].DictBookings[bookingRefNo].GuestDict.Count != 0)
            {
                foreach (string passportNo in data.CustDict.StoreCustomers[custRefNo].DictBookings[bookingRefNo].GuestDict.Keys)
                {
                    if (passportNo != null)
                    {
                        lbxGuests.Items.Add(passportNo);
                    }
                }
            }
            else if(_tempGuestDict.Count != 0)
            {
                foreach (string passportNo in _tempGuestDict.Keys)
                {
                    if (passportNo != null)
                    {
                        lbxGuests.Items.Add(passportNo);
                    }
                }
            }

        }

        // This method display information on guest's textboxes according to the current selection on the guest list box.
        private void lbxGuests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxGuests.SelectedIndex == -1)
            {
                return;
            }
            
            tbxGuestName.Text = _tempGuestDict[lbxGuests.SelectedItem.ToString()].Name;
            tbxPassportNo.Text = lbxGuests.SelectedItem.ToString();
            tbxAge.Text = _tempGuestDict[lbxGuests.SelectedItem.ToString()].Age.ToString();
        }

        // This method retain the last passport number in the textbox before is being edited by the user. 
        private void tbxPassportNo_GotFocus_1(object sender, RoutedEventArgs e)
        {
            oldPassportNo = tbxPassportNo.Text;
        }

        // This method is called by clicking the ADD button. It creates a new Guest object and then add it to the appropriate dictionary and call populateGuestListBox.
        private void btnAddGuest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (data.CustDict.StoreCustomers[_custID].DictBookings.ContainsKey(_bookingID))
                {
                    if (data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].GuestDict.ContainsKey(tbxPassportNo.Text) || _tempGuestDict.ContainsKey(tbxPassportNo.Text))
                    {
                        throw new ArgumentException("Please, insert a valid passport number.");
                    }
                    Guest newGuest = new Guest(tbxGuestName.Text, tbxPassportNo.Text, Int32.Parse(tbxAge.Text.ToString()));
                    data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].addGuest(newGuest);
                }
                else
                {
                    if(_tempGuestDict.ContainsKey(tbxPassportNo.Text))
                    {
                        throw new ArgumentException("Please, insert a valid passport number.");
                    }
                    Guest newGuest = new Guest(tbxGuestName.Text, tbxPassportNo.Text, Int32.Parse(tbxAge.Text.ToString()));
                    _tempGuestDict.Add(newGuest.PassportNo, newGuest);
                }

            }
            catch(Exception excep)
            {
                MessageBox.Show(excep.Message);
            }
            
            populateGuestListBox(data, _custID, _bookingID);
        }

        // This method is called on EDIT button click. It checks for existing Guest in the dictionary and make appropriate editing.
        private void btnEditGuest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (data.CustDict.StoreCustomers[_custID].DictBookings.ContainsKey(_bookingID))
                {
                    data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].GuestDict.Remove(oldPassportNo);
                    _tempGuestDict.Remove(oldPassportNo);

                    if (data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].GuestDict.ContainsKey(tbxPassportNo.Text))
                    {
                        throw new ArgumentException("Please, insert a valid passport number.");
                    }
                    Guest editGuest = new Guest(tbxGuestName.Text, tbxPassportNo.Text, Int32.Parse(tbxAge.Text.ToString()));

                    data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].addGuest(editGuest);
                }
                else
                {
                    _tempGuestDict.Remove(oldPassportNo);

                    if (_tempGuestDict.ContainsKey(tbxPassportNo.Text))
                    {
                        throw new ArgumentException("Please, insert a valid passport number.");
                    }

                    Guest editGuest = new Guest(tbxGuestName.Text, tbxPassportNo.Text, Int32.Parse(tbxAge.Text.ToString()));

                    _tempGuestDict.Add(editGuest.PassportNo, editGuest);
                }
            }
            catch(Exception excep)
            {
                MessageBox.Show(excep.Message);
            }

            tbxGuestName.Text = null;
            tbxPassportNo.Text = null;
            tbxAge.Text = null;
            populateGuestListBox(data, _custID, _bookingID);

        }

        // This method is called on DELETE button click. It checks for existing Guests in the dictionary and delete it using the passport number. Then updates listbox, data and dictionaries.
        private void btnDeleteGuest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(data.CustDict.StoreCustomers[_custID].DictBookings.ContainsKey(_bookingID))
                {
                    if (!data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].GuestDict.ContainsKey(tbxPassportNo.Text))
                    {
                        throw new ArgumentException("Error: Guest not in the data.");
                    }

                    data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].GuestDict.Remove(tbxPassportNo.Text);
                    _tempGuestDict.Remove(tbxPassportNo.Text);
                
                    if (data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].GuestDict.ContainsKey(tbxPassportNo.Text) || _tempGuestDict.ContainsKey(tbxPassportNo.Text))
                    {
                        throw new ArgumentException("Error: Guest not deleted.");
                    }
                }
                
                else
                {
                    if (!_tempGuestDict.ContainsKey(tbxPassportNo.Text))
                    {
                        throw new ArgumentException("Error: Guest not in the data.");
                    }

                    _tempGuestDict.Remove(tbxPassportNo.Text);

                    if(_tempGuestDict.ContainsKey(tbxPassportNo.Text))
                    {
                        throw new ArgumentException("Error: Guest not deleted.");
                    }
                }

                tbxGuestName.Text = null;
                tbxPassportNo.Text = null;
                tbxAge.Text = null;
                populateGuestListBox(data, _custID, _bookingID);
            }
            catch (Exception excep)
            {
                MessageBox.Show(excep.Message);
            }
        }

        // This method is called on SAVE button click. It saves all the information about the booking, any extras, and guest list and updates the binary file. Appropriate
        // checks are done to see if data need to be edited or it is a new booking.
        private void btnSaveBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExtraSelector extraType = new NoExtra();
                ExtraSelector breakfast = new Breakfast();
                ExtraSelector eveningMeal = new EveningMeal();
                ExtraSelector carHire = new CarHire();

                if(data.CustDict.StoreCustomers[_custID].DictBookings.ContainsKey(_bookingID))
                {
                    if(clrArrival.SelectedDate == null || clrDeparture.SelectedDate == null)
                    {
                        throw new ArgumentException("Please select an Arrival Date and a Departure Date");
                    }

                    data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ArrivalDate = Convert.ToDateTime(clrArrival.SelectedDate);
                    data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].DepartureDate = Convert.ToDateTime(clrDeparture.SelectedDate);

                    if (data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ChaletID == Int32.Parse(tbxChaletID.Text))
                    {
                        data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ChaletID = Int32.Parse(tbxChaletID.Text);
                    }

                    if(!data.CustDict.ChaletsBooked.Contains(Int32.Parse(tbxChaletID.Text))) 
                    {
                        data.CustDict.ChaletsBooked.Remove(data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ChaletID);
                        data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ChaletID = Int32.Parse(tbxChaletID.Text);
                        data.CustDict.ChaletsBooked.Add(Int32.Parse(tbxChaletID.Text));
                    }
                    else if (data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ChaletID != Int32.Parse(tbxChaletID.Text) && data.CustDict.ChaletsBooked.Contains(Int32.Parse(tbxChaletID.Text)))
                    {
                        throw new ArgumentException("Chalet already booked, please choose another chalet.");
                    }

                    if(ckbBreakfast.IsChecked == false && data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ExtraDict.ContainsKey("Breakfast"))
                    {
                        data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ExtraDict.Remove("Breakfast");
                    }
                    else if (ckbBreakfast.IsChecked == true && !data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ExtraDict.ContainsKey("Breakfast"))
                    {
                        Extra extra = new Extra("Breakfast");
                        extraType.SetExtra(breakfast);
                        breakfast.ProcessExtra(extra);
                        data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].addExtra(extra);
                    }

                    if (ckbDinner.IsChecked == false && data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ExtraDict.ContainsKey("Evening Meal"))
                    {
                        data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ExtraDict.Remove("Evening Meal");
                    }
                    else if (ckbDinner.IsChecked == true && !data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ExtraDict.ContainsKey("Evening Meal"))
                    {
                        Extra extra = new Extra("Evening Meal");
                        extraType.SetExtra(eveningMeal);
                        eveningMeal.ProcessExtra(extra);
                        data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].addExtra(extra);
                    }

                    if (ckbCarHire.IsChecked == false && data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ExtraDict.ContainsKey("Car Hire"))
                    {
                        data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ExtraDict.Remove("Car Hire");
                        tbxDriver.Text = null;
                        clrStartHire.SelectedDate = null;
                        clrEndHire.SelectedDate = null;
                    }
                    else if (ckbCarHire.IsChecked == true && !data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ExtraDict.ContainsKey("Car Hire"))
                    {
                        if (clrStartHire.SelectedDate == null || clrEndHire.SelectedDate == null || tbxDriver.Text == null)
                        {
                            throw new ArgumentException("Please select a start hire date, an end hire date, and a driver name.");
                        }

                        if(clrStartHire.SelectedDate < clrArrival.SelectedDate || clrEndHire.SelectedDate > clrDeparture.SelectedDate )
                        {
                            throw new ArgumentException("Invalid hiring dates. Please, insert dates within your booking period of staying.");
                        }

                        Extra extra = new Extra("Car Hire");
                        extraType.SetExtra(carHire);
                        carHire.ProcessExtra(extra);
                        extra.DriverName = tbxDriver.Text;
                        extra.StartHire = Convert.ToDateTime(clrStartHire.SelectedDate);
                        extra.EndHire = Convert.ToDateTime(clrEndHire.SelectedDate); 
                        data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].addExtra(extra);
                    }
                    else if(ckbCarHire.IsChecked == true && data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ExtraDict.ContainsKey("Car Hire"))
                    {
                        if (clrStartHire.SelectedDate < clrArrival.SelectedDate || clrEndHire.SelectedDate > clrDeparture.SelectedDate)
                        {
                            throw new ArgumentException("Invalid hiring dates. Please, insert dates within your booking period of staying.");
                        }

                        data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ExtraDict["Car Hire"].DriverName = tbxDriver.Text;
                        data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ExtraDict["Car Hire"].EndHire = Convert.ToDateTime(clrEndHire.SelectedDate); 
                        data.CustDict.StoreCustomers[_custID].DictBookings[_bookingID].ExtraDict["Car Hire"].StartHire = Convert.ToDateTime(clrStartHire.SelectedDate);
                        
                    }
                }

                else
                {
                    if (clrArrival.SelectedDate == null || clrDeparture.SelectedDate == null)
                    {
                        throw new ArgumentException("Please selecet an Arrival Date and a Departure Date");
                    }

                    _newBooking = new Business.Booking(_bookingID, Convert.ToDateTime(clrArrival.SelectedDate), Convert.ToDateTime(clrDeparture.SelectedDate));

                    foreach (KeyValuePair<string,Guest> guest in _tempGuestDict)
                    {
                        _newBooking.addGuest(guest.Value);
                    }

                    if(ckbBreakfast.IsChecked == true)
                    {
                        Extra extra = new Extra("Breakfast");
                        extraType.SetExtra(breakfast);
                        breakfast.ProcessExtra(extra);
                        _newBooking.addExtra(extra);
                    }

                    if (ckbDinner.IsChecked == true)
                    {
                        Extra extra = new Extra("Evening Meal");
                        extraType.SetExtra(eveningMeal);
                        eveningMeal.ProcessExtra(extra);
                        _newBooking.addExtra(extra);
                    }

                    if (ckbCarHire.IsChecked == true)
                    {
                        if (clrStartHire.SelectedDate == null || clrEndHire.SelectedDate == null || tbxDriver.Text == null)
                        {
                            throw new ArgumentException("Please select a start hire date, an end hire date, and a driver name.");
                        }

                        Extra extra = new Extra("Car Hire");
                        extraType.SetExtra(carHire);
                        carHire.ProcessExtra(extra);
                        extra.DriverName = tbxDriver.Text;
                        extra.StartHire = Convert.ToDateTime(clrStartHire.SelectedDate);
                        extra.EndHire = Convert.ToDateTime(clrEndHire.SelectedDate);
                        _newBooking.addExtra(extra);
                    }

                    data.CustDict.StoreCustomers[_custID].addBooking(_newBooking);

                    if (!data.CustDict.ChaletsBooked.Contains(Int32.Parse(tbxChaletID.Text)))
                    {
                        _newBooking.ChaletID = Int32.Parse(tbxChaletID.Text);
                        data.CustDict.ChaletsBooked.Add(_newBooking.ChaletID);
                    }
                    else
                        throw new ArgumentException("Chalet already booked, please choose another chalet.");
                }

                data.updateFile();
            }
            catch(Exception excep)
            {
                MessageBox.Show(excep.Message);
            }

        }

        // This method is called on CLOSE button click. It creates and open the Customer window page and close this current one.

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Customer custPage = new Customer(data, _custID);
            custPage.Show();
            this.Close();
        }

        // Both this methods old focus of the last passport number when change are made in the Guest's textboxes.
        private void tbxGuestName_GotFocus(object sender, RoutedEventArgs e)
        {
            oldPassportNo = tbxPassportNo.Text;
        }

        private void tbxAge_GotFocus(object sender, RoutedEventArgs e)
        {
            oldPassportNo = tbxPassportNo.Text;
        }

    }
}

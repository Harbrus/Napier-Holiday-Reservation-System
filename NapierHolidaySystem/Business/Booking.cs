using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /*
     * Author name: Andrea Silvestro Ortino
     * Business.Booking is a class which helds booking properties/attributes and methods to add guests and extra options. It also calculate the cost of a booking both daily and in total.
     * Date last modified: 22/11/2017 15:55
     */ 

    [Serializable]
    public class Booking
    {
        private DateTime _arrivalDate;
        private DateTime _departureDate;
        private int _bookingRefNo;
        private int _chaletID;
        private SortedDictionary<string, Guest> _guestDict = new SortedDictionary<string,Guest>();
        private SortedDictionary<string, Extra> _extraDict = new SortedDictionary<string, Extra>();
        
        // Booking constructor which takes bookingRefNo, arrivalDate and departureDate as a parameters in order to instantiate an object.
        public Booking(int bookingRefNo, DateTime arrivalDate, DateTime departureDate)
        {
            if (arrivalDate > departureDate)
            {
                throw new ArgumentException("Departure Date must be after Arrival Date.");
            }
            else
            {
                this._bookingRefNo = bookingRefNo;
                this.ArrivalDate = arrivalDate;
                this.DepartureDate = departureDate;
            }
        }

        // ArrivalDate property with get/set methods (with proper validation check) to return and/or set the corrispective private attribute.
        public DateTime ArrivalDate
        {
            get
            {
                return _arrivalDate;
            }
            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new ArgumentException("Insert a valid Arrival Date.");
                }
                else
                    this._arrivalDate = value;
            }
        }

        // DepartureDate property with get/set methods (with proper validation check) to return and/or set the corrispective private attribute.
        public DateTime DepartureDate
        {
            get
            {
                return _departureDate;
            }
            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new ArgumentException("Insert a valid Departure Date.");
                }
                else
                    this._departureDate = value;
            }
        }

        // BookingRefNo property with get method to return the corrispective private attribute.
        public int BookingRefNo
        {
            get
            {
                return _bookingRefNo;
            }
        }

        // ChaleID property with get/set methods (with proper validation check) to return and/or set the corrispective private attribute.
        public int ChaletID
        {
            get
            {
                return this._chaletID;
            }
            set
            {
                if ((value > 10 | value < 1) || value.ToString().Length > 4)
                {
                    throw new ArgumentException("Insert a Chalet ID between 1 and 10.");
                }
                this._chaletID = value;
            }
        }

        // GuestDict property with get method to return the corrispective private dictionary.
        public SortedDictionary<string, Guest> GuestDict
        {
            get
            {
                return _guestDict;
            }
        }

        // ExtraDict property with get method to return the corrispective private dictionary.
        public SortedDictionary<string, Extra> ExtraDict
        {
            get
            {
                return _extraDict;
            }
        }

        // This method add to the _guestDict dictionary the Guest's PassPortNo property as a key and a Guest object as a value.
        public void addGuest(Guest guest)
        {
            if(_guestDict.Count > 6)
            {
                throw new ArgumentException("Cannot add more than 6 guest to a booking.");
            }
            _guestDict.Add(guest.PassportNo, guest);
        }

        // This method add to the _extraDict dictionary the Extra's Description property as a key and a Extra object as a value.
        public void addExtra(Extra extra)
        {
            _extraDict.Add(extra.Description, extra);
        }

        // This method make appropriate calculation of the day cost of a booking. It takes in considerazion extra options' cost and number of guests.
        public string getCost()
        {
            double dayBookingCost = 60;
            double guestsCost = 25 * GuestDict.Count;
            double breakfastCost = 0;
            double eveningMealsCost = 0;
            double carHireCost = 0;
            double guestCostMultiplier = 1;

            if(ExtraDict.ContainsKey("Breakfast"))
            {
                if(GuestDict.Count != 0)
                {
                    guestCostMultiplier = GuestDict.Count;
                }

                breakfastCost =+ ExtraDict["Breakfast"].Cost * guestCostMultiplier;
            }
            if (ExtraDict.ContainsKey("Evening Meal"))
            {
                if (GuestDict.Count != 0)
                {
                    guestCostMultiplier = GuestDict.Count;
                }

                eveningMealsCost = +ExtraDict["Evening Meal"].Cost * guestCostMultiplier;
            }
            if (ExtraDict.ContainsKey("Car Hire"))
            {
                if (GuestDict.Count != 0)
                {
                    guestCostMultiplier = GuestDict.Count;
                }

                carHireCost =+ ExtraDict["Car Hire"].Cost;
            }

            double totalDayCost = dayBookingCost + guestsCost + breakfastCost + eveningMealsCost + carHireCost;
            
            return "The cost per day is:\n - Chalet: " + dayBookingCost + "£\n - Number of guests/cost: " + GuestDict.Count + " / " + guestsCost + "£\n - Breakfast: " + breakfastCost
                + "£\n - Evening Meals: " + eveningMealsCost + "£\n - Car Hire: " + carHireCost + "£\n Total cost per day: " + totalDayCost + "£";
        }

        // This method make appropriate calculation of the total cost of a booking. It takes in considerazion extra options' cost and number of guests.
        public string getTotalCost()
        {
            double bookingLength = (this.DepartureDate - this.ArrivalDate).TotalDays;
            double bookingCost = 60 * bookingLength;
            double guestsCost = 25 * GuestDict.Count * bookingLength;
            double breakfastCost = 0;
            double eveningMealsCost = 0;
            double carHireCost = 0;
            double guestCostMultiplier = 1;

            if (ExtraDict.ContainsKey("Breakfast"))
            {
                if (GuestDict.Count != 0)
                {
                    guestCostMultiplier = GuestDict.Count;
                }

                breakfastCost = ExtraDict["Breakfast"].Cost * guestCostMultiplier * bookingLength;
            }
            if (ExtraDict.ContainsKey("Evening Meal"))
            {
                if (GuestDict.Count != 0)
                {
                    guestCostMultiplier = GuestDict.Count;
                }

                eveningMealsCost = ExtraDict["Evening Meal"].Cost * guestCostMultiplier * bookingLength;
            }
            if (ExtraDict.ContainsKey("Car Hire"))
            {
                if (GuestDict.Count != 0)
                {
                    guestCostMultiplier = GuestDict.Count;
                }

                carHireCost = ExtraDict["Car Hire"].Cost * ((ExtraDict["Car Hire"].EndHire - ExtraDict["Car Hire"].StartHire).TotalDays);
            }

            double totalCost = bookingCost + guestsCost + breakfastCost + eveningMealsCost + carHireCost;

            return "The total booking cost is: " + totalCost + "£";
        }

        // This method override the toString method in order to print a booking object directly with the return of this method.
        public override string ToString()
        {
            return "Booking Reference No: " + this.BookingRefNo.ToString() + " - From " + this.ArrivalDate.ToString() + " to " + this.DepartureDate.ToString() + ", Chalet ID: " + this.ChaletID.ToString();
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Business;

namespace BookingTest
{
    /*
     * Author name: Andrea Silvestro Ortino
     * BookingTestUnit is a class for testing the functionality of the Business.Booking Class
     * Date last modified: 22/11/2017 15:55
     */ 

    [TestClass]
    public class BookingTestUnit
    {
        // This method tests whether the constructor of the Business Booking class is set properly.
        [TestMethod]
        public void Constructor_WithValidAttributes()
        {   
            // arrange attributes
            int bookingRefNo = 1;
            DateTime arrivalDate = new DateTime(2017, 1, 18);
            DateTime departureDate = new DateTime(2017, 2, 18);

            // act - calling methods
            Booking testBooking = new Booking(bookingRefNo, arrivalDate, departureDate);

            // assert - test correctness
            Assert.AreEqual(bookingRefNo, testBooking.BookingRefNo, "Booking Reference Number not set properly.");
            Assert.AreEqual(arrivalDate, testBooking.ArrivalDate, "Arrival Date not set properly.");
            Assert.AreEqual(departureDate, testBooking.DepartureDate, "Departure Date not set properly.");
        }

        // This method tests whether the properties, addExtra, addGuest, and the toString method of the Business Booking class are working properly.
        [TestMethod]
        public void Booking_WithValidProperties()
        {
            // arrange attributes
            int bookingRefNo = 1;
            DateTime arrivalDate = new DateTime(2017, 1, 18);
            DateTime departureDate = new DateTime(2017, 2, 18);
            int chaletID = 1;
            string bookingToString = "Booking Reference No: " + bookingRefNo.ToString() + " - From " + arrivalDate.ToString() + " to " + departureDate.ToString() + ", Chalet ID: " + chaletID.ToString();

            Guest guest1 = new Guest("Andrea", "ya11121314", 23);
            SortedDictionary<string, Guest> guestDict = new SortedDictionary<string,Guest>();
            guestDict.Add(guest1.PassportNo, guest1);

            SortedDictionary<string,Extra> extraDict = new SortedDictionary<string,Extra>();
            Extra newExtra = new Extra("Breakfast");
            ExtraSelector breakfast = new Breakfast();
            breakfast.SetExtra(breakfast);
            breakfast.ProcessExtra(newExtra);
            extraDict.Add(newExtra.Description, newExtra);

            // act - calling methods
            Booking testBooking = new Booking(bookingRefNo, arrivalDate, departureDate);
            testBooking.ArrivalDate = arrivalDate;
            testBooking.DepartureDate = departureDate;
            testBooking.ChaletID = chaletID;
            testBooking.addGuest(guest1);
            testBooking.addExtra(newExtra);
            string callToString = testBooking.ToString();

            // assert - test correctness
            Assert.AreEqual(chaletID, testBooking.ChaletID, "ChaletID not set properly.");
            Assert.AreEqual(bookingRefNo, testBooking.BookingRefNo, "Booking Reference Number not set properly.");
            Assert.AreEqual(arrivalDate, testBooking.ArrivalDate, "Arrival Date not set properly.");
            Assert.AreEqual(departureDate, testBooking.DepartureDate, "Departure Date not set properly.");
            CollectionAssert.AreEqual(guestDict, testBooking.GuestDict, "Guests Dictionary not set properly.");
            CollectionAssert.AreEqual(extraDict, testBooking.ExtraDict, "Extras Dictionary not set properly.");
            Assert.AreEqual(bookingToString, callToString, "To string method not set properly.");
        }

        // This method tests whether the getCost and the getTotalCost methods of the Business Booking class make correct calculations.
        [TestMethod]
        public void Booking_CalculateCosts_WithValidAmount()
        {
            DateTime arrivalDate = new DateTime(2017, 1, 18);
            DateTime departureDate = new DateTime(2017, 1, 25);

            Booking testBooking = new Booking(4, arrivalDate, departureDate);

            Guest guest1 = new Guest("Andrea", "ya11121314", 23);
            Guest guest2 = new Guest("Joseph", "ya11121315", 21);
            Guest guest3 = new Guest("Keith", "ya122221315", 22);
            testBooking.addGuest(guest1);
            testBooking.addGuest(guest2); 
            testBooking.addGuest(guest3);

            Extra newExtra1 = new Extra("Breakfast");
            Extra newExtra2 = new Extra("Car Hire");
            ExtraSelector breakfast = new Breakfast();
            ExtraSelector carHire = new CarHire();
            breakfast.SetExtra(breakfast);
            breakfast.ProcessExtra(newExtra1);
            breakfast.SetExtra(carHire);
            carHire.ProcessExtra(newExtra2);
            newExtra2.StartHire = new DateTime(2017, 1, 18);
            newExtra2.EndHire = new DateTime(2017, 1, 20);
            testBooking.addExtra(newExtra1);
            testBooking.addExtra(newExtra2);

            double expectedDayCost = 200;
            double expectedTotalCost = 1150;
            string expected1 = "The cost per day is:\n - Chalet: " + 60 + "£\n - Number of guests/cost: " + testBooking.GuestDict.Count + " / " + 25*testBooking.GuestDict.Count
                + "£\n - Breakfast: " + 5*testBooking.GuestDict.Count + "£\n - Evening Meals: " + 0 + "£\n - Car Hire: " + 50 + "£\n Total cost per day: " + expectedDayCost + "£";
            string expected2 = "The total booking cost is: " + expectedTotalCost + "£";

            // act - calling methods
            string method1 = testBooking.getCost();
            string method2 = testBooking.getTotalCost();

            // assert - test correctness
            Assert.AreEqual(expected1, method1, "Cost per day calculation is wrong.");
            Assert.AreEqual(expected2, method2, "Total cost calculation is wrong.");

        }
    }
}
 

            
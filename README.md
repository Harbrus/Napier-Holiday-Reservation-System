# Napier Holiday System

The Napier Holiday Village is a reservation system which handles holiday chalet bookings for customers. Each booking is allocated to a chalet; there are 10 chalets that may be allocated.

Each basic booking comprises the following attributes:
- Attribute Type
- Arrival Date Date
- Departure Date Date
- Booking Reference Number: Auto increment starting at 1
- ChaletID: In range 1 to 10

In addition each booking is associated with a customer who has the following attributes:
- Attribute Type
- Name String
- Address String
- Customer Reference Number: Auto increment starting at 1

A customer may have many bookings, each booking must be associated with a customer. Each booking will have a number of guests, up to a maximum of 6 (a customer is not necessarily a guest):
- Attribute Type
- Name String
- Passport Number String (max 10 chars)
- Age Integer 0-101

The cost of a basic chalet is calculated as follows:
- Basic cost per night for a chalet is £60 plus £25 for each guest staying
- Any extras are charged per person, per night
- The total cost of the booking is the sum of the nightly costs/Holidays may have a number of extra options added on
- Evening meals: A chalet may have evening meals I provided, the cost will bean extra £10 per guest per night Meals can only be added for all guests for the duration of the booking.
- Breakfast: A holiday may have breakfast added on to it, the cost will be an extra £5 per guest per day. Meals can only be added for all guests for the duration of the booking.
- Car hire: Car hire costs £50 per day extra. When a car hire is associated with a booking the following should be noted, hire start date, hire end date and name of driver.

The system allows bookings to be added, amended and deleted.

The system  is able to cope with any number of customers, each of whom may have multiple bookings. It is possible to amend a booking (including adding or removing extras) once it has been entered. 

The system is also able to produce invoices showing the cost of a booking, the invoice should show the costs per night and the costs of any extras.

Other Functionalities:
1. When creating a booking check to see if the selected chalet is available, if the new booking clashes with an existing booking then display an error message.
2. Have the data layer store the bookings in a file or database in order to make them persistent

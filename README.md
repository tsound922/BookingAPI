# Booking API

## Features
- Book a time slot

## Tech
This Booking API is using **.NET Core**

## Installation
This project is using **.Net 6**. Please make sure the .NET 6 RUNTIME has been installed. 
The project is using several libraries. Please manage the lib installation in NuGet Manager.

## APIs
**api/Booking**
This api is to book a time slot. 
The business hour is from 9.00am to 5.00pm, and latest booking
is 4.00pm. 4.00pm to 5pm is for processing all day's bookings and will not accept new ones.
The maximum settlements are 4 
**api/Booking/bookingSlot**
This API assumes to process all booked appointments in the system. It will release the occupied booking slots so users can add new books via the API above.

## Usage
When start the solution, the **Swagger API document Tool** will appear.

## License
MIT


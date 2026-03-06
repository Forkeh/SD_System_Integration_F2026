using AirlineWsdl.Messages;
using AirlineWsdl.Models;

namespace AirlineWsdl.Services;

public class AirlineService : IAirlineService
{
    public SearchFlightsResponse SearchFlights(SearchFlightsRequest request)
    {
        // Stub implementation — returns sample data
        return new SearchFlightsResponse
        {
            Flights = new List<Flight>
            {
                new Flight
                {
                    Carrier = "Scandinavian Airlines",
                    FlightNumber = "SK1234",
                    DepartDateTime = request.DepartureDate.AddHours(8),
                    ArriveDateTime = request.DepartureDate.AddHours(12),
                    Origin = request.Origin,
                    Destination = request.Destination,
                    Price = new Money { Amount = 299.99m, Currency = "EUR" }
                }
            }
        };
    }
}

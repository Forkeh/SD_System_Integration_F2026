# Airline System WSDL

A .NET web service using **SoapCore** that exposes a SOAP endpoint with a WSDL document for an airline flight search system.

## Flow

1. **Client sends a SOAP request** to `POST /AirlineService.asmx` with a `SearchFlightsRequest` envelope containing origin/destination airport codes, travel dates, passenger counts, and cabin class.

2. **SoapCore middleware** deserializes the XML envelope into the `SearchFlightsRequest` data contract and routes it to the `IAirlineService.SearchFlights` operation.

3. **`AirlineService.SearchFlights`** processes the request (queries flights matching the criteria) and returns a `SearchFlightsResponse` containing a list of `Flight` objects.

4. **SoapCore serializes the response** back into a SOAP XML envelope and sends it to the client.

Clients can also retrieve the WSDL contract at `GET /AirlineService.asmx?wsdl` to discover the service's operations, messages, and data types.

## Data Types

| Type             | Kind    | Description                                                                       |
| ---------------- | ------- | --------------------------------------------------------------------------------- |
| `CabinClass`     | Enum    | `ECONOMY`, `PREMIUM_ECONOMY`, `BUSINESS`, `FIRST`                                 |
| `PassengerCount` | Complex | Adults, Children, Infants                                                         |
| `Money`          | Complex | Amount (decimal) + Currency (string)                                              |
| `Flight`         | Complex | Carrier, FlightNumber, DepartDateTime, ArriveDateTime, Origin, Destination, Price |

## Messages

- **`SearchFlightsRequest`** — Origin, Destination, DepartureDate, ReturnDate, Passengers, Cabin
- **`SearchFlightsResponse`** — List of `Flight`

## Run

```bash
dotnet run
```

The WSDL is available at `http://localhost:<port>/AirlineService.asmx?wsdl`.

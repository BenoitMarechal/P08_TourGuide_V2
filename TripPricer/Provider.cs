using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripPricer;

public class Provider
{
    public Guid TripId { get; set; }
    public string ProviderName { get; set; }
    public double Price { get; set; }

    // Constructor (optional)
    public Provider(Guid tripId, string providerName, double price)
    {
        TripId = tripId;
        ProviderName = providerName;
        Price = price;
    }

    // Parameterless constructor is required for model binding/serialization
    public Provider() { }
}

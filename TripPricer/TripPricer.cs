using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripPricer.Helpers;

namespace TripPricer;

public class TripPricer
{
    // Semaphore limits concurrent access to 100 calls (adjust as needed)
    private static readonly SemaphoreSlim _semaphore = new(initialCount: 100, maxCount: 100);

    public async Task<IEnumerable<Provider>> GetPrice(
        string apiKey,
        Guid attractionId,
        int adults,
        int children,
        int nightsStay,
        int rewardsPoints)
    {
        await _semaphore.WaitAsync(); // wait for access

        try
        {
            List<Provider> providers = new();
            HashSet<string> providersUsed = new();

            // Non-blocking simulated delay
            int delay = Random.Shared.Next(1, 50);
            await Task.Delay(delay);

            for (int i = 0; i < 10; i++)
            {
                int multiple = Random.Shared.Next(100, 700);
                double childrenDiscount = children / 3.0;
                double price = multiple * adults + multiple * childrenDiscount * nightsStay + 0.99 - rewardsPoints;
                price = Math.Max(0.0, price);

                string provider;
                do
                {
                    provider = GetProviderName(apiKey, adults);
                } while (!providersUsed.Add(provider)); // only adds if not present

                providers.Add(new Provider(attractionId, provider, price));
            }

            return providers;
        }
        finally
        {
            _semaphore.Release(); // release the slot
        }
    }

    public string GetProviderName(string apiKey, int adults)
    {
        int multiple = Random.Shared.Next(0, 10);
        return multiple switch
        {
            0 => "Holiday Travels",
            1 => "Enterprize Ventures Limited",
            2 => "Sunny Days",
            3 => "FlyAway Trips",
            4 => "United Partners Vacations",
            5 => "Dream Trips",
            6 => "Live Free",
            7 => "Dancing Waves Cruselines and Partners",
            8 => "AdventureCo",
            9 => "Cure-Your-Blues",
            _ => throw new InvalidOperationException("Unexpected provider index")
        };
    }
}
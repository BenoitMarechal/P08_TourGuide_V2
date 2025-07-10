using GpsUtil.Location;
using System.Runtime.CompilerServices;
using TourGuide.LibrairiesWrappers.Interfaces;
using TourGuide.Services.Interfaces;
using TourGuide.Users;

namespace TourGuide.Services;

public class RewardsService : IRewardsService
{
    private const double StatuteMilesPerNauticalMile = 1.15077945;
    private readonly int _defaultProximityBuffer = 10;
    //private readonly int _defaultProximityBuffer = int.MaxValue;
    private int _proximityBuffer;
    private readonly int _attractionProximityRange = 200;
    private readonly IGpsUtil _gpsUtil;
    private readonly IRewardCentral _rewardsCentral;
    private static int count = 0;

    public RewardsService(IGpsUtil gpsUtil, IRewardCentral rewardCentral)
    {
        _gpsUtil = gpsUtil;
        _rewardsCentral =rewardCentral;
        _proximityBuffer = _defaultProximityBuffer;
    }

    public void SetProximityBuffer(int proximityBuffer)
    {
        _proximityBuffer = proximityBuffer;
    }

    public void SetDefaultProximityBuffer()
    {
        _proximityBuffer = _defaultProximityBuffer;
    }

    public async Task  CalculateRewards(User user)
    {
        count++;
        IEnumerable<VisitedLocation> userLocations = user.VisitedLocations;
        IEnumerable<Attraction> attractions =await _gpsUtil.GetAttractions();

        foreach (var visitedLocation in userLocations)
        {
            foreach (var attraction in attractions)
            {
                if (!user.UserRewards.Any(r => r.Attraction.AttractionName == attraction.AttractionName))
                {
                    if (await NearAttraction(visitedLocation, attraction))
                    {
                        await user.AddUserReward(new UserReward(visitedLocation, attraction, await GetRewardPoints(attraction, user)));
                    }
                }
            }
        }
    }

    public async Task<bool> IsWithinAttractionProximity(Attraction attraction, Locations location)
    {
        var distance = await GetDistance(attraction, location);
        Console.WriteLine(distance);
        return distance <= _attractionProximityRange;
    }

    private async Task<bool> NearAttraction(VisitedLocation visitedLocation, Attraction attraction)
    {
        return await GetDistance(attraction, visitedLocation.Location) <= _proximityBuffer;
    }

    private async Task<int> GetRewardPoints(Attraction attraction, User user)
    {
        var rewardsPoints= await _rewardsCentral.GetAttractionRewardPoints(attraction.AttractionId, user.UserId);


        return await _rewardsCentral.GetAttractionRewardPoints(attraction.AttractionId, user.UserId);
    }

    public async Task<double> GetDistance(Locations loc1, Locations loc2)
    {
        double lat1 = Math.PI * loc1.Latitude / 180.0;
        double lon1 = Math.PI * loc1.Longitude / 180.0;
        double lat2 = Math.PI * loc2.Latitude / 180.0;
        double lon2 = Math.PI * loc2.Longitude / 180.0;

        double angle = Math.Acos(Math.Sin(lat1) * Math.Sin(lat2)
                                + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(lon1 - lon2));

        double nauticalMiles = 60.0 * angle * 180.0 / Math.PI;
        return StatuteMilesPerNauticalMile * nauticalMiles;
    }
}

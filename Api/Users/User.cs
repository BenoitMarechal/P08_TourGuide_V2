using GpsUtil.Location;
using TripPricer;

namespace TourGuide.Users;

public class User
{
    public Guid UserId { get; }
    public string UserName { get; }
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
    public DateTime LatestLocationTimestamp { get; set; }

    //public IEnumerable<VisitedLocation> VisitedLocations { get; } = new List<VisitedLocation>();
    private readonly List<VisitedLocation> _visitedLocations = new();
    public IEnumerable<VisitedLocation> VisitedLocations => _visitedLocations;

    //public IEnumerable<UserReward> UserRewards { get; } = new List<UserReward>();
    private readonly List<UserReward> _userRewards = new();
    public IEnumerable<UserReward> UserRewards => _userRewards;

    //public IEnumerable<Provider> TripDeals { get; set; } = new List<Provider>();
    private List<Provider> _tripDeals = new();
    public IEnumerable<Provider> TripDeals
    {
        get => _tripDeals;
        set => _tripDeals = value.ToList(); // defensively copy
    }





    public UserPreferences UserPreferences { get; set; } = new UserPreferences();

    public User(Guid userId, string userName, string phoneNumber, string emailAddress)
    {
        UserId = userId;
        UserName = userName;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
    }

    public async Task AddToVisitedLocations(VisitedLocation visitedLocation)
    {
        _visitedLocations.Add(visitedLocation);
    }

    public async Task ClearVisitedLocations()
    {
        _visitedLocations.Clear();
    }

    public async Task AddUserReward(UserReward userReward)
    {
        if (!_userRewards.Exists(r => r.Attraction.AttractionName == userReward.Attraction.AttractionName))
        {
            _userRewards.Add(userReward);
        }
    }

    public VisitedLocation GetLastVisitedLocation()
    {
        if (_visitedLocations.Count == 0)
            throw new InvalidOperationException("No visited locations found.");

        return _visitedLocations[^1];
    }
}

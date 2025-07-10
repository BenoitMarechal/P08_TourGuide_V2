using GpsUtil.Location;

namespace TourGuide.LibrairiesWrappers.Interfaces
{
    public interface IGpsUtil
    {
        Task<VisitedLocation> GetUserLocation(Guid userId);
        Task<IEnumerable<Attraction>> GetAttractions();
    }
}

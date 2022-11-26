namespace CityInfo.API.Models
{
    /// <summary>
    /// A DTO for a city without points of interest
    /// </summary>
    public class CityWithoutPointOfInterestDto
    {
        /// <summary>
        /// Id of the city
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the city
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of the city
        /// </summary>
        public string? Description { get; set; }
    }
}

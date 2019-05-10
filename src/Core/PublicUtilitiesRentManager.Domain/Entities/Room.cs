namespace PublicUtilitiesRentManager.Domain.Entities
{
    public class Room
    {
        public string Id { get; set; }
        public string RoomTypeId { get; set; }
        public string Address { get; set; }
        public double Square { get; set; }
        public bool IsOccupied { get; set; }
        public decimal Price { get; set; }
        public decimal IncreasingCoefToBaseRate { get; set; }
        public decimal PlacementCoef { get; set; }
        public decimal ComfortCoef { get; set; }
        public decimal SocialOrientationCoef { get; set; }
    }
}
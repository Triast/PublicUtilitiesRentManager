namespace PublicUtilitiesRentManager.Domain.Entities
{
    public class Room
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public string RoomType { get; set; }
        public double Square { get; set; }
        public decimal Price { get; set; }
        public bool IsOccupied { get; set; }
    }
}
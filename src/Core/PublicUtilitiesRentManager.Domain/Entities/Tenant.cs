namespace PublicUtilitiesRentManager.Domain.Entities
{
    public class Tenant
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
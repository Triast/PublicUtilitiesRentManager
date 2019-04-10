using System;

namespace PublicUtilitiesRentManager.Domain.Entities
{
    public class Contract
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string RoomId { get; set; }
        public string AccrualTypeId { get; set; }
        public string CalcCoefficientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

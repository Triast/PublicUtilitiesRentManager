using PublicUtilitiesRentManager.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class RoomViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Тип помещения")]
        public string RoomType { get; set; }
        [Display(Name = "Площадь")]
        public double Square { get; set; }
        [Display(Name = "Цена (за кв. м)")]
        public decimal Price { get; set; }
        [Display(Name = "Занято")]
        public bool IsOccupied { get; set; }

        public static RoomViewModel FromRoom(Room room) =>
            new RoomViewModel
            {
                Id = room.Id,
                Address = room.Address,
                RoomType = room.RoomType,
                Square = room.Square,
                Price = room.Price,
                IsOccupied = room.IsOccupied
            };
    }
}

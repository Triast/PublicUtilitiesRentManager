using PublicUtilitiesRentManager.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class RoomTypeViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }

        public static RoomTypeViewModel FromRoomType(RoomType roomType) =>
            new RoomTypeViewModel
            {
                Id = roomType.Id,
                Name = roomType.Name
            };
    }
}

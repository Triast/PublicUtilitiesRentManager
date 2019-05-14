using Microsoft.AspNetCore.Mvc.Rendering;
using PublicUtilitiesRentManager.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class RoomViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        public string RoomTypeId { get; set; }
        [Display(Name = "Тип помещения")]
        public string RoomType { get; set; }
        [Display(Name = "Площадь")]
        public double Square { get; set; }
        [Display(Name = "Занято")]
        public bool IsOccupied { get; set; }
        [Display(Name = "Ставка за 1 кв. м")]
        public decimal Price { get; set; }
        [Display(Name = "Повышаюший коэффициент к базовой ставке")]
        public decimal IncreasingCoefToBaseRate { get; set; }
        [Display(Name = "Коэффициент местоположения")]
        public decimal PlacementCoef { get; set; }
        [Display(Name = "Коэффициент удобства")]
        public decimal ComfortCoef { get; set; }
        [Display(Name = "Коэффициент соц. направленности")]
        public decimal SocialOrientationCoef { get; set; }

        public SelectList RoomTypes { get; set; }

        public static RoomViewModel FromRoom(Room room) =>
            new RoomViewModel
            {
                Id = room.Id,
                RoomTypeId = room.RoomTypeId,
                Address = room.Address,
                Square = room.Square,
                IsOccupied = room.IsOccupied,
                Price = room.Price,
                IncreasingCoefToBaseRate = room.IncreasingCoefToBaseRate,
                PlacementCoef = room.PlacementCoef,
                ComfortCoef = room.ComfortCoef,
                SocialOrientationCoef = room.SocialOrientationCoef
            };

        public static Room FromRoomViewModel(RoomViewModel viewModel) =>
            new Room
            {
                Id = viewModel.Id,
                RoomTypeId = viewModel.RoomTypeId,
                Address = viewModel.Address,
                Square = viewModel.Square,
                IsOccupied = viewModel.IsOccupied,
                Price = viewModel.Price,
                IncreasingCoefToBaseRate = viewModel.IncreasingCoefToBaseRate,
                PlacementCoef = viewModel.PlacementCoef,
                ComfortCoef = viewModel.ComfortCoef,
                SocialOrientationCoef = viewModel.SocialOrientationCoef
            };
    }
}

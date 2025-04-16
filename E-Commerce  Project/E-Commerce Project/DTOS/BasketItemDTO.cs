using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Project.DTOS
{
    public class BasketItemDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price Can Not Be Zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1,int.MaxValue ,ErrorMessage ="Quantity Must Be One Item at Least")]
        public int Quantity { get; set; }
    }
}
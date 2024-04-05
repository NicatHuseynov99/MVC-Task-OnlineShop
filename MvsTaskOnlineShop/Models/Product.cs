namespace MvsTaskOnlineShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image {  get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Size> Sizes { get; set;}
        public int ColorId { get; set; }
        public Color Color { get; set; }
    }
}

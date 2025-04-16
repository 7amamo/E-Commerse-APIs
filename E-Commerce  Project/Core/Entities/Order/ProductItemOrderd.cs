namespace Core.Entities.Order
{
    public class ProductItemOrderd
    {
        public ProductItemOrderd()
        {
            
        }

        public ProductItemOrderd(int productId, string productName, string pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}
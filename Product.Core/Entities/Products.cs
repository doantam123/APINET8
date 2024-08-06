namespace Product.Core.Entities
{
    public class Products : BasicEntity<int>
    {
        // public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string ProductPicture { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}

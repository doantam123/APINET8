namespace Product.Core.Entities
{
    public class Category : BasicEntity<int>
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public virtual ICollection<Products> Products { get; set; } = new HashSet<Products>();
    }
}

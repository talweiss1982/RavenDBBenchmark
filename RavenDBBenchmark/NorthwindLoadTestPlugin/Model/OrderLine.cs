namespace Orders
{
    public class OrderLine
    {
        public string Product { get; set; }
        public string ProductName { get; set; }
        public double PricePerUnit { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
    }
}
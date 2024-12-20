namespace Discount.Grpc.Models;

public class Coupon
{
    public int Id { get; set; } = default;
    public string ProductName { get; set; } = default!;
    public string ProductDescription { get; set; } = default!;
    public double Amount { get; set; } = default;
}

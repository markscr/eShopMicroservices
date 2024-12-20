using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

// DiscountProtoService is a class auto generated after building the project
// Every proto class properties Build Action must be set to Protobuf compiler
// in order to generate the base classes
public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModelList> GetAllDiscounts(
        GetAllDiscountsRequest request,
        ServerCallContext context
    )
    {
        var coupons = await dbContext.Coupones.ToListAsync();
        CouponModelList response = new();
        foreach (var coupon in coupons)
        {
            CouponModel model = new()
            {
                Id = coupon.Id,
                ProductName = coupon.ProductName,
                ProductDescription = coupon.ProductDescription,
                Amount = coupon.Amount,
            };
            response.Coupons.Add(model);
        }

        logger.LogInformation("Returning all discounts");

        return response;
    }

    public override async Task<CouponModel> GetDiscount(
        GetDiscountRequest request,
        ServerCallContext context
    )
    {
        Coupon? coupon = await dbContext.Coupones.FirstOrDefaultAsync(x =>
            x.ProductName == request.ProductName
        );

        if (coupon is null)
        {
            coupon = new()
            {
                ProductName = "No discount",
                ProductDescription = "No discount",
                Amount = 0,
            };
        }

        logger.LogInformation(
            "Discount returned for ProductName: {productName} ProductDescription: {description}, Amount: {amount}",
            coupon.ProductName,
            coupon.ProductDescription,
            coupon.Amount
        );

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> CreateDiscount(
        CreateDiscountRequest request,
        ServerCallContext context
    )
    {
        Coupon coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
        }

        dbContext.Coupones.Add(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation(
            "Discount created for ProductName: {productName}",
            request.Coupon.ProductName
        );

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(
        UpdateDiscountRequest request,
        ServerCallContext context
    )
    {
        Coupon coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
        }

        Coupon? dbCoupon = await dbContext.Coupones.FirstOrDefaultAsync(x =>
            x.Id == request.Coupon.Id
        );

        if (dbCoupon is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Invalid request"));
        }

        dbCoupon = coupon;
        await dbContext.SaveChangesAsync();

        logger.LogInformation(
            "Discount updated for ProductName: {productName} ProductDescription: {description}, Amount: {amount}",
            coupon.ProductName,
            coupon.ProductDescription,
            coupon.Amount
        );

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(
        DeleteDiscountRequest request,
        ServerCallContext context
    )
    {
        Coupon? dbCoupon =
            await dbContext.Coupones.FirstOrDefaultAsync(x => x.ProductName == request.ProductName)
            ?? throw new RpcException(new Status(StatusCode.NotFound, "Invalid request"));

        dbContext.Remove(dbCoupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation(
            "Discount deleted for ProductName: {productName}",
            dbCoupon.ProductName
        );

        return new() { Success = true };
    }
}

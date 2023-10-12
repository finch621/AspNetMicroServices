using Discount.Grpc.Protos;

namespace Discount.API.GrpcServices;
public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
    {
        _discountProtoService = discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
    }

    public async Task<CouponModel> GetDiscount(string productName)
    {
        var getDiscountRequest = new GetDiscountRequest { ProductName = productName };
        return await _discountProtoService.GetDiscountAsync(getDiscountRequest);
    }
}

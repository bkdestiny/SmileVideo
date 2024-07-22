namespace IdentityService.WebAPI.Controllers.UserAPI.Dtos
{
    public class GetUserPagingDataRequest : AbstractPagingDataRequest
    {
        public string SearchText { get; set; } = "";
    }
    public class GetUserPagingDataRequestValidator : AbstractPagingDataRequestValidator
    {
        public GetUserPagingDataRequestValidator():base()
        {

        }
    }
}

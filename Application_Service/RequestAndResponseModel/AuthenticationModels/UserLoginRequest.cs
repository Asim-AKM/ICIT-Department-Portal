namespace Application_Service.RequestAndResponseModel.AuthenticationModels
{
    public record class UserLoginRequest(string CNIC, string Password);
}

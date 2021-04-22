namespace Cms.Auth.IdentityProvider.Controllers
{
    public class DeviceAuthorizationInputModel : ConsentInputModel
    {
        public string UserCode { get; set; }
    }
}
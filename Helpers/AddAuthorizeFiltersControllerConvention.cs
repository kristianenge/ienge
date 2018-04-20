using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace IEnge.Helpers
{
    public class AddAuthorizeFiltersControllerConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerName.Contains("Api"))
            {
                controller.Filters.Add(new AuthorizeFilter("apipolicy"));
            }
            else if (controller.ControllerName == Constants.Strings.AuthControllerName)
            {
                //no Auth filters for /Auth

            }
            else
            {
                //controller.Filters.Add(new AuthorizeFilter("defaultpolicy"));
            }
        }
    }
}

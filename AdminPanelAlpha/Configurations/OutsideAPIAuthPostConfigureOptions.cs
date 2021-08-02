using Microsoft.Extensions.Options;

namespace Game.AdminPanelAlpha.Services
{
    public class OutsideAPIAuthPostConfigureOptions : IPostConfigureOptions<AuthOptions>
    {
        public void PostConfigure(string name, AuthOptions options)
        {
            //throw new System.NotImplementedException();
        }
    }
}
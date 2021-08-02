using Game.AdminPanelAlpha.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.AdminPanelAlpha.Extensions
{   public static class OutsideAPIAuthExtensions
    {
        public static AuthenticationBuilder AddOutsideAPIAuth(this AuthenticationBuilder builder, Action<AuthOptions> options)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<AuthOptions>, OutsideAPIAuthPostConfigureOptions>());
            return builder.AddScheme<AuthOptions, AuthHandler>(AuthHandler.SchemeName, String.Empty, null);
        }
    }
}

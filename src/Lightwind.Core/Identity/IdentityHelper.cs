// Licence: Apache-2.0
// Author: ws_dev@163.com
// ProjectUrl: https://github.com/wswind/lightwind

using Microsoft.AspNetCore.Http;
using System;


namespace Lightwind.Core.Identity
{
    public interface IIdentityHelper
    {
        string GetUserId();
        string GetUserName();
        string GetClaim(string type);
    }

    public class IdentityHelper : IIdentityHelper
    {
        private readonly IHttpContextAccessor _context;

        public IdentityHelper(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetUserId()
        {
            return _context.HttpContext.User.FindFirst("sub").Value;
        }

        public string GetUserName()
        {
            return _context.HttpContext.User.Identity.Name;
        }

        public string GetClaim(string type)
        {
            return _context.HttpContext.User.FindFirst(type)?.Value;
        }
    }
}

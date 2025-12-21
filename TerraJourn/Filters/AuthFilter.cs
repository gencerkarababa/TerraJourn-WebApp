using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TerraJourn.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 1. ÖZEL İZİN KONTROLÜ (AllowAnonymous)
            // Gidilen sayfada veya Controller'da [AllowAnonymous] etiketi var mı? Varsa geç.
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            // 2. OTURUM KONTROLÜ
            // İzin yoksa, oturum var mı?
            if (HttpContext.Current.Session["UserName"] == null)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
﻿using System.Web;
using System.Web.Mvc;

namespace Immanuel.Yt.Cmments.Feedback
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Nexus.Tools
{
    public static class NavigationPageManager
    {
        public static string ActivePageKey => "ActivePage";
        public static string ActiveControllerKey => "ActiveController";

        public static string Index => "Index";
        public static string List => "List";
        public static string Edit => "Edit";
        public static string New => "New";

        public static string Home => nameof(Home);
        public static string Notes => nameof(Notes);
        public static string Books => nameof(Books);
        public static string Courses => nameof(Courses);
        public static string Categories => nameof(Categories);
        public static string Projects => nameof(Projects);
        public static string Tags => nameof(Tags);
        public static string Account => nameof(Account);

        public static string GetLiClassSingleLi(ViewContext viewContext, string actionName, string controller)
        {
            var activeCtrl = viewContext.ViewData[ActiveControllerKey] as string;

            if (string.Equals(activeCtrl, controller))
            {
                var activeAction = viewContext.ViewData[ActivePageKey] as string;
                return string.Equals(activeAction, actionName, StringComparison.OrdinalIgnoreCase) ? "active" : "";
            }
            else
            {
                return "";
            }
        }

        public static string GetLiClass(ViewContext viewContext, string controllerName)
        {
            var activeCtrl = viewContext.ViewData[ActiveControllerKey] as string;
            return string.Equals(activeCtrl, controllerName, StringComparison.OrdinalIgnoreCase) ? "treeview active menu-open" : "treeview";
        }

        public static string GetUlStyle(ViewContext viewContext, string controllerName)
        {
            var activeCtrl = viewContext.ViewData[ActiveControllerKey] as string;
            return activeCtrl == controllerName ? "block" : "none";
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
        public static void AddActiveController(this ViewDataDictionary viewData, string activeController) => viewData[ActiveControllerKey] = activeController;
    }
}

using System.Web.Mvc;

namespace StudentSystem.Clients.Mvc
{
    public class ViewEngineConfig
    {
        public static void RegisterViewEngine()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}
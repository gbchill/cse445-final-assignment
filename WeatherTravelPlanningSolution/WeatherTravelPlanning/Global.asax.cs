using System;
using System.Web;

namespace WeatherTravelPlanning
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Initialize weather cache values
            Application["LastCacheUpdate"] = DateTime.Now;
            Application["WeatherCacheTimeout"] = 30;  // minutes

            // Log application start
            System.Diagnostics.Debug.WriteLine(
                "Weather Application Started: " + DateTime.Now);
        }

   
        protected void Application_End(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(
                "Weather Application Ended: " + DateTime.Now);
        }

  
        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception ex = Server.GetLastError();
        //    System.Diagnostics.Debug.WriteLine(
        //        "Unhandled Application Error: " + ex.Message);

        //    // Clear the error and redirect to a friendly page
        //    Server.ClearError();
        //    Response.Redirect("~/ErrorPage.aspx");
        //}

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["SessionStartTime"] = DateTime.Now;
            Session["PageViews"] = 0;

            // If the user has saved preferences in a cookie, load them into session
            HttpCookie prefCookie = Request.Cookies["WeatherPreferences"];
            if (prefCookie != null)
            {
                Session["TemperatureUnit"] = prefCookie["TempUnit"];
                Session["PreferredLocation"] = prefCookie["Location"];
            }

            System.Diagnostics.Debug.WriteLine(
                "New Session Started: " + Session.SessionID);
        }

    
        protected void Session_End(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(
                "Session Ended: " + Session.SessionID);
        }
    }
}

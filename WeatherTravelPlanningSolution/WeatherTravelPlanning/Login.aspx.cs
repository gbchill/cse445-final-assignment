using System;
using System.Web.Security;

namespace WeatherTravelPlanning
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.IsAuthenticated)
                Response.Redirect("Default.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            //hardcoded simple login check a5
            if ((username == "TA" && password == "Cse445!") || (username == "user" && password == "123"))
            {
                FormsAuthentication.SetAuthCookie(username, false);
                string redirectUrl = Request.QueryString["ReturnUrl"] ?? "Default.aspx";
                Response.Redirect(redirectUrl);
            }
            else
            {
                lblMessage.Text = "Invalid username or password.";
            }
        }
    }
}

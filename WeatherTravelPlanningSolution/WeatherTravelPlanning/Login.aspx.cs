using System;
using System.Web.Security;
using SecurityLibrary;
using System.IO;           
using System.Xml.Linq;

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
            string hashedInputPassword = PasswordHelper.HashPassword(password);

            string filePath = Server.MapPath("~/App_Data1/Member.xml");

            // Hardcoded TA login check first
            if ((username == "TA" && password == "Cse445!") || (username == "user" && password == "123"))
            {
                FormsAuthentication.SetAuthCookie(username, false);
                Session["Username"] = username;
                Session["UserType"] = "Staff"; // <-- Important: TA is a staff
                string redirectUrl = Request.QueryString["ReturnUrl"] ?? "Default.aspx";
                Response.Redirect(redirectUrl);
            }
            else if (File.Exists(filePath))
            {
                // Normal Member login check
                XDocument doc = XDocument.Load(filePath);
                bool validUser = false;

                foreach (var member in doc.Descendants("Member"))
                {
                    if (member.Element("Username").Value == username &&
                        member.Element("Password").Value == hashedInputPassword)
                    {
                        validUser = true;
                        break;
                    }
                }

                if (validUser)
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                    Session["Username"] = username;
                    Session["UserType"] = "Member"; // normal user
                    string redirectUrl = Request.QueryString["ReturnUrl"] ?? "Default.aspx";
                    Response.Redirect(redirectUrl);
                }
                else
                {
                    lblMessage.Text = "Invalid username or password.";
                }
            }
            else
            {
                lblMessage.Text = "No users registered yet.";
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Member.aspx");
        }

    }
}

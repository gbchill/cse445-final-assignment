using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml;

namespace WeatherTravelPlanning
{
    public partial class Login : Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string hashedPassword = GetSHA256Hash(password);
            string xmlPath;
            string nodeName;
            string role;
            string redirectPage;

            if (rbMember.Checked)
            {
                xmlPath = Server.MapPath("~/members.xml");
                nodeName = "Member";
                role = "Member";
                redirectPage = "Members.aspx";
            }
            else
            {
                xmlPath = Server.MapPath("~/staff.xml");
                nodeName = "StaffMember";
                role = "Staff";
                redirectPage = "Staff.aspx";
            }

            // Check XML file
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);

            foreach (XmlNode node in doc.SelectNodes($"//{nodeName}"))
            {
                string xmlEmail = node.SelectSingleNode("Email")?.InnerText;
                string xmlPassword = node.SelectSingleNode("Password")?.InnerText;

                if (xmlEmail == email && xmlPassword == hashedPassword)
                {
                    // Create authentication ticket with role
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1, // Version
                        email, // User name
                        DateTime.Now, // Issue time
                        DateTime.Now.AddDays(1), // Expiration
                        false, // Not persistent
                        role // User data (role)
                    );
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);

                    // Redirect to appropriate page
                    Response.Redirect(redirectPage);
                    return;
                }
            }

            lblMessage.Text = "Invalid email or password.";
        }

        private string GetSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
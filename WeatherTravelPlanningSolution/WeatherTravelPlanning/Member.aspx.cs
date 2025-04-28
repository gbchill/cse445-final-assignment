using System;
using System.Xml.Linq;
using System.IO;
using SecurityLibrary;
using WeatherTravelPlanning.Controls;


namespace WeatherTravelPlanning
{
    public partial class Member : System.Web.UI.Page
    {
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!CaptchaControl1.ValidateCaptcha())
            {
                lblMessage.Text = "Captcha incorrect. Please try again.";
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "Username and password cannot be empty.";
                return;
            }

            string hashedPassword = PasswordHelper.HashPassword(password); // Using your DLL

            string filePath = Server.MapPath("~/App_Data1/Member.xml");

            if (!File.Exists(filePath))
            {
                // Create the file if it doesn't exist
                new XDocument(new XElement("Members")).Save(filePath);
            }

            XDocument doc = XDocument.Load(filePath);

            // Check if username already exists
            bool userExists = false;
            foreach (var member in doc.Descendants("Member"))
            {
                if (member.Element("Username").Value == username)
                {
                    userExists = true;
                    break;
                }
            }

            if (userExists)
            {
                lblMessage.Text = "Username already exists. Choose another.";
                return;
            }

            // Add new member
            XElement newMember = new XElement("Member",
                new XElement("Username", username),
                new XElement("Password", hashedPassword)
            );

            doc.Root.Add(newMember);
            doc.Save(filePath);

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Registration successful! Redirecting to Login";
            Response.Redirect("Login.aspx");
        }

        private string HashPassword(string password)
        {
            // Call your DLL hashing function here.
            // For now, simple placeholder:
            return password.GetHashCode().ToString(); // REPLACE with real DLL call!
        }
    }
}

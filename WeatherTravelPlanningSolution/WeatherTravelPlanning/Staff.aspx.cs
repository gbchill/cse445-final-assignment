using System;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml;

namespace WeatherTravelPlanning
{
    public partial class Staff : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check authentication and set role manually
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Extract role from ticket
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null && !ticket.Expired)
                {
                    string role = ticket.UserData;
                    // Set Principal with role
                    string[] roles = { role };
                    GenericPrincipal principal = new GenericPrincipal(User.Identity, roles);
                    HttpContext.Current.User = principal;
                }
                else
                {
                    Response.Redirect("Login.aspx");
                    return;
                }
            }
            else
            {
                lblMessage.Text = "Authentication cookie missing.";
                Response.Redirect("Login.aspx");
                return;
            }

            if (!User.IsInRole("Staff"))
            {
                lblMessage.Text = "Not in Staff role. Username: " + User.Identity.Name;
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                try
                {
                    string xmlPath = Server.MapPath("~/staff.xml");
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlPath);

                    // Display user info
                    string email = User.Identity.Name;
                    foreach (XmlNode node in doc.SelectNodes("//StaffMember"))
                    {
                        if (node.SelectSingleNode("Email")?.InnerText == email)
                        {
                            lblEmailValue.Text = email;
                            lblNameValue.Text = node.SelectSingleNode("Name")?.InnerText ?? "N/A";
                            lblCityValue.Text = node.SelectSingleNode("City")?.InnerText ?? "N/A";
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(lblEmailValue.Text))
                    {
                        lblMessage.Text = "User not found in staff.xml.";
                    }

                    // Load staff.xml content into textbox, HTML-encoded
                    string xmlContent = File.ReadAllText(xmlPath);
                    txtXmlContent.Text = xmlContent;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error loading staff.xml: " + ex.Message;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string xmlPath = Server.MapPath("~/staff.xml");
                string xmlContent = txtXmlContent.Text;

                // Basic XML validation
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);

                // Save to staff.xml
                File.WriteAllText(xmlPath, xmlContent);
                lblMessage.Text = "staff.xml saved successfully.";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error saving staff.xml: " + ex.Message;
            }
        }
    }
}
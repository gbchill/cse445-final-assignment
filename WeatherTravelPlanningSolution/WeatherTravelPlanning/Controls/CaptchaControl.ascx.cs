using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeatherTravelPlanning.Controls
{
    public partial class CaptchaControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateCaptcha();
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }

        private void GenerateCaptcha()
        {
            string captcha = GenerateRandomString(5);
            Session["CaptchaCode"] = captcha;
            lblCaptcha.Text = captcha;
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(stringChars);
        }

        public bool ValidateCaptcha()
        {
            return txtCaptchaInput.Text.Equals(Session["CaptchaCode"]?.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}

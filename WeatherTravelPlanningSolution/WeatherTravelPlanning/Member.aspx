<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Member.aspx.cs" Inherits="WeatherTravelPlanning.Member" %>
<%@ Register Src="~/Controls/CaptchaControl.ascx" TagPrefix="uc" TagName="CaptchaControl" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Member Sign Up</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Member Registration</h2>
        
        Username: <asp:TextBox ID="txtUsername" runat="server" /><br /><br />
        Password: <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" /><br /><br />
        
        <uc:CaptchaControl ID="CaptchaControl1" runat="server" /><br /><br />
        
        <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" /><br /><br />
        
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    </form>
</body>
</html>


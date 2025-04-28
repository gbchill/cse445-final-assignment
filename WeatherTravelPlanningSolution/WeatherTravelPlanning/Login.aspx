<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WeatherTravelPlanning.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login - Weather Travel Planning</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin:50px;">
            <h2>Login</h2>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
            <br />
            <asp:Label ID="lblUsername" runat="server" Text="Username:" AssociatedControlID="txtUsername" />
            <asp:TextBox ID="txtUsername" runat="server" />
            <br /><br />
            <asp:Label ID="lblPassword" runat="server" Text="Password:" AssociatedControlID="txtPassword" />
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
            <br /><br />
            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
            <asp:LinkButton ID="btnRegister" runat="server" OnClick="btnRegister_Click">Register as New Member</asp:LinkButton>
        </div>
    </form>
</body>
</html>

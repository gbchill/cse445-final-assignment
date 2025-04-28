<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Staff.aspx.cs" Inherits="WeatherTravelPlanning.Staff" ValidateRequest="false" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Staff Area</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Staff Area</h2>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
            <br />
            <asp:Label ID="lblEmail" runat="server" Text="Email: " />
            <asp:Label ID="lblEmailValue" runat="server" />
            <br />
            <asp:Label ID="lblName" runat="server" Text="Name: " />
            <asp:Label ID="lblNameValue" runat="server" />
            <br />
            <asp:Label ID="lblCity" runat="server" Text="City: " />
            <asp:Label ID="lblCityValue" runat="server" />
            <br /><br />
            <asp:Label ID="lblXmlContent" runat="server" Text="staff.xml Content:" AssociatedControlID="txtXmlContent" />
            <br />
            <asp:TextBox ID="txtXmlContent" runat="server" TextMode="MultiLine" Rows="10" Columns="50" />
            <br /><br />
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
        </div>
    </form>
</body>
</html>
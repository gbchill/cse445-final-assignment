<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CaptchaControl.ascx.cs" Inherits="WeatherTravelPlanning.Controls.CaptchaControl" %>

<div>
    <asp:Label ID="lblCaptcha" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
    <br />
    <asp:TextBox ID="txtCaptchaInput" runat="server" Width="100px"></asp:TextBox>
    <br />
    <asp:Button ID="btnRefresh" runat="server" Text="Refresh Captcha" OnClick="btnRefresh_Click" />
</div>

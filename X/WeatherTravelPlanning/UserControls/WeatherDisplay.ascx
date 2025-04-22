<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WeatherDisplay.ascx.cs" Inherits="WeatherTravelPlanning.UserControls.WeatherDisplay" %>

<style>
    .weather-container {
        border: 1px solid #ddd;
        border-radius: 8px;
        padding: 15px;
        margin: 10px 0;
        background-color: #f9f9f9;
        max-width: 400px;
    }
    .weather-header {
        font-size: 20px;
        font-weight: bold;
        color: #333;
        margin-bottom: 10px;
    }
    .temperature {
        font-size: 36px;
        font-weight: bold;
        color: #2c3e50;
        margin: 10px 0;
    }
    .conditions {
        font-size: 18px;
        color: #666;
        margin: 5px 0;
    }
    .details {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 10px;
        margin-top: 15px;
        padding-top: 15px;
        border-top: 1px solid #ddd;
    }
    .detail-item {
        text-align: center;
    }
    .detail-label {
        font-size: 14px;
        color: #888;
    }
    .detail-value {
        font-size: 16px;
        font-weight: bold;
        color: #333;
    }
</style>

<div class="weather-container">
    <div class="weather-header">
        <asp:Label ID="lblLocation" runat="server" Text="Weather"></asp:Label>
    </div>
    <div class="temperature">
        <asp:Label ID="lblTemperature" runat="server" Text="--°F"></asp:Label>
    </div>
    <div class="conditions">
        <asp:Label ID="lblConditions" runat="server" Text="--"></asp:Label>
    </div>
    <div class="details">
        <div class="detail-item">
            <div class="detail-label">High/Low</div>
            <div class="detail-value">
                <asp:Label ID="lblHighLow" runat="server" Text="--°/--°"></asp:Label>
            </div>
        </div>
        <div class="detail-item">
            <div class="detail-label">Rain Chance</div>
            <div class="detail-value">
                <asp:Label ID="lblRainChance" runat="server" Text="--%"></asp:Label>
            </div>
        </div>
        <div class="detail-item">
            <div class="detail-label">Wind</div>
            <div class="detail-value">
                <asp:Label ID="lblWind" runat="server" Text="-- mph"></asp:Label>
            </div>
        </div>
        <div class="detail-item">
            <div class="detail-label">Humidity</div>
            <div class="detail-value">
                <asp:Label ID="lblHumidity" runat="server" Text="--%"></asp:Label>
            </div>
        </div>
    </div>
</div>
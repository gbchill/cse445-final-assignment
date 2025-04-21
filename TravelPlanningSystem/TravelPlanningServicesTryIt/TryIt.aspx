<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryIt.aspx.cs" Inherits="TravelPlanningServicesTryIt.TryIt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Travel Planning Services - TryIt Page</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }
        h1 {
            color: #2c3e50;
        }
        h2 {
            color: #3498db;
            margin-top: 30px;
        }
        .service-section {
            border: 1px solid #e0e0e0;
            padding: 15px;
            margin-bottom: 20px;
            border-radius: 5px;
        }
        .input-group {
            margin-bottom: 10px;
        }
        .input-group label {
            display: inline-block;
            width: 150px;
        }
        .result-box {
            margin-top: 10px;
            padding: 10px;
            border: 1px solid #e0e0e0;
            background-color: #f9f9f9;
            min-height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Travel Planning Services - TryIt Page</h1>
            
            <!-- Weather Service Section -->
            <div class="service-section">
                <h2>Weather Service (WSDL)</h2>
                <p>This service provides a 5-day weather forecast for a given US zip code.</p>
                <p>Service URL: <asp:Label ID="lblWeatherServiceUrl" runat="server" Text="http://localhost:yourport/WeatherService.svc"></asp:Label></p>
                <p>Method: GetWeatherForecast(string zipCode) : string</p>
                
                <div class="input-group">
                    <label for="txtZipCode">Zip Code:</label>
                    <asp:TextBox ID="txtZipCode" runat="server" placeholder="Enter 5-digit US zip code"></asp:TextBox>
                </div>
                
                <asp:Button ID="btnGetWeather" runat="server" Text="Get Weather Forecast" OnClick="btnGetWeather_Click" />
                
                <div class="result-box">
                    <asp:Label ID="lblWeatherResult" runat="server" Text=""></asp:Label>
                </div>
            </div>
            
            <!-- Currency Converter Service Section -->
            <div class="service-section">
                <h2>Currency Converter Service (RESTful)</h2>
                <p>This service converts currency amounts between different currencies.</p>
                <p>Service URL: <asp:Label ID="lblCurrencyServiceUrl" runat="server" Text="http://localhost:yourport/CurrencyConverter.svc"></asp:Label></p>
                <p>Method: ConvertCurrency(decimal amount, string fromCurrency, string toCurrency) : decimal</p>
                
                <div class="input-group">
                    <label for="txtAmount">Amount:</label>
                    <asp:TextBox ID="txtAmount" runat="server" placeholder="Enter amount"></asp:TextBox>
                </div>
                
                <div class="input-group">
                    <label for="ddlFromCurrency">From Currency:</label>
                    <asp:DropDownList ID="ddlFromCurrency" runat="server">
                        <asp:ListItem Text="USD - US Dollar" Value="USD"></asp:ListItem>
                        <asp:ListItem Text="EUR - Euro" Value="EUR"></asp:ListItem>
                        <asp:ListItem Text="GBP - British Pound" Value="GBP"></asp:ListItem>
                        <asp:ListItem Text="JPY - Japanese Yen" Value="JPY"></asp:ListItem>
                        <asp:ListItem Text="CAD - Canadian Dollar" Value="CAD"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="input-group">
                    <label for="ddlToCurrency">To Currency:</label>
                    <asp:DropDownList ID="ddlToCurrency" runat="server">
                        <asp:ListItem Text="USD - US Dollar" Value="USD"></asp:ListItem>
                        <asp:ListItem Text="EUR - Euro" Value="EUR"></asp:ListItem>
                        <asp:ListItem Text="GBP - British Pound" Value="GBP"></asp:ListItem>
                        <asp:ListItem Text="JPY - Japanese Yen" Value="JPY"></asp:ListItem>
                        <asp:ListItem Text="CAD - Canadian Dollar" Value="CAD"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <asp:Button ID="btnConvertCurrency" runat="server" Text="Convert Currency" OnClick="btnConvertCurrency_Click" />
                
                <div class="result-box">
                    <asp:Label ID="lblCurrencyResult" runat="server" Text=""></asp:Label>
                </div>
            </div>
            
            <!-- Word Filter Service Section -->
            <div class="service-section">
                <h2>Word Filter Service</h2>
                <p>This service filters out common stop words from text and provides content analysis.</p>
                <p>Service URL: <asp:Label ID="lblWordFilterServiceUrl" runat="server" Text="http://localhost:yourport/WordFilter.svc"></asp:Label></p>
                <p>Methods:</p>
                <ul>
                    <li>FilterStopWords(string inputText) : string</li>
                    <li>GetTopContentWords(string inputText, int count) : string[]</li>
                    <li>CountContentWords(string inputText) : int</li>
                </ul>
                
                <div class="input-group">
                    <label for="txtInputText">Input Text:</label>
                    <asp:TextBox ID="txtInputText" runat="server" TextMode="MultiLine" Rows="5" Columns="50" placeholder="Enter text to analyze"></asp:TextBox>
                </div>
                
                <div class="input-group">
                    <label for="txtTopWordCount">Number of Top Words:</label>
                    <asp:TextBox ID="txtTopWordCount" runat="server" Text="5"></asp:TextBox>
                </div>
                
                <asp:Button ID="btnFilterStopWords" runat="server" Text="Filter Stop Words" OnClick="btnFilterStopWords_Click" />
                <asp:Button ID="btnGetTopWords" runat="server" Text="Get Top Words" OnClick="btnGetTopWords_Click" />
                <asp:Button ID="btnCountWords" runat="server" Text="Count Content Words" OnClick="btnCountWords_Click" />
                
                <div class="result-box">
                    <asp:Label ID="lblWordFilterResult" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
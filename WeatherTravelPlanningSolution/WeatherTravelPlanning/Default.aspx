<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WeatherTravelPlanning.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Weather Travel Planning System</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        .service-table { border-collapse: collapse; width: 100%; margin-top: 20px; }
        .service-table th, .service-table td { border: 1px solid #ddd; padding: 12px; text-align: left; }
        .service-table th { background-color: #4CAF50; color: white; }
        .service-table tr:nth-child(even) { background-color: #f2f2f2; }
        .service-table tr:hover { background-color: #ddd; }
        h1, h2 { color: #2c3e50; }
        .button-panel { margin-top: 20px; }
        .try-it-panel { margin-top: 20px; border: 1px solid #ddd; padding: 15px; background-color: #f9f9f9; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Weather Travel Planning System</h1>
            <p>Welcome to our weather-based travel planning system. This application helps you plan your travels by providing accurate weather information and recommendations.</p>
            
            <h2>Service Directory</h2>
            <table class="service-table">
                <tr>
                    <th>Provider Name</th>
                    <th>Component Type</th>
                    <th>Operation Name</th>
                    <th>Parameters</th>
                    <th>Return Type</th>
                    <th>Description</th>
                    <th>Try It</th>
                </tr>
                <!-- Web Services -->
                <tr>
                    <td>Team Member 1</td>
                    <td>WCF Service</td>
                    <td>GetWeatherForecast</td>
                    <td>zipCode (string)</td>
                    <td>WeatherData[]</td>
                    <td>Returns 7-day weather forecast for given ZIP</td>
                    <td><asp:Button ID="btnTryForecast" runat="server" Text="Try It" OnClick="btnTryForecast_Click" /></td>
                </tr>
                <tr>
                    <td>Team Member 1</td>
                    <td>WCF Service</td>
                    <td>ConvertTemperature</td>
                    <td>temp (double), fromUnit (string), toUnit (string)</td>
                    <td>double</td>
                    <td>Converts between Fahrenheit and Celsius</td>
                    <td><asp:Button ID="btnTryConverter" runat="server" Text="Try It" OnClick="btnTryConverter_Click" /></td>
                </tr>
                <tr>
                    <td>Team Member 2</td>
                    <td>WCF Service</td>
                    <td>GetRainyDayAdvice</td>
                    <td>zipCode (string)</td>
                    <td>string[]</td>
                    <td>Returns today’s rainy‑day suggestions</td>
                    <td><asp:Button ID="btnTryRainy" runat="server" Text="Try It" OnClick="btnTryRainy_Click" /></td>
                </tr>
                <!-- Local Components -->
                <tr>
                    <td>Team Member 1</td>
                    <td>DLL</td>
                    <td>EncryptUserData</td>
                    <td>data (string)</td>
                    <td>string</td>
                    <td>Encrypts sensitive user data using AES</td>
                    <td><asp:Button ID="btnTryEncryption" runat="server" Text="Try It" OnClick="btnTryEncryption_Click" /></td>
                </tr>
                <tr>
                    <td>Team Member 2</td>
                    <td>User Control</td>
                    <td>WeatherDisplay</td>
                    <td>weatherData (WeatherData)</td>
                    <td>void</td>
                    <td>Displays weather in formatted view</td>
                    <td><asp:Button ID="btnTryControl" runat="server" Text="Try It" OnClick="btnTryControl_Click" /></td>
                </tr>
                <tr>
                    <td>Team Member 1</td>
                    <td>Global.asax</td>
                    <td>Application_Start</td>
                    <td>–</td>
                    <td>void</td>
                    <td>Initializes weather cache at app start</td>
                    <td>N/A</td>
                </tr>
                <tr>
                    <td>Team Member 2</td>
                    <td>Cookie</td>
                    <td>StoreUserPreferences</td>
                    <td>preferences (UserPrefs)</td>
                    <td>void</td>
                    <td>Stores temp units &amp; location</td>
                    <td><asp:Button ID="btnTryCookie" runat="server" Text="Try It" OnClick="btnTryCookie_Click" /></td>
                </tr>
            </table>
            
            <h2>Try It Section</h2>
            <div class="try-it-panel">
                <asp:MultiView ID="mvTryIt" runat="server" ActiveViewIndex="0">
                    <!-- View 0: Default -->
                    <asp:View ID="vwDefault" runat="server">
                        <p>Click a “Try It” button above to test each component.</p>
                    </asp:View>
                    
                    <!-- View 1: Weather Forecast -->
                    <asp:View ID="vwForecast" runat="server">
                        <h3>7‑Day Weather Forecast</h3>
                        <label>Enter ZIP Code:</label>
                        <asp:TextBox ID="txtZipCode" runat="server" MaxLength="5" />
                        <asp:Button ID="btnGetForecast" runat="server" Text="Get Forecast" OnClick="btnGetForecast_Click" />
                        <br /><br />
                        <asp:Label ID="lblForecastResult" runat="server" />
                    </asp:View>
                    
                    <!-- View 2: Temperature Converter -->
                    <asp:View ID="vwConverter" runat="server">
                        <h3>Temperature Converter by ZIP</h3>
                        <p>Enter a ZIP code to fetch today’s temperature and convert:</p>
                        <label>ZIP Code:</label>
                        <asp:TextBox ID="txtConverterZipCode" runat="server" MaxLength="5" />
                        <asp:Button ID="btnConvert" runat="server" Text="Get Temperature" OnClick="btnConvert_Click" />
                        <br /><br />
                        <asp:Label ID="lblConverterResult" runat="server" />
                        <br />
                        <asp:Button ID="btnToggleTemp" runat="server" Text="Toggle °F/°C" OnClick="btnToggleTemp_Click" Visible="false" />
                    </asp:View>
                    
                    <!-- View 3: Rainy Day Advisor -->
                    <asp:View ID="vwRainy" runat="server">
                        <h3>Rainy Day Activity Advisor (Today)</h3>
                        <label>Enter ZIP Code:</label>
                        <asp:TextBox ID="txtRainLocation" runat="server" MaxLength="5" />
                        <asp:Button ID="btnGetAdvice" runat="server" Text="Get Advice" OnClick="btnGetAdvice_Click" />
                        <br /><br />
                        <asp:Label ID="lblRainyResult" runat="server" />
                    </asp:View>
                    
                    <!-- View 4: Encryption -->
                    <asp:View ID="vwEncryption" runat="server">
                        <h3>Data Encryption Test</h3>
                        <label>Enter text to encrypt:</label>
                        <asp:TextBox ID="txtPlainText" runat="server" />
                        <asp:Button ID="btnEncrypt" runat="server" Text="Encrypt" OnClick="btnEncrypt_Click" />
                        <br /><br />
                        <label>Encrypted text:</label> <asp:Label ID="lblEncrypted" runat="server" />
                        <br />
                        <asp:Button ID="btnDecrypt" runat="server" Text="Decrypt" OnClick="btnDecrypt_Click" />
                        <br />
                        <label>Decrypted text:</label> <asp:Label ID="lblDecrypted" runat="server" />
                    </asp:View>
                    
                    <!-- View 5: User Control -->
                    <asp:View ID="vwUserControl" runat="server">
                        <h3>Weather Display Control Test</h3>
                        <!-- your <uc:WeatherDisplay …/> here -->
                    </asp:View>
                    
                    <!-- View 6: Cookie -->
                    <asp:View ID="vwCookie" runat="server">
                        <h3>User Preferences Cookie Test</h3>
                        <label>Temperature Unit:</label>
                        <asp:DropDownList ID="ddlTempUnit" runat="server">
                            <asp:ListItem Value="F">Fahrenheit</asp:ListItem>
                            <asp:ListItem Value="C">Celsius</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <label>Location ZIP:</label>
                        <asp:TextBox ID="txtPrefLocation" runat="server" MaxLength="5" />
                        <br />
                        <asp:Button ID="btnSavePreferences" runat="server" Text="Save Preferences" OnClick="btnSavePreferences_Click" />
                        <asp:Button ID="btnLoadPreferences" runat="server" Text="Load Preferences" OnClick="btnLoadPreferences_Click" />
                        <br /><br />
                        <asp:Label ID="lblCookieResult" runat="server" />
                    </asp:View>
                </asp:MultiView>
            </div>
            
            <div class="button-panel">
                <h3>Access Control (Assignment 6)</h3>
                <asp:Button ID="btnMember" runat="server" Text="Member Page" Enabled="false" />
                <asp:Button ID="btnStaff" runat="server" Text="Staff Page" Enabled="false" />
            </div>
        </div>
    </form>
</body>
</html>

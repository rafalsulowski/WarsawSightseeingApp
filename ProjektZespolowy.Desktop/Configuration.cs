using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using ProjektZespolowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ProjektZespolowy.Models.DTO;
using System.Net.Http;

namespace ProjektZespolowy.Desktop
{
    public static class Configuration
    {
        public enum SystemTheme
        {
            Dark,
            Light
        }

        static Configuration()
        {
            m_MenuVisible = Visibility.Hidden;
            m_SystemTheme = SystemTheme.Light;
            m_ActualLoggerdUser = null;

            EngineOptions engineOptions = new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.HardwareAccelerated,
                LicenseKey = "6P91WMFPXDAK5WJ5A919K4PPEIK5JHR3UP5L66FR8RXHDBM1F7NM76QHYY4GCV9J6W4K"
            }.Build();
            engine = EngineFactory.Create(engineOptions);
            browser = engine.CreateBrowser();

            m_Client = new HttpClient();

        }

        //System settings
        public static readonly IBrowser browser;
        public static readonly IEngine engine;
        public static Visibility m_MenuVisible { get; set; }
        public static SystemTheme m_SystemTheme { get; set; }
        public static User? m_ActualLoggerdUser { get; set; } = null;
        public static string m_JwtToken { get; set; }

        public static HttpClient m_Client { get; set; }


        public static string encodingUrlChracter(string url)
        {
            url = url.Replace("%", "%25");
            url = url.Replace(" ", "%20");
            url = url.Replace("\"", "%22");
            url = url.Replace("<", "%3C");
            url = url.Replace(">", "%3E");
            url = url.Replace("#", "%23");
            url = url.Replace("|", "+");    //niedozwolony ze wzelgu na to ze '|' rodziela waypoints'y w url dla google maps
            return url.Replace(",", "%2C");
        }


        public static string UrlBuilder(List<PlaceDTO> places, TransportType transportType)
        {
            string path = "https://www.google.com/maps/";
            if (places != null && places.Count > 0)
            {
                path = "https://www.google.com/maps/dir/?api=1&origin=";
                string placeUri = places[0].Address;
                placeUri = encodingUrlChracter(placeUri);
                path += placeUri;

                if (places.Count > 2)
                {
                    path += "&waypoints=";
                    for (int i = 1; i < places.Count - 1; i++)
                    {
                        placeUri = "";
                        placeUri = places[i].Address;
                        placeUri = encodingUrlChracter(placeUri);

                        if (i < places.Count - 2)
                            placeUri += "%7C";

                        path += placeUri;
                    }
                }

                placeUri = "";
                placeUri = "&destination=" + places[places.Count - 1].Address;
                placeUri = encodingUrlChracter(placeUri);
                path += placeUri;

                placeUri = "";
                placeUri = "&travelmode=";
                switch (transportType)
                {
                    case TransportType.Car:
                        placeUri += "driving";
                        break;
                    case TransportType.Bike:
                        placeUri += "bicycling";
                        break;
                    case TransportType.Onfoot:
                        placeUri += "walking";
                        break;
                    case TransportType.PublicTransport:
                        placeUri += "transit";
                        break;
                    default:
                        placeUri += "driving";
                        break;
                }

                path += placeUri;
            }
            return path;
        }
    }
}

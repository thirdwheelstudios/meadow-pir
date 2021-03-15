using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;

namespace Thirdwheel.PassiveInfraRed
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        private IDigitalInputPort _pirSensor;

        public MeadowApp()
        {
            Initialize();
        }

        private void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            _pirSensor = Device.CreateDigitalInputPort(Device.Pins.D02, InterruptMode.EdgeBoth, ResistorMode.Disabled, 20d);
            _pirSensor.Changed += OnSensorChanged;

        }

        private static async void OnSensorChanged(object sender, DigitalInputPortEventArgs e)
        {
            if (e.Value)
            {
                Console.WriteLine("\nMotion started");

                await SendMotionSensedAsync();
            }
        }

        private static async Task SendMotionSensedAsync()
        {
            Console.Write("\nInitialize WiFi Adapter...");

            var isInitialized = await Device.InitWiFiAdapter();
            Console.Write(isInitialized ? "Success" : "Failed");

            if (!isInitialized) return;

            Console.Write("\nConnect to WiFi Network...");

            var connectionResult = Device.WiFiAdapter.Connect(Secrets.NetworkSSID, Secrets.NetworkPassword);
            Console.Write(connectionResult.ConnectionStatus);

            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
                return;

            const string apiUrl = "http://192.168.1.8:4321/motionsensor";

            try
            {
                using (var httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(5) })
                using (var content = new StringContent("{\"name\":\"Shed - Interior\"}", Encoding.UTF8, "application/json"))
                {
                    Console.Write("\nSending Motion Sensed...");

                    var postResponse = await httpClient.PostAsync(apiUrl, content);
                    Console.Write(postResponse.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\nEXCEPTION DURING SEND --> {ex}");
            }
            
        }

    }
}

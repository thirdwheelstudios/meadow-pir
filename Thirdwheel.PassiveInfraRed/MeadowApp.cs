using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
            Console.WriteLine($"Motion {(e.Value ? "started" : "ended")}");

            if (e.Value) await SendMotionSensedAsync();
        }

        private static async Task SendMotionSensedAsync()
        {
            Console.Write("\nInitialize WiFi Adapter...");

            var isInitialized = await Device.InitWiFiAdapter();
            Console.Write(isInitialized ? "SUCCESS" : "FAILED");

            if (!isInitialized) return;

            Console.Write("\nConnect to WiFi Network...");

            const string ssid = "SomeSSID";
            const string password = "SomePassword";

            var result = Device.WiFiAdapter.Connect(ssid, password);
            Console.Write(result.ConnectionStatus);

            


        }

    }
}

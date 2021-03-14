using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Meadow.Hardware;

namespace Thirdwheel.PassiveInfraRed
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        private IDigitalInputPort _pirSensor;

        public MeadowApp()
        {
            Initialize();

            Console.WriteLine("Starting app loop...");
            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        private void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            _pirSensor = Device.CreateDigitalInputPort(Device.Pins.D02, InterruptMode.EdgeBoth, ResistorMode.Disabled, 20d);
            _pirSensor.Changed += OnSensorChanged;
        }

        private static void OnSensorChanged(object sender, DigitalInputPortEventArgs e)
        {
            Console.WriteLine($"Motion {(e.Value ? "started" : "ended")}");
        }

    }
}

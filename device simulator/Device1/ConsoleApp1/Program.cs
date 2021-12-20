using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        private static DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=rainWaterLevel.azure-devices.net;DeviceId=device1;SharedAccessKey=2KVe5sbRFSreMGrYGiS5O52/VpVCsjED/AfjBWUE24U=", TransportType.Mqtt);

        static async Task Main(string[] args)
        {
            int tanklevel = 125;
            bool pumping = false;
            int rainWaterTankLevel=25;
            while (true)
            {
                Random rd = new Random();
                int rand_num = rd.Next(0,10);
                int rand_num_rain = rd.Next(0, 4);
                if (pumping == false)
                {
                    if (tanklevel > 25)
                    {
                        tanklevel = tanklevel - rand_num;
                    }
                    else
                    {
                        pumping = true;
                    }
                    rainWaterTankLevel = rainWaterTankLevel + rand_num_rain;
                }
                else
                {
                    rainWaterTankLevel = rainWaterTankLevel - 1 + rand_num_rain;
                    if (rainWaterTankLevel > 0)
                    {
                        if (tanklevel <= 115)
                        {
                            tanklevel = tanklevel + 10;
                        }
                        else
                            pumping = false;
                    }
                    else
                        rainWaterTankLevel = 0;
                }
                long epochTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                await SendMessageAsync(JsonConvert.SerializeObject(new { deviceId = "consoleDevice", rainWaterTankLevel = rainWaterTankLevel, OverheadTankLevel = tanklevel,epochtime= epochTime }));
                await Task.Delay(60000);                
                Console.WriteLine($"deviceId = consoleDevice, rainWaterTankLevel = {rainWaterTankLevel}, OverheadTankLevel = {tanklevel},epochtime={epochTime}");
            }
        }
        private static async Task SendMessageAsync(string message)
        {
            var msg = new Message(Encoding.UTF8.GetBytes(message));            
            msg.Properties["latitude"] = "10.005034";
            msg.Properties["longitude"] = "76.692141";
            await deviceClient.SendEventAsync(msg);
        }
    }
}


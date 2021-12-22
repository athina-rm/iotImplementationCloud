using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        private static string dpsIdScope = "0ne00465B52";

        private static string registrationId = "device-01";            
           
        private const string indEnrollPriKey = "GbHpPo2cr8i7W1S6Zivchwt+n/SasafuYhamjzWewWuacU6ZOyzs5lg4j++ZTj/WJly+IuUvDVUzC99EXgYsSQ==";

        private const string indEnrollSecKey = "egKZ9D+8ikuHJd9srMa19Y6mY3evgr7pgVc/4qQofNiljrDqj+Oz6sYBGOinLCJeOEskP5iZq1puIQbyzS0Lcw==";        

        private const string globalDeviceEndpoint = "global.azure-devices-provisioning.net";
        public static DeviceClient deviceClient;

    static async Task Main(string[] args)
        {
            if (string.IsNullOrWhiteSpace(dpsIdScope) && (args.Length > 0))
            {
                dpsIdScope = args[0];
            }

            if (string.IsNullOrWhiteSpace(dpsIdScope))
            {
                Console.WriteLine("ProvisioningDeviceClientSymmetricKey <IDScope> <registrationID>");
            }

            if (string.IsNullOrWhiteSpace(registrationId) && (args.Length > 1))
            {
                registrationId = args[1];
            }

            if (string.IsNullOrWhiteSpace(registrationId))
            {
                Console.WriteLine("ProvisioningDeviceClientSymmetricKey <IDScope> <registrationID>");
            }

            string primaryKey = "";
            string secondaryKey = "";
            if (!String.IsNullOrEmpty(registrationId) && !String.IsNullOrEmpty(indEnrollPriKey) && !String.IsNullOrEmpty(indEnrollSecKey))
            {
                //Individual enrollment flow, the primary and secondary keys are the same as the individual enrollment keys
                primaryKey = indEnrollPriKey;
                secondaryKey = indEnrollSecKey;
            }
            else
            {
                Console.WriteLine("Invalid configuration provided, must provide group enrollment keys or individual enrollment keys");
            }
            using (var security = new SecurityProviderSymmetricKey(registrationId, primaryKey, secondaryKey))
            using (var transport = new ProvisioningTransportHandlerMqtt(TransportFallbackType.WebSocketOnly))
            {
                ProvisioningDeviceClient provClient = ProvisioningDeviceClient.Create(globalDeviceEndpoint, dpsIdScope, security, transport);
                DeviceRegistrationResult result = await provClient.RegisterAsync();
                if (result.Status != ProvisioningRegistrationStatusType.Assigned)
                {
                    Console.WriteLine($"ProvisioningClient AssignedHub: {result.AssignedHub}; DeviceId: {result.DeviceId}");
                }
                var auth = new DeviceAuthenticationWithRegistrySymmetricKey(result.DeviceId, security.GetPrimaryKey());
                deviceClient = DeviceClient.Create(result.AssignedHub, auth, TransportType.Mqtt);
            }         
            
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
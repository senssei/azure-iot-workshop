using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SampleModule
{
    class IoT_Device
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("deviceProperties")]
        public List<DeviceProperty> DeviceProperties { get; set; }

        [JsonProperty("deviceVariable")]
        DeviceVariable DeviceVariable { get; set; }

        public IoT_Device GetDeviceData()
        {
            //Building our device
            //Create list of properties
            Random rand = new Random();
            List<DeviceProperty> properties = new List<DeviceProperty>();
            DeviceProperty a = new DeviceProperty();
            a.Name = "owner";
            a.Value = "Rafal Warzycha";
            properties.Add(a);

            DeviceProperty b = new DeviceProperty();
            b.Name = "manufactured by";
            b.Value = "ABB";
            properties.Add(b);

            DeviceProperty g = new DeviceProperty();
            g.Name = "inspired by";
            g.Value = "Ewelina";
            properties.Add(g);

            //Create a list of variables
            DeviceVariable c = new DeviceVariable();
            c.Name = "temperature";
            c.Value = rand.Next(25, 35).ToString();
            c.Timestamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            DeviceVariable d = new DeviceVariable();
            d.Name = "torque";
            d.Value = rand.Next(500, 1500).ToString();
            d.Timestamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");


            //Create a device

            IoT_Device deviceData = new IoT_Device();
            deviceData.DeviceId = Guid.NewGuid().ToString();
            deviceData.DeviceProperties = properties;
            if(rand.Next(0,100)>50)
                deviceData.DeviceVariable = c;
            else
                deviceData.DeviceVariable = d;

            return deviceData;
        }
    }

    class DeviceProperty
    {
        [JsonProperty("propertyName")]
        public string Name { get; set; }

        [JsonProperty("propertyValue")]
        public string Value { get; set; }
    }

    class DeviceVariable
    {
        [JsonProperty("variableName")]
        public string Name { get; set; }

        [JsonProperty("variableValue")]
        public string Value { get; set; }

        [JsonProperty("variableTimestamp")]
        public string Timestamp { get; set; }
    }
}

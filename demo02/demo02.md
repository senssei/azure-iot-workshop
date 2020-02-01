# 

## Prerequisites

* Visual Studio Code
* .NET Core SDK
* IoT Edge Tools for Visual Studio Code
* Azure container Registry
* IoT Edge device (simulator, VM, phisical)


## Setup tools

1. In the command palette, search for and select **Azure: Sign in**. Follow the prompts to sign in to your Azure account.

2. In the command palette again, search for and select **Azure IoT Hub: Select IoT Hub**. Follow the prompts to select your Azure subscription and IoT hub.

3. Open the explorer section of Visual Studio Code by either selecting the icon in the activity bar on the left, or by selecting **View** > **Explorer**.

4. At the bottom of the explorer section, expand the collapsed **Azure IoT Hub Devices** menu. You should see the devices and IoT Edge devices associated with the IoT hub that you selected through the command palette.

## Provide your registry credentials to the IoT Edge agent

The environment file stores the credentials for your container registry and shares them with the IoT Edge runtime. The runtime needs these credentials to pull your container images onto the IoT Edge device.

The IoT Edge extension tries to pull your container registry credentials from Azure and populate them in the environment file. Check to see if your credentials are already included. If not, add them now:

1. Create the new file **.env** file in your module solution based on the **.env_sample** - you can rename it.
2. Add the username, password and url values that you copied from your Azure container registry.
3. Save your changes to the **.env** file.

## Build and push your solution

### Login to docker registry

> Before first login rename property **repository** in `demo02\src\EdgeSolution\modules\SampleModule\module.json` to
correct image tag - f.e.  from `iotedgeimages.azurecr.io/samplemodule` to `<ACR login server>/samplemodule`

```bash
docker login -u <ACR username> -p <ACR password> <ACR login server>
```

### Build and push IoT Edge Solution

1. In the Visual Studio Code explorer, right-click the **deployment.template.json** file and select **Build and Push IoT Edge Solution**.

2. Open the **deployment.amd64.json** file in newly created **config** folder. The filename reflects the target architecture, so it will be different if you chose a different architecture.

After each small changes you can open the **module.json** file in the SampleModule folder. Change the version number for the module image. (The version, not the $schema-version.) 
For example, increment the patch version number to 0.0.2 as though we had made a small fix in the module code.

3. In the Visual Studio Code explorer, expand the **Azure IoT Hub Devices** section.

4. Right-click the **IoT Edge device** that you want to deploy to, then select **Create Deployment for Single Device**.

5. In the file explorer, navigate into the **config** folder then select the **deployment.amd64.json** file.

6. Expand the details for your **IoT Edge device**, then expand the Modules list for your device.

7. Use the refresh button to update the device view until you see the **SampleModule** module running on your device.

### View messages from device

1. In the Visual Studio Code explorer, right-click the IoT Edge device that you want to monitor, then select **Start Monitoring Built-in Event Endpoint**.

2. Watch the output window in Visual Studio Code to see messages arriving at your IoT hub.

## Sources

* https://docs.microsoft.com/en-us/azure/iot-edge/tutorial-develop-for-linux
* https://docs.microsoft.com/en-us/azure/iot-edge/offline-capabilities
* https://docs.microsoft.com/en-us/azure/iot-edge/support 
* https://docs.microsoft.com/en-us/azure/iot-edge/quickstart-linux



# # IoT Emulator

The aim of this project is to create an easy interface to control several IoT devices.

In the following image you can see the main form:


[![Emulator](https://github.com/iD365FOnt/IoTEmulator/blob/master/interface.png "Emulator")](https://github.com/iD365FOnt/IoTEmulator/blob/master/interface.png "Emulator")

In order to make it work you have to:
1. Replace the name of each device for the sensor name you want to send to the Device Hub in Azure.
2. Setup the time between each data sending by filling the box with the value in milliseconds
3. Setup each device data value to be sent to Azure
4. Enable the devices you want to send to Azure by checking the checkbox associated
4. Open app.config and fill the values of the Azure IoT resource

`<add key="deviceKey" value="" />`

`<add key="deviceId" value="" />`

`<add key="iotHubHostName" value="**Azure IoT Hub host name**" />`

[![IoT device](https://github.com/iD365FOnt/IoTEmulator/blob/master/iotdevice.png "IoT device")](https://github.com/iD365FOnt/IoTEmulator/blob/master/iotdevice.png "IoT device")

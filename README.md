# iotImplementationCloud
This projects basic goal is to implement a basic IoT architecture using any of the cloud platforms.
## Use case:
### Water level monitoring at home watertanks.
Most of the homes in India where  I come from has wells or underground water reservoirs or rain harvesting tanks that are the water source or bigger reservoir to store water. Water is pumped from these reservoirs to overhead tanks using waterpump so that desirable water pressure can be achieved in the faucets. Since the water pump motor is controlled manually people turn off the motor when the tank overflows which leads to wastage of both electricity and water. Also in cases where the multiple households share the same water tank and one individual has the duty to control it, that person has to be home always to make sure that other people get unhindered water supply. So my solution is to install a water level monitoring system in the water tank and measure and share the water level in realtime to a database and display the information on a webpage. A notification can be sent to the user when the water level gets too low to 20 percent. By installing a smartswitch to control the motor the user can turn the motor ON and OFF from anywhere in the world. The water level data can be used further to monitor the water use in the household and thereby avoid water wastage if there is any. <br>
Data Collection :
  The water level will be measured by using an ultrasonic sensor that measures the distance to an object by transmitting and receiving ultrasonic sound waves. Ideally data should be collected from a real world setup. Since that’s impossible in my case, I will either build a small prototype and measure the data or simulate the data with a program for the execution of the project. Since the project requirement asks for external API data, I collect rain data at the location from https://www.weatherapi.com/.<br>
  The target group for the project are home users who are interested in automation of water tanks and will monitor the water usage.
 ![System_sketch drawio](https://user-images.githubusercontent.com/71870874/147053453-d26ecdf2-13c7-4b27-9740-4b276a88a7ed.png)

### Implementation:
Since Azure seems to be most popular platform in the Nordic so I decided to go with Azure and tried to implement everything using Azure. I used this opportunity to deepen my knowledge on Azure. The project was cut down from initial concept to adjust for time limitation. For example, the bidirectional communication, cold storage for data hasn’t been implemented. 
#### Device :
The device in the current project is a simulated device done in C# that sends the waterlevel in OverheadTank, Rainwater harvesting tank and the measured time to the IotHub. The data is sent once in  a minute using MQTT protocol. In production it would be better to have an edge device that can collect the data and automate the water pumping to the overhead tank. The code simulation currently does the pumping if the water goes below a minimum level.
#### Device Provisioning :
Azure Device provisioning service was used to identify a device when it is initially connected to the iothub. It avoids the need for hardcoding the connection string to each device and thus is more secure. Using a device provisioning service allows even removing an individual devices connection to the cloud if necessary. This gives flexibility while scaling the implementation and minimise vulnerability during production. 
IoTHub was created and linked to the device provisioning service. I used individual enrollment  and symmetric key authentication mechanism which is alright for testing. But in a production level, group enrollment is suggested. I haven’t setup a device twin in my project. But having a digital twin allows for future firmware updates for connected devices.
#### IotHub:
It is the suggested platform for iot devices due to the bidirectional capabilities and the device level identity and management. I chose Mqtt protocol to send the messages over since its lightweight and low latency and that’s most commonly used in iot scenario. Amqp could also be considered in case of using an Edge device. I chose to let the data go to built-in endpoint since this was just a prototype.
#### EventHubTrigger Azure Function
Serverless Azure functions are used in the project. I used an eventhub trigger to run the azure function to get the data every time new data is received in the iothub and transform it and save it to Cosmos DB. 
#### TimeTrigger Azure Function
This is used to collect the data from external API every 5 minutes. The data is transformed and saved to Cosmos DB
I havent used any authorisation level for simplicity during development. But a function key is suggested for production environment for additional security. Azure functions were developed in Python.
#### Cosmos DB
I opted for Cosmos DB in this project for its ease of use. Since it is a noSql document style database, data transformation process was easier. I chose Core (SQL) API since its easy to retrieve data using query and has the best support.
#### PowerBI
Data from cosmosDB was collected in PowerBI for visualisation.
<img width="897" alt="PowerBi Visual" src="https://user-images.githubusercontent.com/71870874/147052759-931ca1f8-886f-471c-b436-e2c5cfda66f5.png">
#### HttpTrigger Azure Function
This is used as a serverless API to display the latest data on the website.  I haven’t used API key during development. Only the preferres domain can access the data due to CORS policy. But Function key or API management is suggested for production environment more include more security. 
#### Azure Static Web Apps
I chose Static webapp for visualisation due to time and knowledge constraints. Azure Static Web Apps offers continuous integration /continuous development which was ideal for me since I am no expert in Javascript or HTML. It allowed a fully automated development process with github. If I had more time I would have explored the option to implement realtime webapp by implementing websocket with Azure Web PubSub or Azure Webapps. The Webpage was hosted in domain  https://wonderful-bush-0de9e9003.azurestaticapps.net and the SSL certificate was managed by Azure itself.<br>
Visual Studio Code IDE, Visual Studio IDE, GitHub and PowerBI were used during the development process.



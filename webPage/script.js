let overhead = document.getElementById('overhead')

fetch("https://gethubandapidata.azurewebsites.net/api/HttpTrigger1")
.then((response) => {
        return response.json();
      })
      .then((myJson) => {
                overhead.innerHTML += myJson[0].OverheadTankLevel;
                document.getElementById('rainWater').innerHTML += myJson[0].rainWaterTankLevel;
                console.log (myJson);
      }) ; 
fetch("http://api.weatherapi.com/v1/current.json?key=3cb4286283984cdcbdf61123211412&q=10.005034,76.692141")
.then((response) => {
        return response.json();
      })
      .then((myJson) => {                
                document.getElementById('apirain').innerHTML += myJson.current.precip_mm;                
      }) ;  

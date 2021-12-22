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
fetch("https://gethubandapidata.azurewebsites.net/api/HttpTrigger2")
.then((response) => {
        return response.json();
      })
      .then((myJson) => {  
                console.log (myJson);              
                document.getElementById('apirain').innerHTML += myJson[0].precip_mm;
                document.getElementById('updated').innerHTML += myJson[0].last_updated;
                document.getElementById('location').innerHTML += myJson[0].location;                             
      }) ;      
 

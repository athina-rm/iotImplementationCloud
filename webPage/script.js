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

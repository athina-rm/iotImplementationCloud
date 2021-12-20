import datetime
import logging
from pip._vendor import requests
import json

import azure.functions as func


def main(mytimer: func.TimerRequest, doc: func.Out[func.Document]) -> None:
    utc_timestamp = datetime.datetime.utcnow().replace(
        tzinfo=datetime.timezone.utc).isoformat()

    if mytimer.past_due:
        logging.info('The timer is past due!')
        

    logging.info('Python timer trigger function ran at %s', utc_timestamp)
    response_API = requests.get('http://api.weatherapi.com/v1/current.json?key=3cb4286283984cdcbdf61123211412&q=10.005034,76.692141')
    print(response_API.status_code)
    data = response_API.text
    parse_json=json.loads(data)
    locality = parse_json['location']
    current_data = parse_json['current']  
    print("Area:", locality)
    #rain = parse_json['current']['precip_mm']
    doc.set(func.Document.from_json(data))
    

import logging
import json
import time

import azure.functions as func


def main(req: func.HttpRequest,doc: func.DocumentList) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')
    data = []  
    for docs in doc:
        d={"rainWaterTankLevel": docs['rainWaterTankLevel'],
           "OverheadTankLevel": docs['OverheadTankLevel'],           
           "time": time.strftime('%Y-%m-%d %H:%M:%S', time.localtime(docs['epochtime'])),}
        data.append(d)
    return func.HttpResponse(body = json.dumps(data), status_code = 200, mimetype="application/json")
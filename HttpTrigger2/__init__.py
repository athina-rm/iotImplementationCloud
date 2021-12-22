import logging
import json
import time

import azure.functions as func


def main(req: func.HttpRequest,doc: func.DocumentList) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')
    data = []  
    for docs in doc:
        d={"location": [docs['location']['lat'],docs['location']['lon']],
           "precip_mm": docs['current']['precip_mm'],
           "last_updated": docs['current']['last_updated'],}
        data.append(d)
    return func.HttpResponse(body = json.dumps(data), status_code = 200, mimetype="application/json")

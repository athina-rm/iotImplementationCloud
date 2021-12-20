from typing import List
import logging
import json

import azure.functions as func


def main(events: List[func.EventHubEvent], doc: func.Out[func.Document])->None :
    for event in events:
        logging.info('Python EventHub trigger processed an event: %s',
                        event.get_body().decode('utf-8'))
        message=event.get_body()        
        properties=events[0].metadata['PropertiesArray']
        msg = json.loads(message)        
        msg.update({"Properties" : properties})
        print(msg)                  
        doc.set(func.Document(msg))

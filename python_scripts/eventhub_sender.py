import asyncio
from azure.eventhub.aio import EventHubProducerClient
from azure.eventhub import EventData

async def run():
    ehub_send_conn_str="Endpoint=sb://ychen32-ehub.servicebus.windows.net/;SharedAccessKeyName=sendToMyEHub;SharedAccessKey=Tdsy5ukPcT7nIHvoOBFBbiwrZZBJyfE7SW0tl1K0hTo=;EntityPath=myeventhub"
    # Create a producer client to send messages to the event hub.
    # Specify a connection string to your event hubs namespace and
    # the event hub name.
    producer = EventHubProducerClient.from_connection_string(conn_str=ehub_send_conn_str)
    
    async with producer:
        # Create a batch.
        event_data_batch = await producer.create_batch()

        # Add events to the batch.
        event_data_batch.add(EventData('First event '))
        event_data_batch.add(EventData('Second event'))
        event_data_batch.add(EventData('Third event'))

        # Send the batch of events to the event hub.
        await producer.send_batch(event_data_batch)

loop = asyncio.get_event_loop()
loop.run_until_complete(run())
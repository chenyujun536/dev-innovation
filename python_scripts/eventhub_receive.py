import asyncio
from azure.eventhub.aio import EventHubConsumerClient
from azure.eventhub.extensions.checkpointstoreblobaio import BlobCheckpointStore

blob_storage_conn_key = "fake_key_1"
blob_storage_conn_str = "DefaultEndpointsProtocol=https;AccountName=ychen32san;AccountKey=fake_key1;EndpointSuffix=core.windows.net"
blob_storage_container_name = "ychen32-storage"
event_hub_receiv_conn_str = "Endpoint=sb://ychen32-ehub.servicebus.windows.net/;SharedAccessKeyName=ReceiveFromMyEHub;SharedAccessKey=A1R+s4fW+4SX2BqUvh8DcdhsvmEuZBadky2jVO5aO4w=;EntityPath=myeventhub"
event_hub_name = "myeventhub"

async def on_event(partition_context, event):
    # Print the event data.
    print("Received the event: \"{}\" from the partition with ID: \"{}\"".format(event.body_as_str(encoding='UTF-8'), partition_context.partition_id))

    # Update the checkpoint so that the program doesn't read the events
    # that it has already read when you run it next time.
    await partition_context.update_checkpoint(event)

async def main():
    # Create an Azure blob checkpoint store to store the checkpoints.
    checkpoint_store = BlobCheckpointStore.from_connection_string(
        blob_storage_conn_str, 
        blob_storage_container_name)

    # Create a consumer client for the event hub.
    client = EventHubConsumerClient.from_connection_string(
        conn_str=event_hub_receiv_conn_str, 
        consumer_group="$Default", 
        eventhub_name=event_hub_name, 
        checkpoint_store=checkpoint_store)
    async with client:
        # Call the receive method. Read from the beginning of the partition (starting_position: "-1")
        await client.receive(on_event=on_event,  starting_position="-1")

if __name__ == '__main__':
    loop = asyncio.get_event_loop()
    # Run the main method.
    loop.run_until_complete(main())

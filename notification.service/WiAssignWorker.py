import time
import json
from datetime import datetime
from kafka import KafkaProducer
import psycopg2
from pymongo import MongoClient

# Подключение к MongoDB
client = MongoClient('mongodb://localhost:27017/')
db = client['mongo']
collection = db['workitems']

# Подключение к кафке
KAFKA_HOST = "localhost"
KAFKA_TOPIC_NAME = "wi_assign"

# Подключение к постгре
pg_connection = psycopg2.connect(database="postgres", user="postgres", password="postgres", host="localhost", port=5434)
cursor = pg_connection.cursor()

class MessageProducer:
    broker = None
    topic = None
    producer = None

    def __init__(self, broker, topic):
        self.broker = broker
        self.topic = topic
        self.producer = KafkaProducer(
            bootstrap_servers=self.broker,
            value_serializer=lambda v: json.dumps(v).encode("utf-8"),
            acks="all",
            retries = 3
        )

    def send_msg(self, msg):
        future = self.producer.send(self.topic, msg)
        self.producer.flush()

kafka_client = MessageProducer(
    f"{KAFKA_HOST}:9092",
    KAFKA_TOPIC_NAME
)

user_assigned_by_wi_id = dict()

while (True):
    time.sleep(5)
    print('Try to get info from mongo')

    user_ids = set()
    cursor.execute("select user_id from wi_assign_sub")
    record = cursor.fetchall()
    for record_i in record:
        user_ids.add(record_i[0])
    print(user_ids)

    query = {"AssignTo": {"$exists": True}}
    items_with_assignee = collection.find(query)

    for item_with_assignee in items_with_assignee:
        if (item_with_assignee["WiId"] not in user_assigned_by_wi_id.keys()) or (user_assigned_by_wi_id[item_with_assignee["WiId"]] != item_with_assignee["AssignTo"]):
            if (item_with_assignee["AssignTo"] in user_ids):
                # Означает, что WI был только что назначен пользователю
                print(item_with_assignee["WiId"])
                user_assigned_by_wi_id[item_with_assignee["WiId"]] = item_with_assignee["AssignTo"]

                try:
                    kafka_client.send_msg({
                        "chatId": item_with_assignee["AssignTo"],
                        #"username": item_with_assignee["Username"],
                        "username": "AlienLF",
                        "wiId": item_with_assignee["WiId"],
                        "wiTitle": item_with_assignee["Title"]
                    })
                except:
                    print("Ошибка при отправке сообщения в кафку")



client.close()
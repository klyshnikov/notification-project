import time
import json
from datetime import datetime, timedelta, timezone
from kafka import KafkaProducer
import psycopg2
from pymongo import MongoClient

# Подключение к MongoDB
client = MongoClient('mongodb://localhost:27017/')
db = client['mongo']  # Укажите имя вашей базы данных
collection = db['workitems']  # Укажите имя вашей коллекции

# Подключение к кафке
KAFKA_HOST = "localhost"
KAFKA_TOPIC_NAME = "wi_informing"

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

sec_period = 10

while (True):
    time.sleep(10)

    user_time_left_values = dict()
    inform_group_by_user = dict()

    # Находим пользователей, которым надо отправить сообщения о времени окончания
    cursor.execute("select user_id, minutes_remaining from wi_time_remaining_sub")
    records = cursor.fetchall()

    for rec in records:
        user_time_left_values[rec[0]] = rec[1]

    now = datetime.now().replace(tzinfo=None)

    query = {"AssignTo": {"$exists": True}}
    items_with_assignee = collection.find(query)

    for item_with_assignee in items_with_assignee:
        end_time = datetime.fromisoformat(item_with_assignee["EndTime"].replace('Z', '+00:00'))
        user_id = item_with_assignee["AssignTo"]

        if (user_id not in user_time_left_values.keys()):
            continue

        expired_times = user_time_left_values[user_id].split(',')

        for expired_time in expired_times:
            expired_time_minutes = int(expired_time)
            end_time_pred_range = [now + timedelta(minutes=expired_time_minutes) - timedelta(seconds=sec_period), now + timedelta(minutes=expired_time_minutes) + timedelta(seconds=sec_period)]
    
            # Make end_time timezone-aware
            if end_time.tzinfo is None:
                end_time = end_time.replace(tzinfo=timezone.utc)
    
            end_time_pred_range = [
                end_time_pred_range[0].replace(tzinfo=timezone.utc),
                end_time_pred_range[1].replace(tzinfo=timezone.utc)
            ]
            
            if end_time_pred_range[0] <= end_time <= end_time_pred_range[1]:
                print('added')
                print(f'key {item_with_assignee["AssignTo"]} {item_with_assignee["Username"]}')
                print(f'{item_with_assignee["AssignTo"]} {item_with_assignee["Username"]} {item_with_assignee["WiId"]} {item_with_assignee["Title"]} {item_with_assignee["EndTime"]}')
                inform_group_by_user_key = (item_with_assignee["AssignTo"], item_with_assignee["Username"])
                if inform_group_by_user_key not in inform_group_by_user:
                    inform_group_by_user[inform_group_by_user_key] = []
                inform_group_by_user[inform_group_by_user_key].append([item_with_assignee["AssignTo"],
                                                                       item_with_assignee["Username"],
                                                                       item_with_assignee["WiId"],
                                                                       item_with_assignee["Title"],
                                                                       item_with_assignee["EndTime"]])

    for k in inform_group_by_user.keys():
        wi_ids = [wid[2] for wid in inform_group_by_user[k]]
        titles = [title[3] for title in inform_group_by_user[k]]
        end_times = [et[4] for et in inform_group_by_user[k]]

        print("Message to kafka:")
        print(
            f"{inform_group_by_user[k][0][0]} {inform_group_by_user[k][0][1]} {json.dumps(wi_ids)} {json.dumps(titles)} {json.dumps(end_times)}")
        try:
            kafka_client.send_msg({
                "chatId": inform_group_by_user[k][0][0],
                "username": inform_group_by_user[k][0][1],
                "wiId": json.dumps(wi_ids),
                "wiTitle": json.dumps(titles),
                "endTime": json.dumps(end_times)
            })
        except:
            print("Ошибка при отправке сообщения в кафку")

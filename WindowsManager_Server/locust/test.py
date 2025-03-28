from locust import HttpUser, task, between
from faker import Faker
import random

fake = Faker()

class ApiUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:800"
    
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.created_host_ids = []
        self.created_publisher_ids = []
        self.created_software_ids = []
        self.hostnames = [fake.hostname() for _ in range(10)]
        self.publisher_names = [fake.company() for _ in range(5)]
        self.software_names = [fake.bs() + " " + fake.word() for _ in range(10)]
        self.versions = [f"{random.randint(1, 20)}.{random.randint(0, 9)}" for _ in range(10)]

    @task(5)
    def get_all_hosts(self):
        self.client.get("/api/Host")

    @task(3)
    def create_host(self):
        hostname = fake.hostname()
        response = self.client.post(
            "/api/Host",
            json=hostname,
            headers={"Content-Type": "application/json"}
        )
        if response.status_code == 200:
            data = response.json()
            if "id" in data:
                self.created_host_ids.append(data["id"])
                self.hostnames.append(hostname)

    @task(4)
    def get_host_by_id(self):
        if self.created_host_ids:
            host_id = random.choice(self.created_host_ids)
            self.client.get(f"/api/Host/{host_id}")

    @task(2)
    def get_host_by_hostname(self):
        if self.hostnames:
            hostname = random.choice(self.hostnames)
            self.client.get(f"/api/Host/Hostname/{hostname}", name="/api/Host/Hostname/[hostname]")

    @task(1)
    def delete_host(self):
        if self.created_host_ids:
            host_id = random.choice(self.created_host_ids)
            response = self.client.delete(f"/api/Host/{host_id}")
            if response.status_code == 200:
                self.created_host_ids.remove(host_id)

    # @task(4)
    # def get_all_publishers(self):
    #     self.client.get("/api/Publisher")

    # @task(3)
    # def create_publisher(self):
    #     publisher_name = fake.company()
    #     response = self.client.post(
    #         "/api/Publisher",
    #         json=publisher_name,
    #         headers={"Content-Type": "application/json"}
    #     )
    #     if response.status_code == 200:
    #         data = response.json()
    #         if "id" in data:
    #             self.created_publisher_ids.append(data["id"])
    #             self.publisher_names.append(publisher_name)

    # @task(2)
    # def update_publisher(self):
    #     if self.created_publisher_ids:
    #         publisher_id = random.choice(self.created_publisher_ids)
    #         new_name = fake.company()
    #         self.client.put(
    #             f"/api/Publisher/{publisher_id}",
    #             json=new_name,
    #             headers={"Content-Type": "application/json"}
    #         )

    # @task(1)
    # def patch_publisher_title(self):
    #     if self.publisher_names:
    #         old_title = random.choice(self.publisher_names)
    #         new_title = fake.company()
    #         self.client.patch(
    #             "/api/Publisher/title",
    #             params={"Title": old_title, "TitleChange": new_title}
    #         )

    # @task(5)
    # def get_all_software(self):
    #     self.client.get("/api/Software")

    # @task(3)
    # def create_software(self):
    #     if self.created_publisher_ids:
    #         software_data = {
    #             "name": random.choice(self.software_names),
    #             "version": random.choice(self.versions),
    #             "publisherid": random.choice(self.created_publisher_ids)
    #         }
    #         response = self.client.post(
    #             "/api/Software",
    #             json=software_data,
    #             headers={"Content-Type": "application/json"}
    #         )
    #         if response.status_code == 200:
    #             data = response.json()
    #             if "id" in data:
    #                 self.created_software_ids.append(data["id"])

    # @task(2)
    # def add_software_to_host(self):
    #     if self.hostnames:
    #         hostname = random.choice(self.hostnames)
    #         software_data = [{
    #             "name": random.choice(self.software_names),
    #             "version": random.choice(self.versions),
    #             "publisher": random.choice(self.publisher_names)
    #         } for _ in range(random.randint(1, 3))]
            
    #         self.client.post(
    #             f"/api/Host/{hostname}/software",
    #             json=software_data,
    #             headers={"Content-Type": "application/json"},
    #             name="/api/Host/[hostname]/software [POST]"
    #         )

    # @task(2)
    # def check_valid_software(self):
    #     if self.hostnames:
    #         hostname = random.choice(self.hostnames)
    #         software_data = [{
    #             "name": random.choice(self.software_names),
    #             "version": random.choice(self.versions),
    #             "publisher": random.choice(self.publisher_names)
    #         } for _ in range(random.randint(1, 3))]
            
    #         self.client.put(
    #             f"/api/Host/{hostname}/software",
    #             json=software_data,
    #             headers={"Content-Type": "application/json"},
    #             name="/api/Host/[hostname]/software [PUT]"
    #         )

    # @task(2)
    # def find_or_create_software(self):
    #     software_data = {
    #         "name": random.choice(self.software_names),
    #         "version": random.choice(self.versions),
    #         "publisher": random.choice(self.publisher_names)
    #     }
    #     self.client.post(
    #         "/api/Software/findOrCreate",
    #         json=software_data,
    #         headers={"Content-Type": "application/json"}
    #     )

    # @task(1)
    # def find_software(self):
    #     software_data = {
    #         "name": random.choice(self.software_names),
    #         "version": random.choice(self.versions),
    #         "publisher": random.choice(self.publisher_names)
    #     }
    #     self.client.post(
    #         "/api/Software/find",
    #         json=software_data,
    #         headers={"Content-Type": "application/json"}
    #     )
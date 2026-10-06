<div align="center">

# 🥗 Adria — Server

**.NET 8 REST API (Adria.Main) with Dockerized MySQL for local development.**

![.NET](https://img.shields.io/badge/.NET-8-512bd4?style=for-the-badge&logo=.net)
![MySQL](https://img.shields.io/badge/MySQL-MySQL-003545?style=for-the-badge&logo=mysql)
![Docker](https://img.shields.io/badge/Docker-Docker-2496ED?style=for-the-badge&logo=docker)

</div>

> 🎓 Created as part of a simulated 2084 "Return to Earth" startup coursework: backend API and dataset for NutriScan.


## 📖 About

- API project located under `server/src/Adria.Main`.
- Uses **ASP.NET Core** on **.NET 8** and exposes Swagger UI for interactive documentation.


## 🏗️ Architecture

```mermaid
flowchart LR
    Client[Client (Vue 3)] -->|HTTP| API[Adria.Main API]
    API -->|CRUD| MySQL[(MySQL database)]
    subgraph LocalDev
        DB[MySQL container] -. docker compose .-> API
    end
```


## ✨ Features

- REST API with Swagger (see `src/Adria.Main`)
- Docker Compose configuration for local MySQL (`server/config/docker/docker-compose.yml`)
- Test project scaffolding under `tests/`


## 🛠️ Tech Stack

| Area | Technologies |
|------|--------------|
| Backend | .NET 8, ASP.NET Core, C# |
| Database | MySQL (Docker) |
| DevOps | Docker, Docker Compose |


## 🚀 Getting Started

### Prerequisites

- .NET SDK 8.0
- Docker & Docker Compose
- Git




### Configuration

| Variable | Description |
|----------|-------------|
| `MYSQL_ROOT_PASSWORD` | MySQL root password (see `server/config/docker/docker-compose.yml`) |
| `MYSQL_DATABASE` | Database name (`nutriscan`) |
| `MYSQL_USER` | MySQL user (`nutri`) |
| `MYSQL_PASSWORD` | MySQL password (`scan`) |


### Run (with Docker database)

```bash
docker compose -f config/docker/docker-compose.yml up -d
dotnet run --project src/Adria.Main
```


### Run tests

```bash
dotnet test
```


## 📡 API / Usage

- Default API URL (local): `http://localhost:8000`
- Swagger UI: `http://localhost:8000/swagger/index.html`

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/swagger/index.html` | API documentation (Swagger UI) |


### Endpoints (summary)

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/subscribe` | Subscribe to push notifications |

| POST | `/api/subscriptions/` | Create a subscription |
| GET | `/api/subscriptions/` | List subscriptions |

| POST | `/api/foodcomposition/create` | Create a new food composition |
| GET | `/api/FoodComposition/by-food/{foodName}` | Get food compositions by food name |
| GET | `/api/FoodComposition/by-nutrient/{type}` | Get food compositions by nutrient |
| POST | `/api/FoodComposition/food/create` | Create a new food |
| GET | `/api/FoodComposition/food/allfood` | Get all foods |

| POST | `/api/Scanner/ScanFood/{adrianId}` | Scan food for a user |
| GET | `/api/Scanner/history/{adrianId}` | Get scan history for a user |
| DELETE | `/api/Scanner/ScanFood/scan/{scanId}` | Delete a scan |

| POST | `/api/users/` | Create a new user |
| GET | `/api/users/{userId}` | Get user by ID |
| GET | `/api/users/` | Get all users |

| POST | `/api/analyses/` | Create a new health analyse |
| GET | `/api/analyses/user/{userId}/stats` | Get latest body stats for a user |
| PUT | `/api/analyses/definitions/{id}/goal` | Update goal for a body stat definition |

| POST | `/api/Supplement/` | Create supplement |
| GET | `/api/Supplement/all` | Get all supplements |
| GET | `/api/Supplement/{id}` | Get supplement by ID |

| POST | `/api/OrderSupplement/` | Create order-supplement detail |
| DELETE | `/api/OrderSupplement/` | Delete order-supplement detail |
| GET | `/api/OrderSupplement/{orderId}` | Get supplements for an order |

| POST | `/api/order/` | Create an order |
| DELETE | `/api/order/` | Delete an order (expects ID in body) |
| GET | `/api/order/{id}` | Get order by ID |
| GET | `/api/order/user/{adrianId}` | Get orders by user (AdrianId) |


### Database schema and seed

The repository includes `create_database.sql` which contains DDL and seed data for development and tests:

- `server/src/Adria.Infrastructure/Persistence/Scripts/create_database.sql`

To create and seed the database using the local Docker MySQL service started with `server/config/docker/docker-compose.yml` run:

```bash
# start the database service
docker compose -f server/config/docker/docker-compose.yml up -d

# run the SQL script against the running MySQL service
# replace <service> with the MySQL service name from your compose project (see `docker compose ps`)
docker compose -f server/config/docker/docker-compose.yml exec -T nutriscan_db.adria \
    mysql -u root -pscan nutriscan < server/src/Adria.Infrastructure/Persistence/Scripts/create_database.sql
```

If your compose service name differs, use `docker compose -f server/config/docker/docker-compose.yml ps` to find the MySQL service container name.


## 👤 Author

| Name | GitHub | LinkedIn |
| --- | --- | --- |
| Maurice De Kegel | [MriceDK](https://github.com/MriceDK) | [LinkedIn](https://www.linkedin.com/in/dekegelmaurice/) |


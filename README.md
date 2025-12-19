# NutriScan Backend (.NET 8)

This repository contains the backend services of the **NutriScan** application. The project is built with **C# / .NET 8** and supports Docker-based development.

---

## 🚀 Requirements

Make sure the following tools are installed on your system:

* [.NET SDK 8.0](https://dotnet.microsoft.com/)
* [Docker & Docker Compose](https://www.docker.com/)
* Git

---

## 📁 Project Structure (Overview)

```
config/
 └─ docker/
     └─ docker-compose.yml
src/
 └─ Adria.Main/        # Main API project
 └─ ...                # Other layers (Domain, Infrastructure, etc.)
tests/
 └─ ...                # Test projects
```

---

## ▶️ Running the Application

### 1️⃣ Start Docker Services

First, start the required infrastructure services (database, etc.):

```bash
docker compose -f config/docker/docker-compose.yml up -d
```

> ⚠️ If you encounter any errors, try removing the **example server container** from the `docker-compose.yml` file.

---

### 2️⃣ Run the Backend API

```bash
dotnet run --project src/Adria.Main
```

The application will run on the following address by default:

```
http://localhost:8000
```

---

### 3️⃣ Swagger UI

You can access the API documentation via Swagger:

```
http://localhost:8000/swagger/index.html
```

Swagger allows you to explore and test all available endpoints.

---

## 🧪 Running Tests

To execute all tests:

```bash
dotnet test
```

---

## 🛠️ Technologies Used

* **.NET 8 / C#**
* **ASP.NET Core Web API**
* **Docker & Docker Compose**
* **Swagger / OpenAPI**
* **xUnit / MSTest / NUnit** (depending on the project setup)

---

## 🧩 Common Issues

* **Docker container conflicts**: Stop any previously running containers that may cause conflicts.
* **Port conflict (8000)**: If the port is already in use, update `launchSettings.json` or Docker configuration.
* **Port conflict (3306 – MySQL)**: On machines with MySQL installed locally, the MySQL service often runs automatically on port **3306**. This can prevent the Docker MySQL container from starting.

  * Stop the local MySQL service before running Docker.
  * Alternatively, change the MySQL port in `docker-compose.yml` if stopping the service is not possible.


---


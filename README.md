# SmartHouse Microservices

A smart home system built on microservices architecture using **ASP.NET Core 9.0**.  
This project demonstrates IoT device management through a centralized facade API using the **Facade design pattern**.

---

## Architecture

The project consists of four microservices:

- **SmartAppMain** (port 5000) – Main API gateway implementing the Facade pattern
- **SmartLightAPI** (port 8002) – Smart lighting control service
- **SmartSpeakerAPI** (port 8001) – Audio system control service
- **SmartCurtainsAPI** (port 8003) – Smart curtains control service

---

## Technologies

- **.NET 9.0 / C# 13.0**
- **ASP.NET Core Web API**
- **Serilog** – structured logging (SmartAppMain)
- **Swagger / OpenAPI** – API documentation
- **xUnit**, **FluentAssertions**, **Moq** – unit testing

---

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 (recommended) or Visual Studio Code

---

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/Mariana8l8/SmartHouse_Microservices.git
cd SmartHouse_Microservices
```

---

## Start Microservices

Each microservice must be started separately.  
Open multiple terminals and run the following commands:

### Terminal 1 – SmartSpeakerAPI

```bash
cd SmartSpeakerAPI
dotnet run
```

### Terminal 2 – SmartLightAPI

```bash
cd SmartLightAPI
dotnet run
```

### Terminal 3 – SmartCurtainsAPI

```bash
cd SmartCurtainsAPI
dotnet run
```

### Terminal 4 – SmartAppMain

```bash
cd SmartAppMain
dotnet run
```

---

## Swagger UI

Once all services are running, Swagger documentation will be available at:

- **SmartAppMain**: http://localhost:5000/swagger
- **SmartSpeakerAPI**: http://localhost:8001/swagger
- **SmartLightAPI**: http://localhost:8002/swagger
- **SmartCurtainsAPI**: http://localhost:8003/swagger

---

## API Endpoints

### SmartAppMain (Facade)

| Method | Endpoint | Description |
|------|---------|-------------|
| GET | `/main/status-all` | Get status of all devices |
| POST | `/main/toggle-light` | Toggle light on/off |
| POST | `/main/toggle-speaker` | Toggle speaker on/off |
| POST | `/main/toggle-curtains` | Toggle curtains open/close |

### SmartLightAPI

| Method | Endpoint | Description |
|------|---------|-------------|
| GET | `/light/status` | Get light status |
| POST | `/light/power/{state}` | Turn light on/off |
| POST | `/light/brightness/{level}` | Set brightness (0–100) |
| GET | `/light/health` | Health check |

### SmartSpeakerAPI

| Method | Endpoint | Description |
|------|---------|-------------|
| GET | `/speaker/status` | Get speaker status |
| POST | `/speaker/power/{state}` | Turn speaker on/off |
| POST | `/speaker/volume/{level}` | Set volume (0–100) |
| GET | `/speaker/health` | Health check |

### SmartCurtainsAPI

| Method | Endpoint | Description |
|------|---------|-------------|
| GET | `/curtains/status` | Get curtains status |
| POST | `/curtains/power/{state}` | Open/close curtains |
| POST | `/curtains/position/{value}` | Set position (0–100) |
| GET | `/curtains/health` | Health check |

---

## Running Tests

Execute all unit tests with:

```bash
dotnet test
```

Test coverage includes:
- Controller unit tests for all IoT services
- IoTFacade integration tests
- HTTP client mocking using Moq

---

## Project Structure

```text
SmartHouse_Microservices/
├── SmartAppMain/
│   ├── Controllers/
│   ├── Services/
│   ├── Models/
│   └── Program.cs
├── SmartLightAPI/
│   ├── Controllers/
│   └── Program.cs
├── SmartSpeakerAPI/
│   ├── Controllers/
│   └── Program.cs
├── SmartCurtainsAPI/
│   ├── Controllers/
│   └── Program.cs
└── Tests/
    ├── IoTFacadeTests.cs
    ├── LightControllerTests.cs
    ├── SpeakerControllerTests.cs
    └── CurtainsControllerTests.cs
```

---

## Design Patterns

- **Facade Pattern** – `IoTFacade` simplifies interaction with multiple microservices
- **Dependency Injection** – `IHttpClientFactory` for HTTP client management
- **Repository Pattern** – Thread-safe device state handling

---

## Logging

`SmartAppMain` uses **Serilog** for structured logging:

- Log file: `log/smartAppLogs.txt`
- Rolling interval: Daily
- Minimum level: Information

---

## Thread Safety

All device controllers implement thread-safe operations using `lock` statements to prevent race conditions when handling concurrent requests.

---

## Configuration

Services communicate via HTTP:

- Speaker: `http://localhost:8001/`
- Light: `http://localhost:8002/`
- Curtains: `http://localhost:8003/`

To change ports, update `launchSettings.json` in each project's `Properties` folder.

---

## License

This project is licensed under the MIT License.

---

## Contributing

Contributions are welcome.  
Feel free to fork the repository and submit a Pull Request.

---

## Contact

For questions or feedback, please open an issue on GitHub.

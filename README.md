# ShiftTrack

A simple, no-fuss app for logging and tracking work shifts. Clock in, clock out, and instantly see your hours, schedules, and earnings in one place — no spreadsheets, no guesswork.

The solution has two projects:

- **`ShiftTrack`** — the ASP.NET Core Web API (EF Core, SQL Server) that owns the `Employees` and `Shifts` data.
- **`ShiftTrack.UI`** — a console client, built with [Spectre.Console](https://spectreconsole.net/), for managing employees and shifts against the API.

## Features

- **Employees** — list, get by ID, create, update, delete
- **Shifts** — list, get by ID, create, update, delete
- Client-side validation (`ValidationService`) before any request hits the API
- Styled console UI: tables, status spinners, headers, and colored success/error/info/warning messages

## Requirements

- .NET SDK matching the project's target framework
- SQL Server (or whatever provider the `ShiftTrack` API's `DbContext` is configured for)

## Getting Started

### 1. Run the API

```
cd ShiftTrack
dotnet run
```

By default the console client expects the API at `https://localhost:7098/`. If your API runs on a different port, update the `BaseAddress` in `ShiftTrack.UI/Services/EmployeeApiService.cs` and `ShiftApiService.cs`.

### 2. Run the console client

```
cd ShiftTrack.UI
dotnet run
```

## Project Structure

```
ShiftTrack/                          # Web API
  Controllers/
  Services/
  DTO/
  Models/

ShiftTrack.UI/                       # Console client
  UI/
    ConsoleMenu.cs                   # Top-level menu (Employee / Shift Management / Exit)
    EmployeeMenu.cs                  # Employee submenu + actions
    ShiftMenu.cs                     # Shift submenu + actions
    ConsoleUI.cs                     # Header, messages, pause, confirm, goodbye
  Services/
    EmployeeApiService.cs            # HTTP calls for employees
    ShiftApiService.cs               # HTTP calls for shifts
    ValidationService.cs             # Shared input validation
  DTO/
    Employee/
      EmployeeDto.cs
      CreateEmployeeDto.cs
      UpdateEmployeeDto.cs
    Shifts/
      ShiftDto.cs
      ShiftStatus.cs
  Program.cs                         # Entry point (top-level statements)
```

## Validation

`ValidationService` is shared by both API services and runs before any HTTP call:

- IDs must be positive numbers
- Employee names are required, max 100 characters, letters and spaces only
- Shift notes are capped at 500 characters

Validation failures throw `ArgumentException`, which each menu action catches and displays via `ConsoleUI.ShowError`.

## Architectural Choices

- **Two-project split (API + console client)** — `ShiftTrack` (the Web API) 
  and `ShiftTrack.UI` (the console client) are kept as separate projects 
  rather than one combined console app. This mirrors a real-world setup 
  where a backend and its client are decoupled: the API owns data access 
  and business rules, while the console client is just one possible 
  consumer of that API. This also means another client (a web frontend, 
  a mobile app) could be built later without touching the API at all.

- **API layer (`ShiftTrack`)** — follows a standard ASP.NET Core structure 
  (`Controllers` → `Services` → `Models`/`DTO`), keeping HTTP concerns 
  (routing, status codes) in Controllers, business/data logic in Services, 
  and using DTOs so the API's public contract doesn't leak internal EF Core 
  entity shapes to consumers.

- **Console client (`ShiftTrack.UI`)** — talks to the API purely over HTTP 
  (`EmployeeApiService`, `ShiftApiService`), with no direct database access 
  of its own. This enforces a clean boundary: the console app doesn't know 
  or care whether the API is backed by SQL Server, another database, or 
  even a different backend entirely — it only knows the API's HTTP contract.

- **Shared `ValidationService` on the client side** — input is validated 
  *before* a request is sent to the API, so obviously invalid input (empty 
  names, negative IDs) is caught immediately with fast, clear feedback, 
  rather than waiting on a round-trip to the API just to get a 400 response. 
  The API is still expected to validate independently too, since a client 
  can't be trusted as the only line of defense.

- **DTOs per operation** (`CreateEmployeeDto`, `UpdateEmployeeDto`, 
  `EmployeeDto`) — rather than one shared model, separate DTOs per action 
  make it clear exactly what fields are required/allowed for each operation 
  (e.g., you can't accidentally set an `Id` on create), and keep the API 
  contract explicit and self-documenting.

- **Spectre.Console for the UI layer** — chosen over plain 
  `Console.WriteLine` calls to give the client a more usable, readable 
  interface (tables, spinners, colored status messages) without needing 
  a full GUI framework.

  ## Reflection

This project helped me understand the client-server relationship 
more concretely than just building a single monolithic console app 
having to actually run two separate projects together, and debug issues 
across the HTTP endpoints.



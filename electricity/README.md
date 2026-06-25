# Electricity Billing Web Application

ASP.NET Web Forms application for electricity billing management with separate admin and customer flows.

## Tech Stack
- ASP.NET Web Forms (`.aspx`, `.master`) on .NET Framework 4.6.2
- C# code-behind
- MySQL connectivity via `MySql.Data`
- Entity Framework 6.2.0 (package reference present)
- Frontend assets: Bootstrap, jQuery, custom CSS/JS

## Project Structure
- `index.aspx` - Landing page
- `AdminLogin.Master` / `AdminLog.aspx` / `AdminWeb.aspx` - Admin login and dashboard flow
- `customerLogin.Master` / `CustomorLog.aspx` / `CustomerWeb.aspx` - Customer login and dashboard flow
- `Bills.aspx` - Bill-related operations
- `add.aspx`, `find.aspx`, `view.aspx`, `pay.aspx` - Core billing actions
- `Web.config` - Application configuration
- `electricity.sln` / `electricity.csproj` - Visual Studio solution and project

## Prerequisites
- Windows with IIS / IIS Express support for ASP.NET Web Forms
- Visual Studio (2019 or later recommended) with .NET desktop + ASP.NET workload
- .NET Framework 4.6.2 targeting pack
- MySQL server accessible from the app

## Getting Started
1. Open `electricity.sln` in Visual Studio.
2. Restore NuGet packages.
3. Update database connection settings in `Web.config`.
4. Build the solution.
5. Run the application and open the default start page (`index.aspx` if not already configured).

## Configuration Notes
- Database connection is read from `appSettings` key `Data` in `Web.config`.
- Avoid storing production credentials in source control.
- Keep environment-specific settings in transform files (`Web.Debug.config`, `Web.Release.config`) or secure secret storage.

## Electricity + IOTController Integration
Both apps can run together against the same MySQL schema (`govaeb`).

1. Run `DB_SETUP.sql` for legacy tables and `DB_SCHEMA_V2.sql` for normalized IoT/API tables.
2. Keep these connection strings aligned:
   - `electricity/Web.config` -> `appSettings:Data`
   - `IOTController/App.config` -> `appSettings:Db`
3. Ensure both point to the same host/database/user (for local setup: `127.0.0.1`, `govaeb`).
4. Start MariaDB first, then run `IOTController` and `electricity`.

Shared tables used across both apps:
- `tblCustomer`
- `tblBill` (includes `BillDate`)
- `tblRates`
- `readings`

Normalized tables (target model):
- `Users`
- `Meters`
- `MeterReadings`
- `TariffPlans`
- `BillingCycles`
- `Bills`
- `Payments`
- `AuditLogs`

## IoT Ingestion API
Endpoint:
- `POST /api/iot/readings.ashx`

Headers:
- `X-Device-Key`: meter device key from `Meters.DeviceKey`
- `X-Signature`: HMAC-SHA256 hex of raw JSON body using `Meters.SharedSecret`

JSON body:
```json
{
  "meterId": "MTR-1001",
  "unitsDelta": 0.14,
  "readingAtUtc": "2026-04-07T08:10:00Z",
  "sequenceNo": 15423
}
```

Behavior:
- Validates payload and timestamp window.
- Authenticates meter by `meterId + X-Device-Key`.
- Verifies HMAC signature.
- Stores reading in `MeterReadings`.
- Enforces idempotency with unique key `(MeterPk, SequenceNo)`.

### IoT API Smoke Test (PowerShell)
Use this script to post one signed test reading without hardware:

```powershell
powershell -ExecutionPolicy Bypass -File .\iot\send_test_reading.ps1 `
  -ApiUrl "http://127.0.0.1/api/iot/readings.ashx" `
  -MeterId "MTR1001" `
  -DeviceKey "dev-MTR1001" `
  -SharedSecret "<value_from_Meters.SharedSecret>"
```

Expected success response status is `accepted` (HTTP 202) or `duplicate` (HTTP 200).

## Build
Use Visual Studio Build, or MSBuild from Developer Command Prompt:

```powershell
msbuild electricity.sln /t:Build /p:Configuration=Debug
```

## Notes
- This repository includes generated build output (`bin/`, `obj/`).
- Consider adding a `.gitignore` to exclude generated artifacts and local IDE files.

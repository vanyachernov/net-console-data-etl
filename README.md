# ✨ ETL Project: Processing and Loading Trip Data

## 🌟 Project Overview

This project is a C# console application that implements a foundational **ETL (Extract, Transform, Load)** pipeline. Its purpose is to process trip data from a CSV file, apply necessary transformations, and efficiently load the clean results into an **MS SQL Server** database.

---

## 🎯 Project Objectives

1.  **Extract:** Read and parse data from the provided input CSV file.
2.  **Transform:**
    * Filter and select only the required **9 core columns**.
    * **Deduplicate** records based on the composite key: (`pickup_datetime`, `dropoff_datetime`, `passenger_count`). Duplicates are isolated and exported.
    * Convert pickup and drop-off times from the local EST timezone to **UTC**.
    * Convert the flag `store_and_fwd_flag` from boolean codes (`Y`/`N`) to readable strings (`Yes`/`No`).
    * Ensure all text-based fields are trimmed of whitespace.
3.  **Load:** Perform an efficient bulk insertion of the cleaned records into the MS SQL Server using **`SqlBulkCopy`**.
4.  **Error Handling:** Write all removed duplicate records to a separate `duplicates.csv` file.

---

## 🛠️ Technology Stack

* **Language:** C# (.NET Core)
* **CSV Reading:** [CsvHelper](https://joshclose.github.io/CsvHelper/)
* **Database Access:** [Microsoft.Data.SqlClient](https://docs.microsoft.com/en-us/sql/connect/ado-net/microsoft-data-sqlclient)
* **Bulk Loading:** [SqlBulkCopy](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlbulkcopy)
* **Time Zone Management:** [TimeZoneConverter](https://github.com/mj1856/TimeZoneConverter)

---

## ⚙️ Setup and Execution

### 1. Requirements

* .NET SDK (matching the version in the `.csproj` file)
* Access to an **MS SQL Server** instance (local or remote).

### 2. Database Preparation

Before running the application, you must create the database and the target table with the optimized schema.

#### A. SQL Table Creation Script

Use the following script to create the `dbo.Trips` table. The schema includes a **clustered primary key** (`Id` IDENTITY) and a **persisted computed column** (`TripDurationMinutes`) to optimize the target analytical queries.

Open the required SQL scripts in the [config/queries](config/queries) directory. Specifically, run the **[createTables.sql](config/queries/createTables.sql)** script in your MS SQL Server instance to set up the necessary table and indexes.


#### B. Configuration

Update the `ConnectionString` constant within the `Program.cs` file to match your SQL Server connection details.

### 3. Execution

1.  Download the input CSV file and place it in the path configured in `Program.cs` (default: `./Data/sample-cab-data.csv`).
2.  Build and run the project:
    ```bash
    dotnet run
    ```
3.  Upon successful completion, the `dbo.Trips` table will be populated, and a file named `./Data/duplicates.csv` will be created.

---

## 📐 Code Architecture

The project adheres to the **Single Responsibility Principle (SRP)** by delegating core functionality to specialized service classes. The `Program.cs` acts as the **Orchestrator** of the ETL workflow.

| Component | Responsibility Zone | Description |
| :--- | :--- | :--- |
| **`Program.cs`** | **Orchestration** | Coordinates the entire ETL sequence (Read -> Deduplicate -> Convert -> Insert -> Write). Contains the `SqlBulkCopy` setup with explicit column mappings. |
| **`CsvReaderService`** | **Extraction** | Solely responsible for file I/O, CSV parsing, data type conversion, and EST-to-UTC time zone conversion. |
| **`TripDataProcessorService`** | **Transform (Business Logic)** | Responsible for the data cleaning logic, specifically identifying duplicates using a composite key (`Pickup`, `Dropoff`, `PassengerCount`). |
| **`TripConverterService`** | **Transform (Structure)** | Responsible for converting the clean `IEnumerable<Trip>` collection into a **`System.Data.DataTable`**, the required input format for `SqlBulkCopy`. |
| **`CsvWriterService`** | **Export** | Responsible for writing a collection of objects back into a CSV file (used for exporting the duplicates). |

---

## 📈 Scaling for Large Data (10 GB+ Input)

If the input CSV file were to exceed 10 GB, the current in-memory processing (`List<Trip>` and `HashSet` for deduplication keys) would lead to an **OutOfMemoryException**.

To scale this project for massive datasets, as for me, the following changes would be necessary:

1.  **Streaming:** The `CsvReaderService.Read` method must be refactored to use **`yield return`** (`IEnumerable<Trip>`) to process data one row at a time.
2.  **Batch Processing:** The orchestration logic must be updated to read, process, and insert data in **manageable batches** (e.g., 100,000 rows at a time).
3.  **Database-Side Deduplication:** The memory-intensive deduplication must be offloaded to **SQL Server** by inserting *all* raw data into a temporary "staging" table and using SQL logic (e.g., `ROW_NUMBER()`) to filter unique rows.
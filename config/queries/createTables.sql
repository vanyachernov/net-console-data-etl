USE TestAssignmentDB;
GO

IF OBJECT_ID('dbo.Trips', 'U') IS NOT NULL 
    DROP TABLE dbo.Trips;
GO

CREATE TABLE dbo.Trips
(
    id                    BIGINT IDENTITY(1,1) NOT NULL,
    tpep_pickup_datetime  DATETIME2 NOT NULL,
    tpep_dropoff_datetime DATETIME2 NOT NULL,
    passenger_count       TINYINT,
    trip_distance         FLOAT,
    store_and_fwd_flag    VARCHAR(3),
    PULocationID          INT,
    DOLocationID          INT,
    fare_amount           DECIMAL(10,2),
    tip_amount            DECIMAL(10,2),
    trip_duration_minutes   AS DATEDIFF(minute, tpep_pickup_datetime, tpep_dropoff_datetime) PERSISTED
);
GO

CREATE CLUSTERED INDEX CI_Trips_PULocationID ON dbo.Trips(PULocationID);
GO

CREATE NONCLUSTERED INDEX NCI_Trips_PULocationID_Tip ON dbo.Trips(PULocationID) INCLUDE (tip_amount);
GO

CREATE NONCLUSTERED INDEX NCI_Trips_TripDistance ON dbo.Trips(trip_distance DESC);
GO

CREATE NONCLUSTERED INDEX NCI_Trips_Duration ON dbo.Trips(trip_duration_minutes DESC);
GO

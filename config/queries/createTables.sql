USE TestAssignmentDB;
GO

CREATE TABLE dbo.Trips
(
    tpep_pickup_datetime  DATETIME2 NOT NULL,
    tpep_dropoff_datetime DATETIME2 NOT NULL,
    passenger_count       TINYINT,
    trip_distance         FLOAT,
    store_and_fwd_flag    VARCHAR(3),
    PULocationID          INT,
    DOLocationID          INT,
    fare_amount           DECIMAL(10,2),
    tip_amount            DECIMAL(10,2)
);
GO

CREATE INDEX INDEX_Trips_PULocationID ON dbo.Trips(PULocationID);
CREATE INDEX INDEX_Trips_TripDistance ON dbo.Trips(trip_distance);
CREATE INDEX INDEX_Trips_PickupDropoff ON dbo.Trips(tpep_pickup_datetime, tpep_dropoff_datetime);
CREATE INDEX INDEX_Trips_TipAmount ON dbo.Trips(tip_amount);
GO

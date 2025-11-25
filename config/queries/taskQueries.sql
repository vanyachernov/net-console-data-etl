-- Find out which `PULocationId` (Pick-up location ID) has the highest tip_amount on average.

SELECT TOP 1
    PULocationID AS PickUpLocationID,
    AVG(tip_amount) AS AverageTip
FROM
    dbo.Trips
GROUP BY
    PULocationID
ORDER BY
    AverageTip DESC;

-- ind the top 100 longest fares in terms of `trip_distance`.

SELECT TOP 100
    tpep_pickup_datetime AS PickUpTime,
    tpep_dropoff_datetime AS DropOffTime,
    trip_duration_minutes AS DurationMinutes,
    trip_distance AS TripDistance,
    PULocationID AS PickUpLocationID
FROM
    dbo.Trips
ORDER BY
    trip_distance DESC;

-- Find the top 100 longest fares in terms of time spent traveling.

SELECT TOP 100
    tpep_pickup_datetime AS PickUpTime,
    tpep_dropoff_datetime AS DropOffTime,
    trip_duration_minutes AS DurationMinutes
FROM
    dbo.Trips
ORDER BY
    trip_duration_minutes DESC;

-- Total amount of tips collected at a specific location.

SELECT
    SUM(tip_amount) AS TotalTips
FROM
    dbo.Trips
WHERE
    PULocationID = 140;
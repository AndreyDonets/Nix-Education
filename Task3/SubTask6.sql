SELECT ShipCountry
FROM (
SELECT ShipCountry, ShipCity, COUNT(ShippedDate) AS OrdersCount
FROM Orders
WHERE ShippedDate IS NOT NULL
GROUP BY ShipCountry, ShipCity) AS TotalData
WHERE OrdersCount > 2
GROUP BY ShipCountry
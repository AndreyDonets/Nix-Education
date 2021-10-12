SELECT ShipCity
FROM Orders
WHERE DATEDIFF(day, OrderDate, ShippedDate) > DAY(9)
GROUP BY ShipCity
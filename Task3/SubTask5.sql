SELECT COUNT(ShipCity) AS CityCount
FROM Orders
WHERE EmployeeID = 1 AND ShipCountry = 'France' AND YEAR(OrderDate) = 1997
GROUP BY EmployeeID
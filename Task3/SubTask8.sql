SELECT ContactName
FROM Customers
CROSS APPLY (
	SELECT ShipCity
	FROM Orders
	WHERE Orders.CustomerID = Customers.CustomerID 
		AND NOT ShipCity = City
	) AS CustomerOrders
GROUP BY ContactName
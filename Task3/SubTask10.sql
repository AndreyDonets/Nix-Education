WITH EmployeeOrders AS(
	SELECT 
		FirstName,
		LastName,
		OrderID
	FROM Orders
	CROSS APPLY(
		SELECT
			FirstName,
			LastName
		FROM Employees
		WHERE Employees.EmployeeID = Orders.EmployeeID
	) AS EmployeersName
	WHERE DATEDIFF(month, '1996.09.01', OrderDate) IN(0, 1, 2)
)
SELECT 
	FirstName,
	LastName,
	SUM(Quantity) AS TotalQuantity
FROM EmployeeOrders
CROSS APPLY(
	SELECT Quantity
	FROM [Order Details]
	WHERE [Order Details].OrderID = EmployeeOrders.OrderID
) AS QuantityPerOrder
GROUP BY 
	FirstName,
	LastName
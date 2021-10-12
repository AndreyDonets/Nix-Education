SELECT TOP 1
	CustomersCount
FROM(
	SELECT 
		EmployeeID, 
		COUNT(CustomerID) AS CustomersCount
	FROM Orders
	GROUP BY EmployeeID
	) AS Total
ORDER BY CustomersCount DESC
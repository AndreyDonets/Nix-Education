SELECT DISTINCT CompanyName
FROM Orders
INNER JOIN(
	SELECT CompanyName, CustomerID
	FROM Customers
	) AS Companies
ON Orders.CustomerID = Companies.CustomerID
WHERE ShippedDate IS NULL
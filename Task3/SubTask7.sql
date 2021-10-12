SELECT ProductName
FROM Products
CROSS APPLY (
	SELECT SUM(Quantity) as TotalQuantity
	FROM [Order Details]
	WHERE Products.ProductID = [Order Details].ProductID
	GROUP BY ProductID
	) AS ProductOrders
WHERE TotalQuantity < 1000
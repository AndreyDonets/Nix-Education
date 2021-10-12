WITH TotalData as(
	SELECT 
		ProductID, 
		SUM(Quantity) AS QuantityProductsPerYear
	FROM [Order Details]
	CROSS APPLY(
		SELECT CustomerID
		FROM Orders
		WHERE [Order Details].OrderId = Orders.OrderID 
		AND YEAR(OrderDate) = 1997
	) AS CustmersInYear
	CROSS APPLY(
		SELECT CompanyName
		FROM Customers
		WHERE Customers.CustomerID = CustmersInYear.CustomerID
		AND Fax IS NOT NULL
	) AS CustomersWithFax
	GROUP BY ProductID
)
SELECT TOP 1
	CategoryName,
	SUM(QuantityProductsPerYear) AS CategoryQuantite
FROM(
	SELECT *
	FROM TotalData
	CROSS APPLY(
		SELECT CategoryID
		FROM Products
		WHERE TotalData.ProductID = Products.ProductID
	) AS CategoryData
	CROSS APPLY(
		SELECT CategoryName
		FROM Categories
		WHERE CategoryData.CategoryID = Categories.CategoryID
	) AS Cat
) AS CategoryWithQuantity
GROUP BY CategoryName
ORDER BY 2 DESC
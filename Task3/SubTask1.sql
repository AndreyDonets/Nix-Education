SELECT TOP 1
	ProductName
FROM dbo.Products
WHERE dbo.Products.CategoryID = 1
ORDER BY UnitPrice DESC
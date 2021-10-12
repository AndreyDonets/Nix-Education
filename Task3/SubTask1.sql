SELECT 
	CategoryName, 
	MAX(UnitPrice) as Price
FROM dbo.Products
INNER JOIN (
	SELECT 
		CategoryId, CategoryName
	FROM 
		dbo.Categories
	) AS CatName
ON dbo.Products.CategoryID = CatName.CategoryID
WHERE dbo.Products.CategoryID = 1
GROUP BY CategoryName
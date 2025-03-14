use GourmetShop;
SELECT * FROM dbo.Product
FOR JSON PATH, ROOT('Products');

SELECT * FROM dbo.Categories
FOR JSON PATH, ROOT('Categories');

SELECT * FROM dbo.Subcategories
FOR JSON PATH, ROOT('Subcategories');

SELECT * FROM dbo.Supplier
FOR JSON PATH, ROOT('Suppliers');

SELECT * FROM dbo.UserInfo
FOR JSON PATH, ROOT('Users');

WITH UserLoginsCte AS (
    SELECT Id
FROM dbo.AspNetUsers
)
SELECT 'e1db9f9d-85e1-4f92-8d6f-f9f7224335ee' AS RoleId, Id AS UserId
	FROM UserLoginsCte
	FOR JSON PATH, ROOT('UserRoles');

SELECT * FROM AspNetRoles WHERE Id = 'e1db9f9d-85e1-4f92-8d6f-f9f7224335ee';
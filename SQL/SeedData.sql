use GourmetShop;
SELECT * FROM dbo.Product
FOR JSON PATH, ROOT('Products');

SELECT * FROM dbo.Categories
FOR JSON PATH, ROOT('Categories');

SELECT * FROM dbo.Subcategories
FOR JSON PATH, ROOT('Subcategories');

SELECT * FROM dbo.Supplier
FOR JSON PATH, ROOT('Suppliers');

SELECT Id, RoleId AS AuthenticationId, FirstName, LastName, City, Country FROM [dbo].[User]
FOR JSON PATH, ROOT('Users');
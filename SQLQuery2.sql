CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Category NVARCHAR(100) NOT NULL,
    ProductName NVARCHAR(200) NOT NULL,
    Price INT NOT NULL,
    Quantity INT 
);

ALTER TABLE Products
ADD Image VARBINARY(MAX) NULL;

Select * FROM Products;
SELECT COUNT(*) FROM Products;
INSERT INTO Products (Category, ProductName, Price, Quantity, Image) VALUES (@category, @productname, @price, @quantity, @image);
DELETE FROM Products WHERE Id = @Id;
UPDATE Products SET Category=@category, ProductName=@productname, Price=@price, Quantity=@quantity, Image=@image WHERE Id = @id;
Select Id , Category , ProductName ,Price ,Quantity From Products 
WHERE Category LIKE @Search OR ProductName LIKE @Search ;
Select ProductName , Price  FROM Products WHERE Category = @category;
UPDATE Products SET Quantity = Quantity - @quantity  WHERE ProductName = @ProductName;


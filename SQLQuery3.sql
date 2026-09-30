CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT,
    OrderDate DATETIME DEFAULT GETDATE(),
    Phone NVARCHAR(20),
    Address NVARCHAR(200),
    TotalAmount INT,
    FOREIGN KEY (UserId) REFERENCES Users(id)
);

SELECT * FROM Orders;
SELECT COUNT(*) FROM Orders
SELECT SUM(TotalAmount) AS TotalIncome FROM Orders;
INSERT INTO Orders (UserId, Phone, Address, TotalAmount) OUTPUT INSERTED.OrderId VALUES (@UserId, @Phone, @Address, @TotalAmount);
SELECT OrderId, OrderDate, Phone , Address,TotalAmount FROM Orders WHERE UserId = @UserId ;
INSERT INTO Orders (UserId, Phone, Address, TotalAmount) OUTPUT INSERTED.OrderId VALUES (@UserId, @Phone, @Address, @TotalAmount);



CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT,
    ProductName NVARCHAR(100),
    Quantity INT,
    Price INT,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);
SELECT * FROM OrderItems;
SELECT ProductName, Quantity, Price FROM OrderItems WHERE OrderId = @OrderId;
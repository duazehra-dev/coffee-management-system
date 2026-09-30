CREATE TABLE Users (
    id INT PRIMARY KEY IDENTITY(1,1),
    FullName VARCHAR(100) NOT NULL,
    Username VARCHAR(50) NOT NULL ,
    Password VARCHAR(255) NOT NULL
);
ALTER TABLE Users
ALTER COLUMN Password VARCHAR(15) NOT NULL;

GO
SELECT * FROM Users;


INSERT INTO Users (FullName, Username, Password) VALUES (@FullName, @Username, @Password);

UPDATE Users SET Password = @password WHERE id = @id;

SELECT COUNT(*) FROM Users WHERE Username = @Username;

SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password;

SELECT COUNT(*) FROM Users;

SELECT id , FullName FROM Users WHERE Username = @Username AND Password = @Password
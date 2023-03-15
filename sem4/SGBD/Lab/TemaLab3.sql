
USE Magazin;

CREATE TABLE Products
(
	productId INT PRIMARY KEY IDENTITY,
	name VARCHAR(20),
	price FLOAT
);

CREATE TABLE Suplliers
(
	supllierId INT PRIMARY KEY IDENTITY,
	name VARCHAR(20),
	phoneNumber VARCHAR(15)
);

CREATE TABLE Orders
(
	orderId INT PRIMARY KEY IDENTITY,
	productId INT,
	supllierId INT,
	quantity INT,
	CHECK (quantity > 0),
	orderDate DATE,
	status VARCHAR(20),
	CHECK(status in ('new', 'sent', 'delivered')),
	FOREIGN KEY (productId) REFERENCES Products(productId),
	FOREIGN KEY (supllierId) REFERENCES Suplliers(supllierId)
);


-- validates if name is not null and contains more than 3 letters
GO;
CREATE OR ALTER FUNCTION validateName(@name varchar(20)) 
RETURNS INT 
AS
BEGIN
	DECLARE @error int;
	SET @error = 0;
	IF(@name = null OR len(@name) < 3)
		SET @error = 1;
	RETURN @error;
END

-- validates if price is a real positive number
GO;
CREATE OR ALTER FUNCTION validatePrice(@price float) 
RETURNS INT 
AS
BEGIN
	IF(@price < 0)
		RETURN 1;
	RETURN 0;
END

-- validates if a string is a valid phone number (contains only digits)
GO;
CREATE OR ALTER FUNCTION validatePhoneNumber(@phoneNumber varchar(15))
RETURNS INT 
AS
BEGIN
	IF (ISNUMERIC(@phoneNumber) = 1)
		RETURN 0;
	RETURN 1;
END

-- validates if quantity > 0
GO;
CREATE OR ALTER FUNCTION validateQuantity(@quantity int)
RETURNS INT 
AS
BEGIN
	IF(@quantity < 0)
		RETURN 1;
	RETURN 0;
END

-- validates if status id new, sent or delivered
GO;
CREATE OR ALTER FUNCTION validateStatus(@status varchar(20))
RETURNS INT 
AS
BEGIN
	IF(@status IN ('new', 'sent', 'delivered'))
		RETURN 0;
	RETURN 1;
END


GO;
CREATE OR ALTER PROCEDURE AddOrder 
	@productName varchar(20), 
	@price float,
	@supllierName varchar(20),
	@phoneNumber varchar(15),
	@quantity int,
	@orderDate date,
	@orderStatus varchar(20)
AS
BEGIN
	PRINT 'AddOrder'
	BEGIN TRAN
		PRINT 'Begin transaction'
		BEGIN TRY
			IF(dbo.validateName(@productName) <> 0)
				BEGIN RAISERROR('Product name not valid', 14, 1)
				END
			IF(dbo.validatePrice(@price) <> 0)
				BEGIN RAISERROR('Product price not valid', 14, 1)
				END
			IF(dbo.validateName(@supllierName) <> 0)
				BEGIN RAISERROR('Supllier name not valid', 14, 1)
				END
			IF(dbo.validatePhoneNumber(@phoneNumber) <> 0)
				BEGIN RAISERROR('Phone number not valid', 14, 1)
				END
			IF(dbo.validateQuantity(@quantity) <> 0)
				BEGIN RAISERROR('Quantity not valid', 14, 1)
				END
			IF(dbo.validateStatus(@orderStatus) <> 0)
				BEGIN RAISERROR('Status not valid', 14, 1)
				END

			PRINT 'Valid data'

			INSERT INTO Products (name, price) VALUES (@productName, @price)
			DECLARE @productId INT = SCOPE_IDENTITY()
			INSERT INTO Suplliers (name, phoneNumber) VALUES (@supllierName, @phoneNumber)
			DECLARE @supllierId INT = SCOPE_IDENTITY()
			INSERT INTO Orders (productId, supllierId, quantity, orderDate, status)
			VALUES (@productId, @supllierId, @quantity, @orderDate, @orderStatus)

			COMMIT TRAN
			PRINT 'Tansaction commited'
		END TRY
		BEGIN CATCH
			ROLLBACK TRAN
			PRINT ERROR_MESSAGE()
			PRINT 'Transaction rollback'
		END CATCH
END



GO;
CREATE OR ALTER PROCEDURE AddOrder2 
	@productName varchar(20), 
	@price float,
	@supllierName varchar(20),
	@phoneNumber varchar(15),
	@quantity int,
	@orderDate date,
	@orderStatus varchar(20)
AS
BEGIN
	PRINT 'AddOrder2'
	DECLARE @productId INT = -1;
	DECLARE @supllierId INT = -1;

	BEGIN TRAN
		PRINT 'Begin transaction for product'
		BEGIN TRY
			IF(dbo.validateName(@productName) <> 0)
				BEGIN RAISERROR('Product name not valid', 14, 1)
				END
			IF(dbo.validatePrice(@price) <> 0)
				BEGIN RAISERROR('Product price not valid', 14, 1)
				END
			INSERT INTO Products (name, price) VALUES (@productName, @price)
			SET @productId = SCOPE_IDENTITY()
			COMMIT TRAN
			PRINT 'Commit transaction for product'
		END TRY
		BEGIN CATCH
			ROLLBACK TRAN
			PRINT ERROR_MESSAGE()
			PRINT 'Transaction rollback for product'
		END CATCH


		BEGIN TRAN
		PRINT 'Begin transaction for supllier'
		BEGIN TRY
			IF(dbo.validateName(@supllierName) <> 0)
				BEGIN RAISERROR('Supllier name not valid', 14, 1)
				END
			IF(dbo.validatePhoneNumber(@phoneNumber) <> 0)
				BEGIN RAISERROR('Phone number not valid', 14, 1)
				END
			INSERT INTO Suplliers(name, phoneNumber) VALUES (@supllierName, @phoneNumber)
			SET @supllierId = SCOPE_IDENTITY()
			COMMIT TRAN
			PRINT 'Commit transaction for supllier'
		END TRY
		BEGIN CATCH
			ROLLBACK TRAN
			PRINT ERROR_MESSAGE()
			PRINT 'Transaction rollback for supllier'
		END CATCH

		
		BEGIN TRAN
		PRINT 'Begin transaction for order'
		BEGIN TRY
			IF(@productId = -1 or @supllierId = -1)
				BEGIN RAISERROR('Product or supllier could not be added', 14, 1)
				END
			IF(dbo.validateQuantity(@quantity) <> 0)
				BEGIN RAISERROR('Quantity not valid', 14, 1)
				END
			IF(dbo.validateStatus(@orderStatus) <> 0)
				BEGIN RAISERROR('Status not valid', 14, 1)
				END
			INSERT INTO Orders (productId, supllierId, quantity, orderDate, status)
			VALUES (@productId, @supllierId, @quantity, @orderDate, @orderStatus)
			COMMIT TRAN
			PRINT 'Commit transaction for order'
		END TRY
		BEGIN CATCH
			ROLLBACK TRAN
			PRINT ERROR_MESSAGE()
			PRINT 'Transaction rollback for order'
		END CATCH
END



-- productName, price, supllierName, phoneNumber, quantity, orderDate, orderStatus
EXEC AddOrder 'produs1', 30.6, 'firma1', '12345678', 10, '2022-04-05', 'delivered'
EXEC AddOrder 'produs1', -30.6, 'firma1', '12345678', 10, '2022-04-05', 'delivered'  -- invalid product
EXEC AddOrder 'produs1', 30.6, 'firma1', 'abc', 10, '2022-04-05', 'delivered'        -- invalid supllier
EXEC AddOrder 'produs1', 30.6, 'firma1', '12345678', 10, '2022-04-05', 'status'      -- invalid order

EXEC AddOrder2 'produs1', 30.6, 'firma1', '12345678', 10, '2022-04-05', 'delivered'
EXEC AddOrder2 'produs1', -30.6, 'firma1', '12345678', 10, '2022-04-05', 'delivered'  -- invalid product
EXEC AddOrder2 'produs1', 30.6, 'firma1', 'abc', 10, '2022-04-05', 'delivered'        -- invalid supllier
EXEC AddOrder2 'produs1', 30.6, 'firma1', '12345678', 10, '2022-04-05', 'status'      -- invalid order


SELECT * FROM Products
SELECT * FROM Suplliers
SELECT * FROM Orders

DELETE FROM Orders
DELETE FROM Products
DELETE FROM Suplliers

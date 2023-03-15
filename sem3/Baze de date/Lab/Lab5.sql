USE MagazinCosmeticeOnline
GO

-- validare parametri CRUD Clienti
CREATE or ALTER FUNCTION validare_varsta (@varsta INT)
RETURNS VARCHAR(50)
AS
BEGIN
	DECLARE @mesaj VARCHAR(50)
	SET @mesaj = ''
	IF @varsta < 16
		SET @mesaj = @mesaj + 'Varsta invalida!'
	RETURN @mesaj
END
GO

-- CRUD Clienti
CREATE or ALTER PROCEDURE CRUD_Clienti
	@nume VARCHAR(20), @prenume VARCHAR(20), @varsta INT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @msg VARCHAR(50)
	SET @msg = dbo.validare_varsta(@varsta)
	
	IF @msg = '' -- validare cu succes
	BEGIN
		-- insert
		INSERT INTO Clienti(nume, prenume, varsta)
		VALUES (@nume, @prenume, @varsta)
		-- select
		SELECT * FROM Clienti
		-- update
		UPDATE Clienti SET nume = 'Xulescu'
		WHERE prenume like 'M%'  
		-- delete
		DELETE FROM Clienti
		WHERE varsta < 23

		PRINT 'CRUD pentru tabela Clienti'
	END
	ELSE
		RAISERROR(@msg, 15, 1)	

	SET NOCOUNT OFF
END
GO

EXEC CRUD_Clienti 'Costan', 'Mara', 12
EXEC CRUD_Clienti 'Costan', 'Mara', 25


-- validare parametri CRUD Vouchere
GO
CREATE or ALTER FUNCTION validare_procent (@procent DECIMAL(5, 4))
RETURNS VARCHAR(50)
AS
BEGIN
	DECLARE @mesaj VARCHAR(50)
	SET @mesaj = ''
	IF @procent < 0 or @procent >= 1
		SET @mesaj = @mesaj + 'Procent invalid!'
	RETURN @mesaj
END
GO

-- CRUD Vouchere
CREATE or ALTER PROCEDURE CRUD_Vouchere
	@procent DECIMAL(5, 4), @dataEx DATE
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @msg VARCHAR(50)
	SET @msg = dbo.validare_procent(@procent)
	
	IF @msg = '' -- validare cu succes
	BEGIN
		-- insert
		INSERT INTO Vouchere(procentReducere, dataExpirare)
		VALUES (@procent, @dataEx)
		-- select
		SELECT * FROM Vouchere
		-- update
		UPDATE Vouchere SET procentReducere = 0.1890
		WHERE dataExpirare between '2021-11-1' and '2021-12-3'
		-- delete
		DELETE FROM Vouchere
		WHERE  dataExpirare < '2022-01-05'

		PRINT 'CRUD pentru tabela Vouchere'
	END
	ELSE
		RAISERROR(@msg, 15, 1)	

	SET NOCOUNT OFF
END
GO

EXEC CRUD_Vouchere 1.45, '2021-01-01'
EXEC CRUD_Vouchere 0.4321, '2050-12-03'


-- validare parametri CRUD ComenziProduse
GO
CREATE or ALTER FUNCTION validare_ComenziProduse (@idC INT, @idP INT)
RETURNS VARCHAR(50)
AS
BEGIN
	DECLARE @mesaj VARCHAR(50)
	SET @mesaj = ''
	IF not exists (SELECT idComanda FROM Comenzi WHERE idComanda = @idC)
		SET @mesaj = @mesaj + 'Comanda inexistenta!'
	IF not exists (SELECT idProdus FROM Produse WHERE idProdus = @idP)
		SET @mesaj = @mesaj + 'Produs inexistent!'
	IF exists (SELECT * FROM ComenziProduse WHERE idComanda = @idC and idProdus = @idP)
		SET @mesaj = @mesaj + 'Inregistrare existenta!'
	RETURN @mesaj
END
GO


-- CRUD ComenziProduse m-n
CREATE or ALTER PROCEDURE CRUD_ComenziProduse
	@idC INT, @idP INT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @msg VARCHAR(50)
	SET @msg = dbo.validare_ComenziProduse(@idC, @idP)
	
	IF @msg = '' -- validare cu succes
	BEGIN
		-- insert
		INSERT INTO ComenziProduse(idComanda, idProdus)
		VALUES (@idC, @idP)
		-- select
		SELECT * FROM ComenziProduse
		-- update
		PRINT 'Nu pot face update pe pk!'  
		-- delete
		DELETE FROM ComenziProduse
		WHERE idProdus < 5

		PRINT 'CRUD pentru tabela ComenziProduse'
	END
	ELSE
		RAISERROR(@msg, 15, 1)	

	SET NOCOUNT OFF
END
GO

EXEC CRUD_ComenziProduse 2, 234 -- comanda si produs inexistente
EXEC CRUD_ComenziProduse 19116, 234 -- produs inexistent
EXEC CRUD_ComenziProduse 19116, 24 -- inregistrare existenta
EXEC CRUD_ComenziProduse 19119, 13



-- creare indecsi
CREATE NONCLUSTERED INDEX idx_Clienti ON Clienti (varsta ASC) INCLUDE (prenume)
CREATE NONCLUSTERED INDEX idx_Vouchere ON Vouchere (dataExpirare DESC) INCLUDE (procentReducere)
CREATE NONCLUSTERED INDEX idx_ComenziProduse ON ComenziProduse (idProdus) INCLUDE (idComanda)

-- creare view-uri
GO
CREATE VIEW view_Clienti AS
	SELECT prenume, varsta FROM Clienti WHERE varsta > 40
GO

SELECT * FROM view_Clienti


GO
CREATE VIEW view_Vouchere AS
	SELECT dataExpirare, procentReducere FROM Vouchere WHERE dataExpirare > '2022-01-05'
GO

SELECT * FROM view_Vouchere


GO
CREATE VIEW view_ComenziProduse AS
	SELECT idComanda, idProdus FROM ComenziProduse WHERE idProdus > 13
GO

SELECT * FROM view_ComenziProduse

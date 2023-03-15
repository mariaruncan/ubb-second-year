INSERT INTO Tables
VALUES ('Angajati'), ('Comenzi'), ('ComenziProduse')



-- view pentru idClient, valoare si data comenzilor
GO
CREATE VIEW View1 AS
	SELECT idClient, valoare, data 
	FROM Comenzi
GO

-- interactiuni angajati-clienti
GO
CREATE VIEW View2 AS
	SELECT A.CNP, A.nume, C.idClient
	FROM Angajati as A inner join Comenzi as C on A.CNP = C.angajat
GO

-- ce suma de bani a gestionat fiecare angajat?
GO
CREATE VIEW View3 AS
	SELECT A.CNP, A.nume, A.prenume, SUM(C.valoare) as sumaGestionata
	FROM Angajati as A inner join Comenzi as C on A.CNP = C.angajat
	GROUP BY A.CNP, A.nume, A.prenume
GO

-- inserare in Views
INSERT INTO Views
VALUES ('View1'), ('View2'), ('View3')

-- select view 1,2,3
GO
CREATE or ALTER PROCEDURE selectView1 AS
	SELECT * FROM View1
GO

GO
CREATE or ALTER PROCEDURE selectView2 AS
	SELECT * FROM View2
GO

GO
CREATE or ALTER PROCEDURE selectView3 AS
	SELECT * FROM View3
GO


-- inserare in Tests
INSERT INTO Tests
VALUES ('deleteTable'), ('insertTable'), ('selectView')


-- inserare in TestViews
INSERT INTO TestViews
VALUES (3, 1), (3, 2), (3, 3)


-- inserare in TestTables
INSERT INTO TestTables
VALUES (1, 1, 1000, 3), (1, 2, 1000, 2), (1, 3, 1000, 1),
(2, 1, 1000, 1), (2, 2, 1000, 2), (2, 3, 1000, 3)


SET NOCOUNT ON

-- creeare proceduri stocate pentru inserare, stergere si evaluare view-uri
-- insert in Angajati
GO
CREATE or ALTER PROCEDURE insertInAngajati AS
	DECLARE @NoOfRows int
	DECLARE @i int
	DECLARE @salariu money
	DECLARE @nume varchar(20)
	DECLARE @prenume varchar(20)

	SELECT TOP 1 @NoOfRows = NoOfRows 
	FROM TestTables
	WHERE TableID = 1 and TestID = 1

	SET @i = 0
	SET @salariu = 1500
	

	WHILE @i < @NoOfRows
	BEGIN
		SET @nume = 'nume' + CONVERT(varchar(5), @i)
		SET @prenume = 'prenume' + CONVERT(varchar(5), @i)
		SET @salariu = @salariu + 10
		INSERT INTO Angajati (nume, prenume, salariu)
		VALUES (@nume, @prenume, @salariu)
		SET @i = @i + 1
	END
GO

-- delete from Angajati
GO
CREATE or ALTER PROCEDURE deleteFromAngajati AS
	DECLARE @NoOfRows int

	SELECT TOP 1 @NoOfRows = NoOfRows 
	FROM TestTables
	WHERE TestID = 2 AND TableID = 1
	
	DECLARE @i int
	SET @i = 0
	WHILE @i < @NoOfRows
	BEGIN
		DELETE FROM Angajati
		WHERE CNP = (SELECT MAX(CNP) FROM Angajati)
		SET @i = @i + 1
	END
GO








-- insert in Comenzi
GO
CREATE or ALTER PROCEDURE insertInComenzi
AS
BEGIN
	DECLARE @fkAngajat int
	DECLARE @fkAdresa int
	DECLARE @fkClient int
	DECLARE @NoOfRows int
	DECLARE @i int

	SELECT TOP 1 @fkAngajat = CNP FROM Angajati
	SELECT TOP 1 @fkAdresa = idAdresa FROM Adrese
	SELECT TOP 1 @fkClient = idClient FROM Clienti

	SELECT TOP 1 @NoOfRows = NoOfRows 
	FROM TestTables
	WHERE TestID = 1 AND TableID = 2
	
	SET @i = 0

	WHILE @i < @NoOfRows
	BEGIN
		INSERT INTO Comenzi (data, mod_plata, valoare, angajat, idAdresa, idClient)
		VALUES (SYSDATETIME(), 'cash', @i*10, @fkAngajat, @fkAdresa, @fkClient)
		SET @i = @i + 1
	END
END
GO

-- delete from Comenzi
GO
CREATE or ALTER PROCEDURE deleteFromComenzi AS
	DECLARE @NoOfRows int

	SELECT TOP 1 @NoOfRows = NoOfRows 
	FROM TestTables
	WHERE TestID = 2 AND TableID = 2
	
	DECLARE @i int
	SET @i = 0
	WHILE @i < @NoOfRows
	BEGIN
		DELETE FROM Comenzi
		WHERE idComanda = (SELECT MAX(idComanda) FROM Comenzi)
		SET @i = @i + 1
	END
GO








-- insert in ComenziProduse
GO
CREATE or ALTER PROCEDURE insertInComenziProduse
AS
BEGIN
	DROP TABLE ComenziProduse
	SELECT idComanda, idProdus
	INTO ComenziProduse
	FROM Comenzi cross join Produse
END
GO

-- delete from ComenziProduse
GO
CREATE or ALTER PROCEDURE deleteFromComenziProduse AS
	DELETE FROM ComenziProduse
GO















GO
CREATE or ALTER PROCEDURE testAngajati
AS
BEGIN
	
	DECLARE @ds DATETIME	-- start time test
	DECLARE @di DATETIME	-- intermediate time test
	DECLARE @de DATETIME	-- end time test

	SET @ds = GETDATE()
	EXEC deleteFromAngajati-- delete from table
	EXEC insertInAngajati -- insert into table

	SET @di=GETDATE()
	EXEC selectView2 -- evaluate (select from) view
	SET @de=GETDATE()

	-------------------
	-- if you want to see the difference of these 2 times, you can use DATEDIFF
	--Print DATEDIFF(@de, @ds)

	DECLARE @description NVARCHAR(2000)
	SET @description = 'Test tabela Angajati + view2'

	INSERT INTO TestRuns(Description, StartAt, EndAt)
	VALUES(@description, @ds, @de);


	-- extract the TestRunId and “combine” it with the Id of table involved and 
	--also with the view involved in the corresponding tables

	DECLARE @TestRunID INT = (SELECT MAX(TestRunID) FROM TestRuns)
	DECLARE @TableID INT = (SELECT TOP 1 TableId FROM Tables)

	INSERT INTO TestRunTables(TestRunID, TableID, StartAt, EndAt)
	VALUES(@TestRunID, @TableID, @ds, @di)

	DECLARE @ViewID INT = (SELECT TOP 1 ViewID FROM Views)
	INSERT INTO TestRunViews(TestRunID, ViewID, StartAt, EndAt)
	VALUES(@TestRunID, @ViewID, @di, @de)

END;


GO
CREATE or ALTER PROCEDURE testComenzi
AS
BEGIN
	DECLARE @ds DATETIME	-- start time test
	DECLARE @di DATETIME	-- intermediate time test
	DECLARE @de DATETIME	-- end time test

	SET @ds = GETDATE()
	EXEC deleteFromComenzi -- delete from table
	EXEC insertInComenzi -- insert into table

	SET @di=GETDATE()
	EXEC selectView1 -- evaluate (select from) view
	SET @de=GETDATE()

	DECLARE @description NVARCHAR(2000)
	SET @description = 'Test tabela Comenzi + view1'

	INSERT INTO TestRuns(Description,StartAt,EndAt)
	VALUES(@description,@ds,@de);

	-- extract the TestRunId and “combine” it with the Id of table involved and 
	--also with the view involved in the corresponding tables

	DECLARE @TestRunID INT = (SELECT MAX(TestRunID) FROM TestRuns)
	
	SELECT *
	INTO REZ1
	FROM Tables
	DELETE TOP (1) FROM REZ1
	
	DECLARE @TableID INT = (SELECT TOP 1 TableId FROM REZ1)
	DROP TABLE REZ1

	INSERT INTO TestRunTables(TestRunID,TableID,StartAt,EndAt)
	VALUES(@TestRunID,@TableID,@ds,@di)

	SELECT *
	INTO REZ2
	FROM Views
	DELETE TOP (2) FROM REZ2

	DECLARE @ViewID INT = (SELECT TOP 1 ViewID FROM REZ2)
	DROP TABLE REZ2

	INSERT INTO TestRunViews(TestRunID,ViewID,StartAt,EndAt)
	VALUES(@TestRunID,@ViewID,@di,@de)

END;


GO
CREATE or ALTER PROCEDURE testComenziProduse
AS
BEGIN

	DECLARE @ds DATETIME	-- start time test
	DECLARE @di DATETIME	-- intermediate time test
	DECLARE @de DATETIME	-- end time test

	SET @ds = GETDATE()
	EXEC deleteFromComenziProduse -- delete from table
	EXEC insertInComenziProduse -- insert into table

	SET @di=GETDATE()
	EXEC selectView3 -- evaluate (select from) view
	SET @de=GETDATE()

	DECLARE @description NVARCHAR(2000)
	SET @description = 'Test tabela ComenziProduse + view3'

	INSERT INTO TestRuns(Description,StartAt,EndAt)
	VALUES(@description,@ds,@de);

	-- extract the TestRunId and “combine” it with the Id of table involved and 
	--also with the view involved in the corresponding tables

	DECLARE @TestRunID INT = (SELECT MAX(TestRunID) FROM TestRuns)
	
	SELECT *
	INTO REZ3
	FROM Tables
	DELETE TOP (2) FROM REZ3
	
	DECLARE @TableID INT = (SELECT TOP 1 TableId FROM REZ3)
	DROP TABLE REZ3

	INSERT INTO TestRunTables(TestRunID,TableID,StartAt,EndAt)
	VALUES(@TestRunID,@TableID,@ds,@di)

	SELECT *
	INTO REZ4
	FROM Views
	DELETE TOP (1) FROM REZ4

	DECLARE @ViewID INT = (SELECT TOP 1 ViewID FROM REZ4)
	DROP TABLE REZ4

	INSERT INTO TestRunViews(TestRunID,ViewID,StartAt,EndAt)
	VALUES(@TestRunID,@ViewID,@di,@de)

END;


GO
CREATE or ALTER PROCEDURE main_testare AS

delete from TestRuns
delete from TestRunTables
delete from TestRunViews

BEGIN	
	SELECT *
	INTO REZ
	FROM Tables

	select * from Tables

	DECLARE @n INT = (SELECT COUNT(*) FROM Tables)
	WHILE @n > 0
	BEGIN
		DECLARE @nume NVARCHAR(50);
		SELECT TOP 1 @nume=Name FROM REZ
		PRINT('TEST tabela ' + @nume)
		EXEC('test'+@nume)

		DELETE TOP (1) FROM REZ
		SET @n=@n-1

	END

	DROP TABLE REZ
END;

exec main_testare

select * from TestRuns
select * from TestRunTables
select * from TestRunViews


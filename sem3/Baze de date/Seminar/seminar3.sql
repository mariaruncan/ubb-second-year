-- 1.
CREATE DATABASE Seminar3
GO
USE Seminar3
GO

CREATE TABLE Tipuri(
	tid INT PRIMARY KEY IDENTITY,
	nume VARCHAR(20),
	descriere VARCHAR(50)
);
CREATE TABLE Petreceri(
	pid INT PRIMARY KEY IDENTITY,
	nume VARCHAR(20),
	buget MONEY,
	data_petrecerii DATETIME,
	spatiu VARCHAR(50),
	tid INT FOREIGN KEY REFERENCES Tipuri(tid)
 );

 -- 2.
GO
CREATE PROCEDURE adauga_tip
(@Nume VARCHAR(20), @Descriere VARCHAR(50))
AS
BEGIN
INSERT INTO Tipuri(nume, descriere) values (@Nume, @Descriere)
END;
GO

EXEC adauga_tip 'Halloween', '31 octombrie'
EXEC adauga_tip 'Ciubar', 'cu apa'
SELECT * FROM Tipuri


-- 3.
GO
CREATE PROCEDURE adauga_pertecere
(@Nume VARCHAR(20), @Buget MONEY, @Data DATETIME, @Spatiu VARCHAR(50), @Tip VARCHAR(20))
AS
BEGIN
DECLARE
@Tid INT = (SELECT TOP 1 Tid FROM Tipuri WHERE nume = @Tip) 
IF @Tid IS NULL
	THROW 50001, 'Nu s-a gasit tipul!', 1;
ELSE
	INSERT INTO Petreceri (nume, buget, data_petrecerii, spatiu, tid)
	VALUES(@Nume, @Buget, @Data, @Spatiu, @Tid)

END;
GO

EXEC adauga_pertecere 'Halloween Party', 50, '2021-10-31 19:00', 'Forms Space', 'Halloween'
EXEC adauga_pertecere 'Halloween Party', 50, '2021-10-31 19:00', 'Forms Space', 'Paste'
SELECT * FROM Petreceri
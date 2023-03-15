-- Creați o bază de date pentru gestiunea mersului trenurilor. Baza de date va conține informații despre rutele tuturor trenurilor.
-- Entitățile de interes pentru domeniul problemei sunt: trenuri, tipuri de tren, stații și rute.
-- Fiecare tren are un nume și aparține unui tip.Tipul trenului are o descriere.
-- Fiecare rută are un nume, un tren asociat și o listă de stații cu ora sosirii și ora plecării pentru fiecare stație.
-- Ora sosirii și ora plecării sunt reprezentate ca perechi oră/minut(exemplu: trenul ajunge la 5 PM și pleacăla 5:10 PM). 
-- Stația are un nume.

-- 1) Scrieți un script SQL care creează un model relațional pentru a reprezenta datele. 

CREATE DATABASE Seminar6
USE Seminar6
GO

CREATE TABLE Tipuri
(
	id INT IDENTITY PRIMARY KEY,
	descriere VARCHAR(200)
);

CREATE TABLE Trenuri
(
	id INT IDENTITY PRIMARY KEY,
	nume VARCHAR(20),
	tipId INT FOREIGN KEY REFERENCES Tipuri(id)
)

CREATE TABLE Statii
(
	id INT IDENTITY PRIMARY KEY,
	nume VARCHAR(50)
);

CREATE TABLE Rute
(
	id INT IDENTITY PRIMARY KEY,
	nume VARCHAR(50),
	idTren INT FOREIGN KEY REFERENCES Trenuri(id)
);

CREATE TABLE StatiiRute
(
	idStatie INT FOREIGN KEY REFERENCES Statii(id),
	idRuta INT FOREIGN KEY REFERENCES Rute(id),
	oraSosirii TIME,
	oraPlecarii TIME,
	CONSTRAINT pk_StatiiRute PRIMARY KEY (idStatie, idRuta)
);

-- 2) Creați o procedură stocată care primește un tip de tren și adaugă tipul de tren în tabel, dacă acesta nu a fost deja adăugat.
GO
CREATE PROCEDURE adaugaTip (@descriere VARCHAR(200))
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Tipuri WHERE descriere = @descriere)
		INSERT INTO Tipuri VALUES (@descriere)
END
GO

EXEC adaugaTip 'international'
EXEC adaugaTip 'regional'
EXEC adaugaTip 'interjudetean'
EXEC adaugaTip 'expres'
EXEC adaugaTip 'ocazional'
SELECT * FROM Tipuri

-- 3) Creați o procedură stocată care primește un tren, un tip de tren și adaugă trenul în tabel. Dacă tipul trenului nu există, se va afișa un mesaj de eroare.
GO
CREATE PROCEDURE adaugaTren (@numeTren VARCHAR(20), @descriereTip VARCHAR(200))
AS
BEGIN
	DECLARE @idTip INT
	SELECT TOP 1 @idTip = id FROM Tipuri WHERE descriere = @descriereTip
	IF (@idTip is null)
		RAISERROR('Tip invalid!', 15, 1)
	ELSE
		INSERT INTO Trenuri(nume, tipId) VALUES (@numeTren, @idTip)
END
GO

EXEC adaugaTren 'Thomas the Thank Engi', 'international' 
EXEC adaugaTren 'IRN-2042', 'regional'
EXEC adaugaTren 'James The Thank', 'abc'
EXEC adaugaTren 'IR-23', 'expres'
EXEC adaugaTren 'IRN-1065', 'ocazional'
SELECT * FROM Trenuri

-- 4)Creați o procedură stocată care primește o rută, un tren și adaugă ruta în tabel. Dacă trenul nu există, se va afișa un mesaj de eroare. 
GO
CREATE PROCEDURE adaugaRuta (@numeRuta VARCHAR(50), @numeTren VARCHAR(20))
AS
BEGIN
	DECLARE @idTren INT
	SELECT TOP 1 @idTren = id FROM Trenuri WHERE nume = @numeTren

	IF @idTren is not null
		INSERT INTO Rute (nume, idTren) VALUES (@numeRuta, @idTren)
	ELSE
		RAISERROR('Numele trenului este invalid!', 15, 1)
END
GO

EXEC adaugaRuta 'Cluj-Campia', 'Thomas the Thank Eng'
EXEC adaugaRuta 'Oradea-Bucuresti', 'CFR'
EXEC adaugaRuta 'Oradea-Campia', 'IRN-1065'
EXEC adaugaRuta 'Iasi-Timisoara', 'IRN-2042'

SELECT * FROM Rute

-- 5) Creați o procedură stocată care primește o stație și adaugă stația în tabel, dacă aceasta nu a fost deja adăugată. 
GO
CREATE PROCEDURE adaugaStatie (@numeStatie VARCHAR(50))
AS
BEGIN
	DECLARE @idStatie INT
	SELECT TOP 1 @idStatie = id FROM Statii WHERE nume = @numeStatie
	
	IF @idStatie is null
		INSERT INTO Statii(nume) VALUES (@numeStatie)
END 
GO

EXEC adaugaStatie 'Siret'
EXEC adaugaStatie 'Suceava'
EXEC adaugaStatie 'Cluj Napoca'
EXEC adaugaStatie 'Siret'

SELECT * FROM Statii


-- 6) Creați o procedură stocată care primește o rută, o stație, ora sosirii, ora plecării și adaugă 
-- noua stație rutei. Dacă stația există deja, se actualizează ora sosirii și ora plecării. 
GO
CREATE OR ALTER PROCEDURE adaugaStatiiRute 
(@numeRuta VARCHAR(50), @numeStatie VARCHAR(50), @oraSosire TIME, @oraPlecare TIME)
AS
BEGIN
	DECLARE @idRuta INT
	DECLARE @idStatie INT

	SELECT TOP 1 @idRuta = id FROM Rute WHERE nume = @numeRuta
	SELECT TOP 1 @idStatie = id FROM Statii WHERE nume = @numeStatie

	IF @idRuta is not null AND @idStatie is not null
	BEGIN
		IF EXISTS(SELECT * FROM StatiiRute WHERE idRuta = @idRuta and idStatie = @idStatie)
			UPDATE StatiiRute SET oraSosirii = @oraSosire, oraPlecarii = @oraPlecare
			WHERE idRuta = @idRuta and idStatie = @idStatie
		ELSE
			INSERT INTO StatiiRute(idRuta, idStatie, oraPlecarii, oraSosirii) VALUES 
			(@idRuta, @idStatie, @oraPlecare, @oraSosire)
	END
	ELSE
		RAISERROR('Cel putin unul dintre nume este invalid!', 15, 1)
END
GO

SELECT * FROM Rute
SELECT * FROM Statii

EXEC adaugaStatiiRute 'Cluj-Campia', 'Suceava', '15:30:15', '17:15:00'
EXEC adaugaStatiiRute'Oradea-Campia', 'Siret', '13:45:00', '14:50:15'
EXEC adaugaStatiiRute'Oradea-Campia', 'Siret', '20:45:00', '21:50:15'
EXEC adaugaStatiiRute 'Oradea-Cluj', 'Siret', '20:45:00', '21:50:15'
EXEC adaugaStatiiRute 'Oradea-Campia', 'Brasov', '20:45:00', '21:50:15'
EXEC adaugaStatiiRute 'Oradea-Cluj', 'Brasov', '20:45:00', '21:50:15'

SELECT * FROM Rute
SELECT * FROM Statii
SELECT * FROM StatiiRute


-- 7) Creați un view care afișează numele rutelor care conțin toate stațiile. 
GO
CREATE VIEW ruteComplete AS
	SELECT R.nume From Rute as R inner join StatiiRute SR on R.id = SR.idRuta
	GROUP BY R.id, R.nume
	HAVING count(*) = (SELECT count(*) FROM Statii)
GO

SELECT * FROM ruteComplete

-- 8) Creați o funcție care afișează toate stațiile care au mai mult de un tren
-- la un anumit moment din zi.


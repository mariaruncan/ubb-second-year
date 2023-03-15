USE MagazinCosmeticeOnline
Go

CREATE TABLE Versiuni
(
	nrVersiune int
);


INSERT INTO Versiuni (nrVersiune) VALUES (0);
SELECT * FROM Versiuni;

GO
CREATE PROCEDURE do_1
AS
BEGIN
	ALTER TABLE Angajati
	ALTER COLUMN nume varchar(30)
	PRINT 'do_1: Angajati.nume varchar(20) -> varchar(30)'
END
GO

EXEC do_1;

GO
CREATE PROCEDURE undo_1
AS
BEGIN
	ALTER TABLE Angajati
	ALTER COLUMN nume varchar(20)
	PRINT 'undo_1: Angajati.nume varchar(30) -> varchar(20)'
END
GO

EXEC undo_1

GO
CREATE PROCEDURE do_2
AS
BEGIN
	ALTER TABLE Comenzi
	ADD CONSTRAINT angajat_6 DEFAULT 6 FOR angajat
	PRINT 'do_2: Comenzi.angajat are default valoarea 6 (constraint angajat_6)'
END
GO

EXEC do_2

GO
CREATE PROCEDURE undo_2
AS
BEGIN
	ALTER TABLE Comenzi
	DROP CONSTRAINT angajat_6
	PRINT 'undo_2: Comenzi.angajat; a fost stearsa valoarea default (constraint angajat_6)'
END
GO

EXEC undo_2

GO
CREATE PROCEDURE do_3
AS
BEGIN
	CREATE TABLE AngajatiProducatori
	(
		angajat int foreign key references Angajati(CNP)
	);
	PRINT 'do_3: am creat tabela AngajatiProducatori cu un camp angajat (FK pt Angajati.CNP)'
END
GO

EXEC do_3

GO
CREATE PROCEDURE undo_3
AS
BEGIN
	DROP TABLE AngajatiProducatori
	PRINT 'undo_3: am sters tabela AngajatiProducatori'
END
GO

EXEC undo_3

GO
CREATE PROCEDURE do_4
AS
BEGIN
	ALTER TABLE AngajatiProducatori
	ADD producator int
	PRINT 'do_4: am adaugat coloana producator in AngajatiProducatori'
END
GO

EXEC do_4

GO
CREATE PROCEDURE undo_4
AS
BEGIN
	ALTER TABLE AngajatiProducatori
	DROP COLUMN producator
	PRINT 'undo_4: am sters coloana producator din AngajatiProducatori'
END
GO

EXEC undo_4

GO
CREATE PROCEDURE do_5
AS
BEGIN
	ALTER TABLE AngajatiProducatori
	ADD CONSTRAINT fk_Producator FOREIGN KEY (producator) REFERENCES Producatori(idProducator)
	PRINT 'do_5: am adaugat fk_Producator pe AngajatiProducatori.producator references Producatori.idProducator '
END
GO

EXEC do_5

GO
CREATE PROCEDURE undo_5
AS
BEGIN
	ALTER TABLE AngajatiProducatori
	DROP CONSTRAINT fk_Producator
	PRINT 'undo_5: am sters fk_Producator'
END
GO

EXEC undo_5

GO
CREATE PROCEDURE main
@vsNoua int
AS
BEGIN
	DECLARE @curenta int;
	SET @curenta = 0;
	DECLARE @comanda VARCHAR(10)
	SELECT TOP 1 @curenta = nrVersiune FROM Versiuni

	IF @vsNoua < 0 OR @vsNoua > 5
		PRINT 'Versiune indisponibila!'

	ELSE IF @vsNoua = @curenta
		PRINT 'Suntem deja in versiunea ceruta!'

	ELSE IF @curenta < @vsNoua
	BEGIN
		SET @curenta = @curenta + 1
		WHILE @curenta <= @vsNoua
		BEGIN
			SET @comanda = 'do_' + CONVERT(VARCHAR(10), @curenta)
			EXEC @comanda
			SET @curenta = @curenta + 1
		END
		UPDATE Versiuni SET nrVersiune = @vsNoua
	END

	ELSE
	BEGIN
		WHILE @curenta > @vsNoua
		BEGIN
			SET @comanda = 'undo_' + CONVERT(VARCHAR(10), @curenta)
			EXEC @comanda
			SET @curenta = @curenta - 1
		END
		UPDATE Versiuni SET nrVersiune = @vsNoua
	END

END
GO

EXEC main 0

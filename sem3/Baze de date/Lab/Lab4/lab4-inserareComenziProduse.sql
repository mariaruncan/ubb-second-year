-- inserare in ComenziProduse PK=FK+FK
CREATE PROCEDURE inserareComenziProduse
AS
BEGIN
	INSERT INTO ComenziProduse
	SELECT C.idComanda, P.idProdus
	FROM Comenzi as C CROSS JOIN Produse as P
END


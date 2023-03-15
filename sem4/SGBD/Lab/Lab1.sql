CREATE DATABASE Lab1SGBD
GO
USE Lab1SGBD

CREATE TABLE Alimente
(
	cod_a INT PRIMARY KEY IDENTITY,
	nume VARCHAR(100),
	pret INT,
	data_expirare DATE
);

INSERT INTO Alimente (nume, pret, data_expirare) VALUES
('paine', 5, '2022-03-04'), ('cascaval', 20, '2022-05-05'),
('suc la doza', 5, '2022-06-10'), ('supa la plic', 1, '2024-03-03');

SELECT * FROM Alimente
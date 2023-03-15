CREATE DATABASE Seminar2226SGBD;
GO;
USE Seminar2226SGBD;

CREATE TABLE Orase
(
	cod_o INT PRIMARY KEY IDENTITY,
	nume VARCHAR(100),
	judet VARCHAR(100)
);

CREATE TABLE Cartiere
(
	cod_c INT PRIMARY KEY IDENTITY,
	nume VARCHAR(100),
	siguranta INT,
	cod_o INT FOREIGN KEY REFERENCES Orase(cod_o)
);

INSERT INTO Orase (nume, judet) VALUES ('Cluj-Napoca', 'Cluj'),
('Dej', 'Cluj'), ('Ramnicu Valcea', 'Valcea'), ('Mangalia', 'Constanta'),
('Dragasani', 'Valcea')
SELECT * FROM Orase 

INSERT INTO Cartiere(nume, siguranta, cod_o) VALUES ('Gheorgheni', 57, 1),
('Manastur',  45, 1), ('Marasti', 0, 1), ('Ostroveni', 75, 3), 
('Neptun', 70, 4), ('Dealul Florilor', 76, 2), ('Nord', 8, 3);
SELECT * FROM Cartiere
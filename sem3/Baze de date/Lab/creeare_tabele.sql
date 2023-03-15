CREATE DATABASE MagazinCosmeticeOnline
GO
USE MagazinCosmeticeOnline
GO

CREATE TABLE Angajati
(
	CNP INT PRIMARY KEY IDENTITY,
	nume VARCHAR(20),
	prenume VARCHAR(20),
	salariu MONEY
);

CREATE TABLE ConturiBancare
(
	CNP INT FOREIGN KEY REFERENCES Angajati(CNP),
	IBAN INT,
	sold MONEY,
	CONSTRAINT pk_ConturiBancare PRIMARY KEY(CNP)
);


CREATE TABLE Producatori
(
	idProducator INT PRIMARY KEY IDENTITY,
	nume VARCHAR(20),
	telefon INT,
	mail VARCHAR(50)
);


CREATE TABLE Produse
(
	idProdus INT PRIMARY KEY IDENTITY,
	nume VARCHAR(50),
	tip VARCHAR(20),
	volum INT,
	pret MONEY,
	idProducator INT FOREIGN KEY REFERENCES Producatori(idProducator)
);


CREATE TABLE Adrese
(
	idAdresa INT PRIMARY KEY IDENTITY,
	judet VARCHAR(20),
	oras VARCHAR(20),
	strada VARCHAR(20),
	numar INT
);


CREATE TABLE Clienti
(
	idClient INT PRIMARY KEY IDENTITY,
	nume VARCHAR(20),
	prenume VARCHAR(20),
	varsta INT CHECK (varsta>=16)
);


CREATE TABLE Comenzi
(
	idComanda INT PRIMARY KEY IDENTITY,
	data DATETIME,
	mod_plata VARCHAR(20) CHECK (mod_plata='cash' OR mod_plata='card'),
	valoare MONEY,
	angajat INT FOREIGN KEY REFERENCES Angajati(CNP),
	idAdresa INT FOREIGN KEY REFERENCES Adrese(idAdresa),
	idClient INT FOREIGN KEY REFERENCES Clienti(idClient)
);


CREATE TABLE ComenziProduse
(
	idComanda INT FOREIGN KEY REFERENCES Comenzi(idComanda),
	idProdus INT FOREIGN KEY REFERENCES Produse(idProdus)
	CONSTRAINT pk_Prod PRIMARY KEY(idComanda, idProdus)
);


CREATE TABLE CarduriFidelitate
(
	idClient INT FOREIGN KEY REFERENCES Clienti(idClient),
	numarPuncte INT CHECK (numarPuncte>=0),
	dataExpirare DATE,
	CONSTRAINT pk_CarduriFidelitate PRIMARY KEY(idClient)
);


CREATE TABLE Vouchere
(
	idVoucher INT PRIMARY KEY IDENTITY,
	procentReducere DECIMAL(5,4),
	dataExpirare DATE
);


CREATE TABLE CarduriVouchere
(
	idClient INT FOREIGN KEY REFERENCES CarduriFidelitate(idClient),
	idVoucher INT FOREIGN KEY REFERENCES Vouchere(idVoucher)
	CONSTRAINT pk_CardVoucher PRIMARY KEY(idClient, idVoucher)
);

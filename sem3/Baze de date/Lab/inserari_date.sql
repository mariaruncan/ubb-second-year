INSERT INTO Angajati(nume, prenume, salariu)
VALUES ('Octavian', 'Lavinia', 1800), ('Pop', 'Ciprian', 2200), ('Steliana', 'Ana', 2000),
('Cap', 'Andrei', 1800), ('Pasca', 'Florin', 2500), ('Buta', 'Ioan', 1900),
('Muntean', 'Ionut', 2000), ('Atanasie', 'Tiberiu', 1800), ('Popescu', 'Paula', 2000),
('Fodor', 'Loredana', 1800);

SELECT * FROM Angajati;



INSERT INTO ConturiBancare (CNP, IBAN, sold)
VALUES (2, 620716894, 1234.55), (3, 282982452, 5632.78), (4, 686413401, 8999.23),
(5, 863511450, 782.10), (6, 104539288, 2365.10), (7, 578628549, 5666.87),
(8, 507791375, 56488.45), (9, 971112714, 12345.87), (10, 176553816, 6214.78),
(11, 862746896, 6547.99);

SELECT * FROM ConturiBancare;




INSERT INTO Adrese (judet, oras, strada, numar)
VALUES ('Suceava', 'Suceava', 'Universitatii', 9), ('Cluj', 'Cluj-Napoca', 'Calea Motilor', 4), ('Iasi', 'Iasi', 'George Enescu', 10),
('Brasov', 'Brasov', 'Stefan cel Mare', 6), ('Suceava', 'Vatra Dornei', 'Ciprian Porumbescu', 2), ('Constanta', 'Constanta', 'Traian', 12);

SELECT * FROM Adrese;




INSERT INTO Producatori (nume, telefon, mail)
VALUES ('Garnier', 0925364785, 'garnier@gmail.com'), ('Lancome', 0356472817, 'lancome@gmail.com'), ('Urban Decay', 0645412345, 'urban-decay@gmail.com'),
('Dior', 0245367283, 'dior@gmail.com'), ('Benefit', 0678354627, 'benefit@gmail.com'), ('NYX', 0781928374, 'nyx@gmail.com'),
('Armani', 0123467384, 'armani@gmail.com'), ('The Ordinary', 0914253627, 'ordinary@gmail.com');

SELECT * FROM Producatori



INSERT INTO Produse (nume, tip, volum, pret, idProducator)
VALUES ('Matte Lip Cream', 'ruj', 5, 44.90, 14), ('Moroccan Argan Oil', 'ingrijire corp', 30, 39.00, 16), 
('La Vie est Belle', 'parfum', 100, 342.30, 10), ('My Way', 'parfum', 75, 368.90, 15), 
('Gel hidratant', 'crema', 50, 25.40, 9), ('Naked Ultraviolet', 'fard', 50, 191.20, 11),
('Face Glow', 'fard', 50, 250.00, 12), ('Gimme Brow', 'fard', 7, 131.20, 13), 
('Teint Idole Ultra', 'fond de ten', 30, 156.30, 10), ('Luminous Silk', 'fond de ten', 18, 175.90, 15),
('Ultimate Shadow', 'fard', 50, 86.00, 14), ('Shine Loud', 'ruj', 7, 57, 14);

SELECT * FROM Produse;




INSERT INTO Vouchere (procentReducere, dataExpirare)
VALUES (0.15, '2021-12-31'), (0.20, '2022-5-15'), (0.05, '2021-10-29'),
(0.15, '2022-3-7'), (0.5, '2021-10-27'), (0.10, '2021-12-1'),
(0.10, '2022-1-1'), (0.30, '2022-6-6'), (0.25, '2021-11-15');

SELECT * FROM Vouchere;



INSERT INTO Clienti (nume, prenume, varsta)
VALUES ('Runcan', 'Maria', 20), ('Maciuc', 'Ana', 35), ('Ionescu', 'Marius', 43),
('Budeanu', 'Larisa', 33), ('Franciuc', 'Matei', 57), ('Mandrescu', 'Florin', 22),
('Macovei', 'Alexandra', 25);

SELECT * FROM Clienti;




INSERT INTO CarduriFidelitate (idClient, numarPuncte, dataExpirare)
VALUES (1, 56, '2023-12-31'), (2, 102, '2023-12-31'), (3, 64, '2023-12-31'),
(4, 32, '2023-12-31'), (5, 203, '2023-12-31'), (6, 91, '2023-12-31'), 
(7, 12, '2023-12-31');

SELECT * FROM CarduriFidelitate;




INSERT INTO CarduriVouchere (idClient, idVoucher)
VALUES (1, 1), (1, 2), (2, 8), (2, 7),
(2, 4), (3, 5), (4, 6), (4, 8),
(5, 3), (5, 4), (5, 8), (6, 6),
(6, 1);

SELECT * FROM CarduriVouchere;




INSERT INTO Comenzi (data, mod_plata, valoare, angajat, idAdresa, idClient)
VALUES ('2021-10-20', 'cash', 512.2, 8, 1, 5), ('2021-10-1', 'card', 394.3, 2, 2, 6),
('2021-10-20', 'cash', 322.4,  3, 3, 1), ('2021-10-23', 'card', 250,  4, 3, 1),
('2021-8-12', 'card', 654.5, 5, 4, 7), ('2021-9-3', 'card', 426.2, 6 , 4, 2),
('2021-9-9', 'cash', 527.2, 5, 1, 3), ('2021-7-5', 'cash', 558.9, 7, 5, 1),
('2021-10-1', 'card', 70.3, 8, 2, 4), ('2021-9-10', 'card', 143, 7, 6,1),
('2021-10-5', 'card', 300.5, 10, 5, 6), ('2021-10-13', 'cash', 425.9, 9, 6,7);

SELECT * FROM Comenzi;




INSERT INTO ComenziProduse (idComanda, idProdus)
VALUES (13, 3), (13, 11), (13, 2), (13, 1),
(2, 4), (2, 5), (3, 6), (3, 8),
(4, 7), (5, 12), (5, 7), (5, 9),
(5, 6), (6, 1), (6, 2), (6, 3),
(7, 6), (7, 7), (7, 11), (8, 5),
(8, 3), (8, 6), (9, 1), (9, 5),
(10, 11), (10, 12), (11, 1), (11, 2),
(11, 5), (11, 6), (12, 12), (12, 4);

SELECT * FROM ComenziProduse
-- 1. oferim o marire de salariu de 10% angajatilor care au procesat comenzi si au salariul mai
-- mic de 2000 si afisam soldul nou dupa 6 luni
SELECT A.CNP, A.nume, A.prenume, A.salariu, A.salariu*1.1 AS salariu_nou, C.sold, C.sold+6.6*A.salariu AS sold_6luni
FROM Angajati A INNER JOIN ConturiBancare C ON A.CNP=C.CNP
WHERE A.salariu < 2000 AND A.CNP IN (SELECT DISTINCT angajat FROM Comenzi);


-- 2. clientii care au vouchere ce expira pana pe 31/12/2021
SELECT C.nume, C.prenume, CF.numarPuncte, V.idVoucher, V.procentReducere, V.dataExpirare
FROM Clienti C INNER JOIN CarduriFidelitate CF on C.idClient=CF.idClient
INNER JOIN CarduriVouchere CV on CF.idClient=CV.idClient
INNER JOIN Vouchere V on V.idVoucher=CV.idVoucher
WHERE V.dataExpirare<='2021-12-31';


-- 3. categoriile de produse a caror medie de pret este cel putin 100
SELECT tip, AVG(pret) as pret_mediu
FROM Produse
GROUP BY tip
HAVING AVG(pret) >= 100;


-- 4. comenzile si valoarea acestora
SELECT C.idComanda, SUM(P.pret) as total_plata
FROM Comenzi C INNER JOIN ComenziProduse CP on C.idComanda=CP.idComanda
INNER JOIN Produse P on CP.idProdus=P.idProdus
GROUP BY C.idComanda


-- 5. clientii care au facut cel putin 2 comenzi
SELECT CL.idClient, COUNT(*) as numar_comenzi
FROM Clienti CL INNER JOIN Comenzi C on CL.idClient=C.idClient
GROUP BY CL.idClient
HAVING COUNT(*)>=2;


-- 6. voucherele existente, valabile dupa 1/1/2022
SELECT DISTINCT procentReducere, dataExpirare
FROM Vouchere
WHERE dataExpirare>='2022/1/1';


-- 7. interactiunile angajati-clienti
SELECT A.CNP, A.nume as numeA, A.prenume as prenumeA, CL.idClient, CL.nume as numeC, CL.prenume as prenumeC 
FROM Angajati A, Comenzi C, Clienti CL
WHERE A.CNP=C.angajat AND C.idClient=CL.idClient



-- 8. adresele la care comanda clientii
SELECT DISTINCT CL.idClient, CL.nume, CL.prenume, A.*
FROM Clienti CL full outer join Comenzi C on CL.idClient=C.idClient
full outer join Adrese A on C.idAdresa=A.idAdresa


-- 9. producatorii care au produse mai ieftine de 150
SELECT DISTINCT PR.*
FROM Producatori PR INNER JOIN Produse P on PR.idProducator=P.idProducator
WHERE P.pret<=150

-- 10. comenzi si valoarea acestora dupa folosirea punctelor bonus(1p = -1)
SELECT C.idComanda, C.valoare, C.valoare-CF.numarPuncte as valoare_noua, CL.idClient, CF.numarPuncte
FROM Comenzi C INNER JOIN Clienti CL on C.idClient=Cl.idClient
INNER JOIN CarduriFidelitate CF on CL.idClient=CF.idClient


-- 11. angajat - numarul de produse vandute
SELECT A.CNP, COUNT(*) as nr_produse_vandute
FROM Angajati A INNER JOIN Comenzi C on A.CNP=C.angajat
INNER JOIN ComenziProduse CP on C.idComanda=CP.idComanda
GROUP BY A.CNP, CP.idComanda


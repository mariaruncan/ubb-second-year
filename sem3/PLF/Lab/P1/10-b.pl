% cmmdc(x:integer, y:integer, Rez:integer)
% (i,i,o)-determinist
% cmmdc(x, y) = { x, daca y = 0
%               { cmmdc(y, x%y), altfel
%
% functie(L:list, Rez:integer)
% (i,o)-determinist
% functie(l1,...,ln) = {l1, daca n = 1
%		       {cmmdc(l1, functie(l2,...,ln), altfel


cmmdc(X,0,X) :- !.
cmmdc(X,Y,Rez) :-
	X1 is Y,
	Y1 is mod(X,Y),
	cmmdc(X1,Y1,Rez).



functie([Head],Head) :- !.
functie([Head|Tail],Rez) :-
	functie(Tail, Rez1),
	cmmdc(Head,Rez1,Rez).

% inlocuire(V, N, l1...ln) =
% {[], daca n = 0
% { N(+)inlocuire(V, N, l2...ln), daca l1 = V si n > 0
% { l1(+)inlocuire(V, N, l2...ln), altfel
% inlocuire(V, N, L, R)
% (i,i,i,o)-determinist, (o,i,i,i), (i,i,o,i), (i,i,i,i)
% V - intreg; elementul ale carui aparitii trebuie inlocuite cu N in
%   lista L
% N - intreg; elementul cu care vor fi inlocuite aparitiile lui V
%     in lista L
% L - lista; lista in care trebuie inlocuite aparitiile lui V cu N
% R - lista; lista rezultata dupa inlocuirea aparitiilor lui V cu N
inlocuire(_, _, [], []) :- !.
inlocuire(V, N, [V|T], [N|R]):-
	!,
	inlocuire(V, N, T, R).
inlocuire(V, N, [H|T], [H|R]) :-
	inlocuire(V, N, T, R).


% maximLista(l1...ln) =
% { l1, daca (l1 > maximLista(l2...ln) si n > 2) sau n = 1
% { maximLista(l2...ln), altfel
% maximLista(L, R)
% (i,o)-detrminist, (i,i)
% L - lista; lista a carei maxim se cauta
% R - intreg; elementul maxim al listei L
maximLista([X], X) :- !.
maximLista([H|T], Max) :-
	maximLista(T, TMax),
	H > TMax,
	!,
	Max is H.

maximLista([H|T], Max) :-
	maximLista(T, TMax),
	H =< TMax,
	Max is TMax.


% extractNumere(l1...ln) =
% {[], daca n = 0
% {l1(+)extractNumere(l2...ln), daca l1 este numar
% {extractNumere(l2...ln), altfel
% extractNumere(L, R)
% (i,o)-determinist, (i,i)
% L - lista; lista din care trebuie sa extragem toate numerele
% R - lista; lista ce contine toate numerele din L
extractNumere([], []) :- !.
extractNumere([H|T], [H|R]) :-
	number(H),
	!,
	extractNumere(T, R).
extractNumere([_|T], R) :-
	extractNumere(T, R).


% runAux(l1...ln, e) =
% {[], daca n = 0
% {inlocuire(e,maximLista(l1), l1) (+) runAux(l2...ln, e),
%           daca l1 este lista si n > 0
% {l1(+)runAux(l2...ln, e), altfel
% runAux(L, E, R)
% ()model
% L - lista; lista in a carei subliste trebuie sa inlocuim
%     toate aparitiile elementului E cu elementul maxim al
%     sublistei
% E - intreg; elementul ale carui aparitii in sublistele listei
%     L trebuie inlocuite cu maximul sublistei respective
% R - lista; lista rezultata dupa inlocuirea aparitiilor elementului
%     E in sublistele listei L cu maximul sublistei respective
runAux([], _, []):-!.
runAux([H|T], E, [R1|R]) :-
	is_list(H),
	!,
	maximLista(H, Max),
	inlocuire(E, Max, H, R1),
	runAux(T, E, R).
runAux([H|T], E, [H|R]) :-
	runAux(T, E, R).



% run(l1...ln) = runAux(l1...ln, maximLista(extractNumere(l1...ln)))
% run(L, R)
% ()model
% L - lista; lista eterogena in care toate aparitiile elementului
%     maxim (dintre valorile intregi) trebuie inlocuite in fiecare
%     sublista cu maximul sublistei respective
% R - lista; lista rezultata in urma inlocuirii tuturor aparitiilor
%     elementului maxim(dintre valorile intregi) ale listei L cu
%     maximul sublistei respective
run(L, R) :-
	extractNumere(L, Numere),
	maximLista(Numere, Max),
	runAux(L, Max, R).




% candidat(L:lista, X:intreg)
% (i, o) - nedet; (i, i) - det
candidat([H|_], H) .
candidat([_|T], K) :-
	candidat(T, K).

% vale(L:lista, F:intreg, S:lista, Rez:list)
% (i, i, i, o) - nedet
vale(_, -1, S, S).
vale(L, 1, [S1|TS], Rez) :-
	candidat(L, C),
	C < S1,
	vale(L, 1, [C,S1|TS], Rez).
vale(L, _, [S1|TS], Rez) :-
	candidat(L, C), %(i,o) nedet
	C > S1,
	\+candidat([S1|TS], C), %(i,i) det
	vale(L, -1, [C,S1|TS], Rez).

% principalVale(L:lista, Rez:lista)
% (i, o) - nedet
principalVale(L, Rez) :-
	candidat(L, C1),
	candidat(L, C2),
	C1 < C2,
	vale(L, 1, [C1, C2], Rez).

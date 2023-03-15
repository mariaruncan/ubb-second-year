candidat([H|_], H).
candidat([_|T], Rez):-
	candidat(T, Rez).

par1([],[],[]):-!.
par1([H1|T1], [H2|T2], [Rez1|Rez2]):-
	candidat([H1|T1], X),
	candidat([H2|T2], Y),
	Rez1 is X*Y,
	par1(T1, T2, Rez2).


main(L1,L2, Rez):-
	findall(X, par1(L1,L2,X),Rez).

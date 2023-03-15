descreste([_]):-!.
descreste([H1, H2|T]) :-
	H1 > H2,
	!,
	descreste([H2|T]).


munte([H1,H2|T]):-
	H1 > H2,
	!,
	descreste([H2|T]).
munte([H1,H2|T]) :-
	H1 < H2,
	munte([H2|T]).


muntePrincipal([H1,H2,H3|T]) :-
	H1 < H2,
	munte([H2,H3|T]).

nrListe([],0):-!.

nrListe([H|T], R):-
	is_list(H),
	muntePrincipal(H),
	!,
	nrListe(T, R1),
	R is 1+R1.

nrListe([_|T], R):-
	nrListe(T, R).

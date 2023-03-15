%insert(n:pozitie,e:element,L:list,Rez:list)
%(i,i,i,o)-determinist
%insert(n, e, l1,...,lk) = {(e), l este vida
%                          {(e, l1,...,lk), n = 0
%                          { l1 + insert(n-1, e, l2,...,lk), altfel

insert(N,E,[],[E]) :- !.
insert(N,E,[Head|Tail],[E,Head|Tail]) :- N=:=0, !.
insert(N,E,[Head|Tail1],[Head|Tail2]) :-
	N1 is N-1,
	insert(N1,E,Tail1,Tail2).


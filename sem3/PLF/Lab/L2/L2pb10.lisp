; 10. Se da un arbore de tipul (2). Sa se precizeze nivelul pe care apare un nod
;   A
;  / \
; B   C
;    / \
;   D   E
; (nod (lista-subarbore-1) (lista-subarbore-2)) (2)
; (A (B) (C (D) (E))) (2)

; nivelAux(l1...ln, e, nivel) = { -1, n = 0
;								{ nivel, l1 = e
;								{ max(nivelAux(l2, e, nivel+1), nivelAux(l3, e, nivel+1)), altfel
; l - arborele in care se face cautarea
; e - elementul cautat
; nivel - nivelul curent pe care incercam sa cautam
; (nivel '(A (B) (C (D) (E () (F)))) 'F) => 3
; (nivel '(A (B) (C (D) (E))) 'E) => 2
(defun nivelAux(l e nivel)
	(cond
		((null l) -1)
		((eq (car l) e) nivel)
		(t (max (nivelAux (cadr l) e (+ nivel 1)) (nivelAux (caddr l) e (+ nivel 1))))
	)
)

; nivel(l, e) = nivelAux(l, e, 0)
(defun nivel(l e)
	(nivelAux l e 0)
)



; 2. Definiti o functie care obtine dintr-o lista data lista tuturor atomilor
; care apar, pe orice nivel, dar in aceeasi ordine.
; De exemplu: (((A B) C) (D E)) --> (A B C D E)
; extrageAtomi(x) = { (). daca x este lista vida
;					{ (x), daca x este atom
;					{ U(i=1,n) extrageAtomi(xi) , daca x este lista x1 ... xn
; x - lista neliniara din care trebuie sa extragem atomii in aceeasi ordine
(defun extrageAtomi(x)
		(cond
		((null x) nil)		
			((atom x) (list x))
			(t (MAPCAN #'extrageAtomi x))
		)
)
; (extrageAtomi '(((A B) C) (D E)))
; (extrageAtomi '(1 (2 3 (a)) 4 ((5))))
; (extrageAtomi '(1 (2 3 (a) b) 4 ((5))))

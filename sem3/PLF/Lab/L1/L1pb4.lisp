; a) Definiti o functie care intoarce suma a doi vectori.
; len(v1...vn) = { 0, n = 0
; 				 { 1 + len(v2...vn)
; v - vectorul a carei lungime dorim sa o aflam (la nivel superficial)
; (len '(1 2 3 4 5)) => 5
; (len '(a b (c d) e)) => 4
(defun len (v)
	(cond
		((null v) 0)
		(t (+ 1 (len(cdr v))))
	)
)

; sumaVectori(a1...an, b1...bm) = { (), n = 0 sau m = 0 sau n != m
; 								  { (a1 + b1) (+) sumaVectori(a2...an, b2...bm)
; a, b - vectorii a caror suma dorim sa o aflam
; (sumaVectori '(1 2 3) '(1 2 3)) => (2 4 6)
; (sumaVectori '(1 2 3) '(1 2 3 4)) => NIL
(defun sumaVectori(a b)
	(cond
		((or (null a) (null b) (not (equal (len a) (len b)))) nil)
		(t (cons (+ (car a) (car b)) (sumaVectori (cdr a) (cdr b))))
	)
)


; b) Definiti o functie care obtine dintr-o lista data lista tuturor atomilor care apar, pe orice nivel, dar in aceeasi ordine. De exemplu:
; (((A B) C) (D E)) --> (A B C D E)
; selecteazaAtomi(l1...ln) = { (), n = 0
;							 { l1 (+) selecteazaAtomi(l2...ln), l1 este atom
;							 { selecteazaAtomi(l1) (+) selecteazaAtomi(l2...ln), altfel (l1 este lista)
; l - lista din care vrem sa obtinem lista atomilor de pe orice nivel
; (selecteazaAtomi '(((A B) C) (D E)))
(defun selecteazaAtomi(l)
	(cond
		((null l) nil)
		((atom (car l)) (cons (car l) (selecteazaAtomi (cdr l))))
		(t (append (selecteazaAtomi (car l)) (selecteazaAtomi (cdr l))))
	)
)


; c) Sa se scrie o functie care plecand de la o lista data ca argument, inverseaza numai secventele continue de atomi. Exemplu:
; (a b c (d (e f) g h i)) ==> (c b a (d (f e) i h g))
; secventeContinueRec(l1...ln, col) = { col, n = 0
;									  { col (+) secventeContinueRec(l1, []) (+) secventeContinueRec(l2...ln, []), l1 este lista
;									  { secventeContinueRec(l2...ln, l1(+)col), altfel
; l - lista in care vrem sa inversam secventele continue
; col - var colectoare
; (secventeContinue '(a b c (d (e f) g h i))) 
(defun secventeContinueRec (l col)
	(cond
		((null l) col)
		((listp (car l)) (append col (cons (secventeContinueRec (car l) ()) (secventeContinueRec (cdr l) ()))))
		(t (secventeContinueRec (cdr l) (cons (car l) col)))
	)
)

; secventeContinue(l1...ln) = secventeContinueRec(l1...ln, []) 
; l - lista in care vrem sa inversam secventele continue
(defun secventeContinue (l)
	(secventeContinueRec l ())
)


; d) Sa se construiasca o functie care intoarce maximul atomilor numerici dintr-o lista, de la nivelul superficial.
; maxim(a, b) = { a, a > b
;			    { b, altfel
; a, b - doi intregi
; (maximLista '(-2 (a 9) 0 1 (10) 4 5)) => 5
(defun maxim (a b)
	(cond
		((equal a nil) b)
		((equal b nil) a)
		((> a b) a)
		(t b)
	)
)

; maximLista(l1...ln) = { nil, n = 0 
;						{ maxim(l1, maximLista(l2...ln)), l1 este numar
;						{ maximLista(l2...ln), altfel
; l - lista a carei maxim numeric dorim sa il aflam
(defun maximLista (l)
	(cond
		((null l) nil)
		((numberp (car l)) (maxim (car l) (maximLista (cdr l))))
		(t (maximLista (cdr l)))
	)
)
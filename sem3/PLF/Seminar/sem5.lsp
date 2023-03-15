(defun interclasare(a b)
	(cond
		((null a) b)
		((null b) a)
		((< (car a) (car b)) (cons (car a) (interclasare (cdr a) b)))
		((> (car a) (car b)) (cons (car b) (interclasare a (cdr b))))
		(t (cons (car b) (interclasare (cdr a) (cdr b))))
	)
)




(defun sterge_elem(e l)
	(cond
		((null l) nil)
		((and (atom (car l)) (not (equal (car l) e)))
			(cons (car l) (sterge_elem e (cdr l))))
		((atom (car l)) (sterge_elem e (cdr l)))
		(t (cons (sterge_elem e (car l)) (sterge_elem e (cdr l))))
	)
)






(defun genereaza_lista(l m i col)
	(cond
		((null l) col)
		((and (numberp (car l)) (< (car l) m)) (genereaza_lista (cdr l) (car l) (+ i 1) (list i)))
		((equal (car l) m) (genereaza_lista (cdr l) m (+ i 1) (cons i col)))
		(t (genereaza_lista (cdr l) m (+ i 1) col))
	)
)

(defun wrapper_list(l i)
	(cond 
		((null l) nil)
		((not (numberp (car l))) (wrapper_list (cdr l) (+ i 1)))
		((numberp (car l)) (genereaza_lista (cdr l) (car l) (+ i 1) (list i)))
	)
)


(defun wrapper_list_init(l)
	(wrapper_list l 1)
)
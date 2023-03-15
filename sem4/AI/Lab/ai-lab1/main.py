import cmath


def print_menu():
    print("""1. Să se determine ultimul (din punct de vedere alfabetic) cuvânt care poate apărea într-un text care 
conține mai multe cuvinte separate prin ” ” (spațiu).
2. Să se determine distanța Euclideană între două locații identificate prin perechi de numere.
3. Să se determine produsul scalar a doi vectori rari care conțin numere reale. Un vector este rar atunci când 
conține multe elemente nule. Vectorii pot avea oricâte dimensiuni.
4. Să se determine cuvintele unui text care apar exact o singură dată în acel text.
7. Să se determine al k-lea cel mai mare element al unui șir de numere cu n elemente (k < n).
9. Considerându-se o matrice cu n x m elemente întregi și o listă cu perechi formate din coordonatele a 2 căsuțe 
din matrice ((p,q) și (r,s)), să se calculeze suma elementelor din sub-matricile identificate de fieare pereche.
10. Considerându-se o matrice cu n x m elemente binare (0 sau 1) sortate crescător pe linii, să se identifice 
indexul liniei care conține cele mai multe elemente de 1.
0. Exit\n""")


def citeste_matrice():
    n = int(input("Nr linii: "))
    m = int(input("Nr coloane: "))
    mat = []
    for i in range(n):
        linie = []
        for j in range(m):
            linie.append(int(input("mat[{}][{}]".format(i, j))))
        mat.append(linie)
    return mat


def run():
    print_menu()
    while True:
        cmd = int(input(">>>"))
        if cmd == 0:
            print("Byeee :)")
            break
        elif cmd == 1:
            text = input("Introdu textul: ")
            print(cerinta1(text))
        elif cmd == 2:
            a = []
            b = []
            a.append(int(input("xa: ")))
            a.append(int(input("ya: ")))
            b.append(int(input("xb: ")))
            b.append(int(input("yb: ")))
            print("Distanta este: ", cerinta2(a, b))
        elif cmd == 3:
            n = int(input("Dimensiune vectori: "))
            a = []
            b = []
            for i in range(n):
                a.append(int(input("a[{}] = ".format(i))))
            for i in range(n):
                b.append(int(input("b[{}] = ".format(i))))
            print("Produsul scalar este: ", cerinta3(n, a, b))
        elif cmd == 4:
            text = input("Introdu textul: ")
            print("Cuvintele care apar o data sunt: ", cerinta4(text))
        elif cmd == 7:
            # n, v, k
            n = int(input("n = "))
            v = []
            for i in range(n):
                v.append(int(input("v[{}]: ".format(i))))
            k = int(input("k = "))
            print("Al {}-lea cel mai mare elem este {}".format(k, cerinta7(n, v, k)))
        elif cmd == 9:
            mat = citeste_matrice()
            p = int(input("p = "))
            q = int(input("q = "))
            r = int(input("r = "))
            s = int(input("s = "))
            print("Suma este: ", cerinta9(mat, (p, q), (r, s)))
        elif cmd == 10:
            mat = citeste_matrice()
            print("Linia cu cele mai multe valori de 1 este ", cerinta10(mat))
        else:
            print("Comanda invalida!")
        print("\n")


def cerinta1(text):
    cuvinte = text.split()
    ultim = ""
    for cuv in cuvinte:
        if cuv > ultim:
            ultim = cuv
    return ultim


def cerinta2(a, b):
    return cmath.sqrt(pow(a[0] - b[0], 2) + pow(a[1] - b[1], 2)).real


def cerinta3(n, a, b):
    produs_scalar = 0
    for i in range(n):
        produs_scalar += a[i] * b[i]
    return produs_scalar


def cerinta4(text):
    cuvinte = text.split()
    dictionar = {}
    for cuv in cuvinte:
        if cuv in dictionar:
            dictionar[cuv] += 1
        else:
            dictionar[cuv] = 1
    rezultat = []
    for key in dictionar:
        if dictionar[key] == 1:
            rezultat.append(key)
    return rezultat


def cerinta7(n, v, k):
    v.sort()
    return v[n - k]


def cerinta9(mat, a, b):
    suma = 0
    for i in range(a[0], b[0] + 1):
        for j in range(a[1], b[1] + 1):
            suma += mat[i][j]
    return suma


def cerinta10(mat):
    nr1 = -1
    k = 0
    index_linie = 0
    for row in mat:
        suma = row.count(1)
        if suma > nr1:
            nr1 = suma
            index_linie = k
        k += 1
    return index_linie + 1


if __name__ == "__main__":
    run()

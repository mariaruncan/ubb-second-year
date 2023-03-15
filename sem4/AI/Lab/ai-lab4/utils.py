from cmath import sqrt
from random import randint


def pathLength(lista, mat):
    length = 0
    prev = 0
    for node in lista:
        if mat[prev][node] == 0 and prev != node:
            return 999999
        length += mat[prev][node]
        prev = node
    return length


def generateARandomPermutation(n):
    perm = [i for i in range(1, n)]
    pos1 = randint(0, n - 2)
    pos2 = randint(0, n - 2)
    perm[pos1], perm[pos2] = perm[pos2], perm[pos1]
    perm.insert(0, 0)
    perm.append(0)
    return perm


def readData(filename):
    f = open(filename, "r")
    net = {}
    n = int(f.readline())
    net['noNodes'] = n
    mat = []
    for i in range(n):
        mat.append([])
        line = f.readline()
        elems = line.split(",")
        for j in range(n):
            mat[-1].append(int(elems[j]))
    net['mat'] = mat
    source = int(f.readline())
    destination = int(f.readline())
    net['source'] = source
    net['destination'] = destination
    return net


def buildPath(lista, source, destination, mat):
    posS = -1
    posD = -1
    path = {}

    for i in range(len(lista)):
        if lista[i] == source:
            posS = i
        if lista[i] == destination:
            posD = i

    prev = posS
    cur = (posS + 1) % len(lista)
    costRight = mat[lista[prev]][lista[cur]]
    pathRight = []
    pathRight.append(lista[prev])
    pathRight.append(lista[cur])
    while lista[cur] != destination:
        prev = cur
        cur += 1
        cur %= len(lista)
        costRight += mat[lista[prev]][lista[cur]]
        pathRight.append(lista[cur])

    prev = posS
    cur = (posS - 1) % len(lista)
    costLeft = mat[lista[prev]][lista[cur]]
    pathLeft = []
    pathLeft.append(lista[prev])
    pathLeft.append(lista[cur])
    while lista[cur] != destination:
        prev = cur
        cur -= 1
        cur %= len(lista)
        costLeft += mat[lista[prev]][lista[cur]]
        pathLeft.append(lista[cur])

    if costLeft < costRight:
        path['cost'] = costLeft
        path['path'] = pathLeft
    else:
        path['cost'] = costRight
        path['path'] = pathRight
    if 0 in path['path']:
        path['path'].remove(0)
    return path


def parser(filename):
    f = open(filename, "r")
    net = {}
    coords = []
    _ = f.readline()
    _ = f.readline()
    _ = f.readline()
    x = f.readline()

    if filename == "Data/hardE.txt":
        _, _, dim = x.split()
    else:
        _, dim = x.split()
    net['noNodes'] = int(dim)

    x = f.readline()
    x = f.readline()
    x = f.readline()

    while x != "EOF\n":
        _, coordx, coordy = x.split()
        coords.append([float(coordx), float(coordy)])
        x = f.readline()

    mat = []
    for i in range(net['noNodes']):
        mat.append([])
        for j in range(net['noNodes']):
            if i == j:
                mat[i].append(0)
                continue
            x1 = coords[i][0]
            x2 = coords[j][0]
            y1 = coords[i][1]
            y2 = coords[j][1]
            mat[i].append(int(sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)).real))

    net['noNodes'] = net['noNodes'] - 1
    net['mat'] = mat
    f.close()
    return net
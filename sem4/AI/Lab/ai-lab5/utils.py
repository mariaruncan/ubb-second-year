from cmath import sqrt

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


def parser(filename):
    f = open(filename, "r")
    net = {}
    coords = []
    _ = f.readline()
    _ = f.readline()
    _ = f.readline()
    x = f.readline()

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

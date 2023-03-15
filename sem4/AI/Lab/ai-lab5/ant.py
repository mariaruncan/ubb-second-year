from random import randint, uniform

INF = 999999999


class Ant:

    def __init__(self, params):
        self.__params = params
        self.__path = [randint(0, self.__params["noNodes"] - 1)]
        self.__visited = [False] * params["noNodes"]
        self.__visited[self.__path[0]] = True
        self.__exists = True

    def distance(self):
        d = 0
        lung = len(self.__path)
        if not self.__exists:
            return INF
        for i in range(lung - 1):
            d = d + self.__params["mat"][self.__path[i]][self.__path[i + 1]]
        d = d + self.__params["mat"][self.__path[lung - 1]][self.__path[0]]
        return d

    def nextNode(self):
        q = uniform(0, 1)
        currNode = self.__path[-1]
        phero = self.__params["pheromone"]
        alpha = self.__params["alpha"]
        beta = self.__params["beta"]
        mat = self.__params["mat"]
        js = []

        if q < self.__params["q0"]:
            suma = 0
            possNodes = []
            for i in range(self.__params["noNodes"]):
                if not self.__visited[i] and i != currNode and self.__params["mat"][currNode][i]:
                    j = (phero[currNode][i] ** alpha) * ((1 / mat[currNode][i]) ** beta)
                    js.append(j)
                    suma += j

            for i in range(self.__params["noNodes"]):
                possNodes.append((js[i] / suma, i))

            possNodes.sort()

            prob = uniform(0, 1)
            for i in range(len(possNodes) - 1):
                if possNodes[i][0] <= prob < possNodes[i + 1][0]:
                    return possNodes[i][1]

            if len(possNodes) == 0:
                raise Exception("")
            return possNodes[-1][1]

        else:
            possNodes = []
            for i in range(len(phero[currNode])):
                if i != currNode and not self.__visited[i] and mat[currNode][i]:
                    j = (phero[currNode][i] ** alpha) * ((1 / mat[currNode][i]) ** beta)
                    possNodes.append((j, i))

            possNodes.sort()

            if len(possNodes) == 0:
                raise Exception("")
            return possNodes[0][1]

    def explore(self):
        try:
            nextNode = self.nextNode()
        except:
            self.__exists = False
            return

        currNode = self.__path[-1]
        self.__visited[nextNode] = True
        self.__path.append(nextNode)

        self.__params["pheromone"][currNode][nextNode] = (1 - self.__params['evaporationRate']) * \
                                                         self.__params['pheromone'][currNode][nextNode] + \
                                                         self.__params['evaporationRate'] * \
                                                         self.__params['mat'][currNode][nextNode]
        self.__params["pheromone"][nextNode][currNode] = self.__params["pheromone"][currNode][nextNode]

    def path(self):
        return self.__path

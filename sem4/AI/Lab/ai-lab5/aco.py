from random import randint

from ant import Ant

class AntColonyOptimization:
    def __init__(self, params):
        self.__params = params
        self.__ants = []

    def initialise(self):
        self.__ants = []
        for _ in range(self.__params["noAnts"]):
            self.__ants.append(Ant(self.__params))

    def initialisePhero(self):
        for i in range(self.__params['noNodes']):
            ph = [1] * self.__params['noNodes']
            self.__params['pheromone'].append(ph)

    def bestAnt(self):
        best = self.__ants[0]
        for ant in self.__ants:
            if ant.distance() < best.distance():
                best = ant
        return best

    def explore(self):
        for _ in range(self.__params['noNodes'] - 1):
            for ant in self.__ants:
                ant.explore()
        self.updatePhero()

    def deleteEdge(self):
        i = randint(0, self.__params['noNodes']-1)
        j = randint(0, self.__params['noNodes']-1)
        while i == j or self.__params["mat"][i][j] == 0:
            j = randint(0, self.__params["noNodes"] - 1)
        print("Removed edge: (" + str(i) + ", " + str(j) + ")")
        self.__params["pheromone"][i][j] = 0
        self.__params["pheromone"][j][i] = 0
        self.__params["mat"][i][j] = 0
        self.__params["mat"][j][i] = 0

    def updatePhero(self):
        ant = self.bestAnt()
        bestPath = ant.path()
        for i in range(len(bestPath) - 1):
            u = bestPath[i]
            v = bestPath[i + 1]
            self.__params["pheromone"][u][v] = self.__params["evaporationRate"] * self.__params["pheromone"][u][v] +\
                                               self.__params["evaporationRate"] * (1/ant.distance())
            self.__params["pheromone"][v][u] = self.__params["pheromone"][u][v]

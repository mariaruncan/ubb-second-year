from random import randint, random
from utils import generateNewValue


class Chromosome:
    def __init__(self, problParam=None):
        self.__problParam = problParam
        self.__genes = [generateNewValue(problParam['min'], problParam['max']) for _ in range(problParam['noDim'])]
        self.__fitness = 0.0

    @property
    def genes(self):
        return self.__genes

    @property
    def fitness(self):
        return self.__fitness

    @genes.setter
    def genes(self, l=[]):
        self.__genes = l

    @fitness.setter
    def fitness(self, fit=0.0):
        self.__fitness = fit

    def crossover(self, c):
        newGenes = []
        for i in range(self.__problParam['noDim']):
            r = random()
            if r < 0.5:
                newGenes.append(self.__genes[i])
            else:
                newGenes.append(c.__genes[i])
        newChromo = Chromosome(c.__problParam)
        newChromo.genes = newGenes
        return newChromo

    def mutation(self):
        pos1 = randint(0, len(self.__genes) - 1)
        pos2 = randint(0, len(self.__genes) - 1)
        temp = self.__genes[pos1]
        self.__genes[pos1] = self.__genes[pos2]
        self.__genes[pos2] = temp

    def __str__(self):
        return '\nChromo: ' + str(self.__genes) + ' has fit: ' + str(self.__fitness)

    def __repr__(self):
        return self.__str__()

    def __eq__(self, c):
        return self.__genes == c.__genes and self.__fitness == c.__fitness

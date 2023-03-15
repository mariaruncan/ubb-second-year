from random import randint
from utils import generateARandomPermutation


class Chromosome:
    def __init__(self, mat=None, noNodes=None, function=None):
        self.__mat = mat
        self.__noNodes = noNodes
        self.__function = function
        self.__genes = generateARandomPermutation(noNodes)
        self.__fitness = 0.0

    @property
    def genes(self):
        return self.__genes

    @property
    def mat(self):
        return self.__mat

    @property
    def fitness(self):
        return self.__fitness

    @genes.setter
    def genes(self, new=[]):
        self.__genes = new

    @fitness.setter
    def fitness(self, fit=0.0):
        self.__fitness = fit

    def crossover(self, c):
        r1 = randint(1, self.__noNodes - 2)
        r2 = randint(1, self.__noNodes - 2)
        if r1 > r2:
            r2, r1 = r1, r2
        newGenes = self.__genes[r1:r2]
        newGenes.insert(0, 0)
        for gene in c.__genes:
            if gene not in newGenes:
                newGenes.append(gene)
        newGenes.append(0)
        newChromo = Chromosome(self.__mat, self.__noNodes, self.__function)
        newChromo.genes = newGenes
        return newChromo

    def mutation(self):
        pos1 = randint(1, len(self.__genes) - 2)
        pos2 = randint(1, len(self.__genes) - 2)
        temp = self.__genes[pos1]
        del self.__genes[pos1]
        self.__genes.insert(pos2, temp)

    def computeFitness(self):
        self.__fitness = self.__function(self.__genes, self.__mat)

    def __str__(self):
        return '\nChromo: ' + str(self.__genes) + ' has fit: ' + str(self.__fitness)

    def __repr__(self):
        return self.__str__()

    def __eq__(self, c):
        return self.__genes == c.__genes and self.__fitness == c.fitness

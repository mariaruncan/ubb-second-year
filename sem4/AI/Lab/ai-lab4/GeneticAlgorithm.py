from random import randint
from RealChromosome import Chromosome


class GeneticAlgorithm:
    def __init__(self, param=None):
        self.__param = param
        self.__population = []

    @property
    def population(self):
        return self.__population

    def initialise(self):
        for _ in range(0, self.__param['popSize']):
            c = Chromosome(self.__param['mat'], self.__param['noNodes'], self.__param['function'])
            self.__population.append(c)
        self.evaluation()

    def evaluation(self):
        for c in self.__population:
            c.computeFitness()

    def bestChromosome(self):
        best = self.__population[0]
        for c in self.__population:
            if c.fitness < best.fitness:
                best = c
        return best

    def worstChromosome(self):
        worst = self.__population[0]
        for c in self.__population:
            if c.fitness > worst.fitness:
                worst = c
        return worst

    def selection(self):
        list = []
        for _ in range(self.__param['turnirDim']):
            pos = randint(0, self.__param['popSize'] - 1)
            list.append((pos, self.__population[pos]))
        list = sorted(list, key=lambda x: x[1].fitness)
        return list[0][0]


    def oneGeneration(self):
        newPop = []
        for _ in range(self.__param['popSize']):
            p1 = self.__population[self.selection()]
            p2 = self.__population[self.selection()]
            off = p1.crossover(p2)
            off.mutation()
            newPop.append(off)
        self.__population = newPop
        self.evaluation()

    def oneGenerationElitism(self):
        newPop = [self.bestChromosome()]
        for _ in range(self.__param['popSize'] - 1):
            p1 = self.__population[self.selection()]
            p2 = self.__population[self.selection()]
            off = p1.crossover(p2)
            off.mutation()
            newPop.append(off)
        self.__population = newPop
        self.evaluation()

    def oneGenerationSteadyState(self):
        for _ in range(self.__param['popSize']):
            p1 = self.__population[self.selection()]
            p2 = self.__population[self.selection()]
            off = p1.crossover(p2)
            off.mutation()
            off.fitness = self.__param['function'](off.genes, off.mat)
            worst = self.worstChromosome()
            if off.fitness > worst.fitness:
                for i in range(self.__param['popSize']):
                    if self.__population[i] == worst:
                        self.__population[i] = off
                        break
        self.evaluation()

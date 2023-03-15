from collections import Counter
from GeneticAlgorithm import GeneticAlgorithm
from utils import readGMLFile, plotNetwork
import networkx as nx


def modularity(communities):
    noNodes = param['noNodes']
    mat = param['mat']
    degrees = param['degrees']
    noEdges = param['noEdges']
    M = 2 * noEdges
    Q = 0.0
    for i in range(0, noNodes):
        for j in range(0, noNodes):
            if communities[i] == communities[j]:
                Q += (mat.item((i, j)) - degrees[i] * degrees[j] / M)
    return Q * 1 / M


def run(network):
    MIN = 1
    MAX = network['noNodes']
    gaParams = {'popSize': 200, 'noGen': 300}
    problParam = {'min': MIN, 'max': MAX, 'function': modularity, 'noDim': MAX}

    ga = GeneticAlgorithm(gaParams, problParam)
    ga.initialise()
    ga.evaluation()

    allTimesBestChromo = ga.bestChromosome()

    for i in range(gaParams['noGen']):
        ga.oneGenerationSteadyState()
        liveBestChromo = ga.bestChromosome()
        if allTimesBestChromo.fitness < liveBestChromo.fitness:
            allTimesBestChromo = liveBestChromo
        print("Generatia:" + str(i) + " nrComunitati = " + str(len(Counter(allTimesBestChromo.genes).keys())) + str(
            allTimesBestChromo))

    return allTimesBestChromo.genes


# param = readGMLFile('data/dolphins.gml')
# param = readGMLFile('data/football.gml')
# param = readGMLFile('data/karate.gml')
# param = readGMLFile('data/krebs.gml')
# param = readGMLFile('data/ex1.gml')
# param = readGMLFile('data/ex2.gml')
param = readGMLFile('data/ex3.gml')
# param = readGMLFile('data/ex4.gml')
communities = run(param)
plotNetwork(param, communities)


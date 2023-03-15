from cmath import sqrt

from GeneticAlgorithm import GeneticAlgorithm
from RealChromosome import Chromosome
from utils import readData, pathLength, buildPath, parser

# net = readData("tsp/easy_01_tsp.txt")
# net = readData("data/easy1.txt")
# net = readData("data/easy2.txt")
# net = readData("data/medium1.txt")
# net = readData("data/medium2.txt")
# net = readData("data/hard1.txt")
net = parser("data/berlin52.tsp")
MIN = 0
MAX = net['noNodes']

params = {'popSize': 200, 'noGen': 5000, 'min': MIN, 'MAX': MAX, 'function': pathLength,
          'noNodes': MAX, 'turnirDim': 20, 'mat': net['mat']}

alg = GeneticAlgorithm(params)
alg.initialise()

bestChromo = Chromosome(params['mat'], params['noNodes'], params['function'])
bestChromo.fitness = 99999999
gens = []
bests = []

for generation in range(params['noGen']):
    if generation > 0:
        alg.oneGenerationElitism()
    ch = alg.bestChromosome()
    ch.fitness = alg.bestChromosome().fitness
    gens.append(generation)
    bests.append(ch.fitness)
    if ch.fitness < bestChromo.fitness:
        bestChromo = ch
    print("Gen " + str(generation) + " " + str(ch.genes) + " fitness: " + str(ch.fitness))

import matplotlib.pyplot as plt
plt.plot(gens, bests, 'ro')
plt.show()

# print("\n\nBest solutions: ")
# res = alg.bestChromosome()
# for ch in res:
#     print(str(ch.genes) + " fitness: " + str(ch.fitness))

# print("\n\nFor solution 1:")
# path = buildPath(res[0].genes, net['source'], net['destination'], net['mat'])
# print(str(net['source']) + " -> " + str(net['destination']) + " cost: " + str(path['cost']) + " " + str(path['path']))

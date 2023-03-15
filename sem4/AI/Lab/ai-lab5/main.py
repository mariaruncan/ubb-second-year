from aco import AntColonyOptimization
from utils import readData, parser
import matplotlib.pyplot as plt

net = parser("data/berlin52.tsp")

params = {"noAnts": 50, "noGen": 300, "removeRate": 15, "pheromone": [], "evaporationRate": 0.01, "q0": 0.01,
          "alpha": 5, "beta": 3, "mat": net["mat"], "noNodes": net["noNodes"]}

aco = AntColonyOptimization(params)
aco.initialise()
aco.initialisePhero()
bestAnt = None
gens = []
bests = []

for gen in range(params['noGen']):
    aco.explore()
    ant = aco.bestAnt()
    if bestAnt is None or ant.distance() < bestAnt.distance():
        bestAnt = ant
    if gen % params["removeRate"] == 0:
        aco.deleteEdge()
    gens.append(gen)
    bests.append(ant.distance())
    aco.initialise()
    print("Gen " + str(gen) + " distance: " + str(ant.distance()) + " path: " + str(ant.path()))

print("\n\nBest ant: " + str(bestAnt.distance()) + " path: " + str(bestAnt.path()))

plt.plot(gens, bests, markersize=12)
text = 'Params:' + '\nnoAnts:' + str(params['noAnts']) + '\nnoGen:' + str(params['noGen']) + \
      '\nevaporationRate:' + str(params['evaporationRate']) + '\nq0:' + str(params['q0']) + \
      '\nalpha:' + str(params['alpha']) + '\nbeta:' + str(params['beta'])
plt.legend(title=text)
plt.savefig("output.png")
plt.show()

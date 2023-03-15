from random import randint
import networkx as nx
import numpy as np
import matplotlib.pyplot as plt


def generateNewValue(lim1, lim2):
    return randint(lim1, lim2)


def readGMLFile(fileName):
    print('---------------------------------')
    print(fileName)
    print('---------------------------------')
    if 'karate' in fileName:
        graph = nx.read_gml(fileName, label='id')
    else:
        graph = nx.read_gml(fileName)
    network = {"noNodes": graph.number_of_nodes(), "noEdges": graph.number_of_edges()}
    matrix = nx.to_numpy_matrix(graph)
    network["mat"] = matrix
    degrees = []
    noEdges = 0

    for i in range(network['noNodes']):
        d = 0
        for j in range(network['noNodes']):
            if matrix.item(i, j) == 1:
                d += 1
            if j > i:
                noEdges += matrix.item(i, j)
        degrees.append(d)

    network["degrees"] = degrees
    return network


# plot a network
def plotNetwork(network, communities=None):
    if communities is None:
        communities = [1] * network['noNodes']
    np.random.seed(123)  # to freeze the graph's view (networks uses a random view)
    A = np.matrix(network["mat"])
    G = nx.from_numpy_matrix(A)
    pos = nx.spring_layout(G)  # compute graph layout
    plt.figure(figsize=(10, 10))  # image is 8 x 8 inches
    nx.draw_networkx_nodes(G, pos, node_size=80, cmap=plt.cm.RdYlBu, node_color=communities)
    nx.draw_networkx_edges(G, pos, alpha=0.3)
    plt.show()

# prerequisites
import os
import numpy as np
import networkx as nx
import matplotlib.pyplot as plt
import warnings

warnings.simplefilter('ignore')

# citeste un fisier GML si returneaza network ul corespunzator
def readGMLFile(fileName):
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

# functie most_valuable_edge pentru a alege ce muchie va fi scoasa
def most_valuable_edge(G):
    betweenness = nx.edge_betweenness_centrality(G)  # dictionar unde cheia este (u, v) si valoarea betweenness ul
    return max(betweenness, key=betweenness.get)


def greedyCommunitiesDetection(network, noOfCommunities):
    A = np.matrix(network["mat"])
    g = nx.from_numpy_matrix(A)
    g.remove_edges_from(nx.selfloop_edges(g))

    liveNumConnectedComponents = nx.number_connected_components(g)

    while liveNumConnectedComponents < noOfCommunities:
        edge = most_valuable_edge(g)
        g.remove_edge(edge[0], edge[1])
        liveNumConnectedComponents = nx.number_connected_components(g)

    components = tuple(nx.connected_components(g))

    communities = [0] * network['noNodes']
    color = 1
    for community in components:
        for node in community:
            communities[node] = color
        color += 1
    return communities

# extrage comunitatile din fisierul .txt
def extractCommunitiesFromTxtFile(fileName):
    f = open(fileName, "r")
    lines = f.readlines()
    n = int(lines[-1].split()[0])
    communities = [0] * n
    for line in lines:
        elems = line.strip().split(" ")
        if '' in elems:
            elems.remove('')
        communities[int(elems[0])-1] = int(elems[1])
    return communities


def run(fileName, noOfCommunities):
    crtDir = os.getcwd()
    filePath = os.path.join(crtDir, 'data/real-networks/real/' + fileName, fileName + '.gml')
    network = readGMLFile(filePath)
    plotNetwork(network, greedyCommunitiesDetection(network, noOfCommunities))
    plotNetwork(network, extractCommunitiesFromTxtFile('data/real-networks/real/' + fileName + '/classLabel' + fileName
                                                       + '.txt'))


def test(fileName):
    gmlFile = 'data/real-networks/real/' + fileName + '/' + fileName + '.gml'
    txtFile = 'data/real-networks/real/' + fileName + '/classLabel' + fileName + '.txt'

    expectedCommunities = extractCommunitiesFromTxtFile(txtFile)
    noOfCommunities = max(expectedCommunities)

    x = []
    for color in range(1, noOfCommunities+1):
        n = 0
        for i in expectedCommunities:
            if i == color:
                n += 1
        x.append(n)
    x.sort()

    resultCommunities = greedyCommunitiesDetection(readGMLFile(gmlFile), noOfCommunities)
    y = []
    for color in range(1, noOfCommunities + 1):
        n = 0
        for i in resultCommunities:
            if i == color:
                n += 1
        y.append(n)
    y.sort()

    for i in range(noOfCommunities):
        assert(abs(x[i] - y[i]) < 10)


def main():
    # run('dolphins', 2)
    # run('football', 12)  # multigraph 1 in gml
    # run('karate', 2) # label="id"
    # run('krebs', 3)

    test('dolphins')
    test('football')
    test('karate')
    test('krebs')


main()

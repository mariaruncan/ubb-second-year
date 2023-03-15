import numpy as np


class KMeans:

    def __init__(self, k=2, tolerance=0.001, epochs=25):
        self.classes = None
        self.centroids = None
        self.k = k
        self.epochs = epochs
        self.tolerance = tolerance

    @staticmethod
    def euclideanDistance(point1, point2):
        return np.linalg.norm(point1 - point2, axis=0)

    def fit(self, data):
        self.centroids = {}

        indexes = []
        for i in range(self.k):
            idx = np.random.randint(len(data))
            while idx in indexes:
                idx = np.random.randint(len(data))
            indexes.append(idx)
            self.centroids[i] = data[idx]

        for i in range(self.epochs):
            self.classes = {}
            for j in range(self.k):
                self.classes[j] = []

            for point in data:
                distances = []
                for index in self.centroids:
                    distances.append(self.euclideanDistance(point, self.centroids[index]))
                cluster_index = distances.index(min(distances))
                self.classes[cluster_index].append(point)

            previous = dict(self.centroids)
            for cluster_index in self.classes:
                self.centroids[cluster_index] = np.average(self.classes[cluster_index], axis=0)

            isOptimal = True

            for centroid in self.centroids:
                original_centroid = previous[centroid]
                curr = self.centroids[centroid]
                try:
                    if np.sum((curr - original_centroid) / original_centroid * 100.0) > self.tolerance:
                        isOptimal = False
                except:
                    pass
            if isOptimal:
                break

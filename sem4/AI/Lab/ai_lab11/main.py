import numpy as np

from kmeans import KMeans
from utils import loadData, getVocabulary, vectorizeSentences


def main():
    inputs, outputs, labelNames = loadData('data.csv')

    np.random.seed(8)

    noSamples = len(inputs)
    indexes = [i for i in range(noSamples)]
    trainSample = np.random.choice(indexes, int(0.8 * noSamples), replace=False)
    testSample = [i for i in indexes if i not in trainSample]

    trainInputs = [inputs[i] for i in trainSample]
    trainOutputs = [outputs[i] for i in trainSample]
    testInputs = [inputs[i] for i in testSample]
    testOutputs = [outputs[i] for i in testSample]

    vocabulary = getVocabulary(trainInputs)
    trainFeatures = vectorizeSentences(trainInputs, vocabulary)
    testFeatures = vectorizeSentences(testInputs, vocabulary)

    trainFeatures = np.array(trainFeatures)
    testFeatures = np.array(testFeatures)

    kMeans = KMeans(2)
    kMeans.fit(trainFeatures)

    # Accuracy
    correct = 0
    centroids = kMeans.centroids
    for x, y in zip(testFeatures, testOutputs):
        distances = [kMeans.euclideanDistance(x, centroids[0]), kMeans.euclideanDistance(x, centroids[1])]
        if (distances[0] < distances[1] and y == 'positive') or (distances[1] < distances[0] and y == 'negative'):
            correct += 1

    print("Accuracy: ", correct / len(testOutputs))


if __name__ == "__main__":
    main()
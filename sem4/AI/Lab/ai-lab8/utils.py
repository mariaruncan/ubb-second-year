import csv

import numpy as np
from matplotlib import pyplot as plt
from sklearn.preprocessing import StandardScaler


def loadData(fileName, inputVariabNames, outputVariabName):
    data = []
    dataNames = []
    with open(fileName) as csv_file:
        csv_reader = csv.reader(csv_file, delimiter=',')
        line_count = 0
        for row in csv_reader:
            if line_count == 0:
                dataNames = row
            else:
                data.append(row)
            line_count += 1

    inputs = []
    for i in range(len(data)):
        rowValues = []
        for feature in inputVariabNames:
            featureIndex = dataNames.index(feature)
            rowValues.append(float(data[i][featureIndex]))
        inputs.append(rowValues)

    selectedOutput = dataNames.index(outputVariabName)
    outputs = [float(data[i][selectedOutput]) for i in range(len(data))]

    return inputs, outputs

def plotDataHistogram(x, variableName):
    n, bins, patches = plt.hist(x, 10)
    plt.title('Histogram of ' + variableName)
    plt.show()

def mse(computed, real):
    error = 0.0
    for y1, y2 in zip(computed, real):
        error += (y1 - y2) ** 2
    error /= len(real)
    return error


def normalisation(trainData, testData):
    scaler = StandardScaler()
    if not isinstance(trainData[0], list):
        # encode each sample into a list
        trainData = [[d] for d in trainData]
        testData = [[d] for d in testData]

        scaler.fit(trainData)  # fit only on training data
        normalisedTrainData = scaler.transform(trainData)  # apply same transformation to train data
        normalisedTestData = scaler.transform(testData)  # apply same transformation to test data

        # decode from list to raw values
        normalisedTrainData = [el[0] for el in normalisedTrainData]
        normalisedTestData = [el[0] for el in normalisedTestData]
    else:
        scaler.fit(trainData)  # fit only on training data
        normalisedTrainData = scaler.transform(trainData)  # apply same transformation to train data
        normalisedTestData = scaler.transform(testData)  # apply same transformation to test data
    return normalisedTrainData, normalisedTestData


def plot3D(xTrain=None, yTrain=None, zTrain=None, xModel=None, yModel=None, zModel=None, xTest=None, yTest=None, zTest=None, title=None):
    fig = plt.figure(figsize=(8, 8))
    ax = fig.add_subplot(111, projection='3d')
    if xTrain:
        ax.scatter(xTrain, yTrain, zTrain, label='training data', marker='x', c=[0 for _ in range(len(zTrain))])
    if xModel:
        ax.scatter(xModel, yModel, zModel, label='learnt model', marker='^')
    if xTest:
        ax.scatter(xTest, yTest, zTest, label='test data', marker='^')
    ax.set_xlabel("GDP per capita")
    ax.set_ylabel("Freedom")
    ax.set_zlabel("Happiness")
    if title is not None:
        ax.set_title(title)
    ax.legend()
    plt.show()

import sys

import numpy as np
from matplotlib import pyplot as plt
from sklearn import linear_model
from sklearn.datasets import load_linnerud
from sklearn.metrics import mean_squared_error

from regressor import GDRegression
from utils import loadData, normalisation, plotDataHistogram, plot3D, mse

def run(regressor, inputs, outputs):
    feature1 = [x[0] for x in inputs]
    feature2 = [x[1] for x in inputs]
    plotDataHistogram(feature1, "GDP per capita")
    plotDataHistogram(feature2, "freedom")
    plotDataHistogram(outputs, "Happiness score")

    plot3D(feature1, feature2, outputs, [], [], [], [], [], [], "GDP capital and Freedom vs. Happiness")

    # Split the Data Into Training and Test Subsets(80/20%)
    np.random.seed(8)
    indexes = [i for i in range(len(inputs))]
    trainSample = np.random.choice(indexes, int(0.8 * len(inputs)), replace=False)
    testSample = [i for i in indexes if i not in trainSample]

    trainInputs = [inputs[i] for i in trainSample]
    trainOutputs = [outputs[i] for i in trainSample]

    testInputs = [inputs[i] for i in testSample]
    testOutputs = [outputs[i] for i in testSample]


    trainInputs, testInputs = normalisation(trainInputs, testInputs)
    trainOutputs, testOutputs = normalisation(trainOutputs, testOutputs)
    feature1train = [x[0] for x in trainInputs]
    feature2train = [x[1] for x in trainInputs]
    feature1test = [x[0] for x in testInputs]
    feature2test = [x[1] for x in testInputs]

    plot3D(feature1train, feature2train, trainOutputs, [], [], [], feature1test, feature2test, testOutputs,
       "train and validation data (after normalisation)")


    regressor.fit(trainInputs, trainOutputs, 0.0002)
    print(type(regressor))
    w0, w1, w2 = regressor.intercept_, regressor.coef_[0], regressor.coef_[1]
    print("the learnt model: f(x) = ", w0, " + ", w1, " * x1 + ", w2, " * x2")


    # numerical representation of the regressor model
    noOfPoints = 50
    xref = []
    val = min(feature1)
    step1 = (max(feature1) - min(feature1)) / noOfPoints
    for _ in range(1, noOfPoints):
        for _ in range(1, noOfPoints):
            xref.append(val)
        val += step1

    yref = []
    val = min(feature2)
    step2 = (max(feature2) - min(feature2)) / noOfPoints
    for _ in range(1, noOfPoints):
        aux = val
        for _ in range(1, noOfPoints):
            yref.append(aux)
            aux += step2
    zref = [w0 + w1 * el1 + w2 * el2 for el1, el2 in zip(xref, yref)]
    plot3D(feature1train, feature2train, trainOutputs, xref, yref, zref, [], [], [], "train data and learnt model")

    # use the trained model to predict new inputs
    computedTestOutputs = regressor.predict(testInputs)
    plot3D([], [], [], feature1test, feature2test, computedTestOutputs, feature1test, feature2test, testOutputs,
        "predictions vs real test data")

    print('prediction error: ', mse(computedTestOutputs, testOutputs))
    print('tool error: ', mean_squared_error(testOutputs, computedTestOutputs))
    print('\n')


sys.stdout = open('output.txt', 'w')
inputs, outputs = loadData("world-happiness-report-2017.csv", ["Economy..GDP.per.Capita.", "Freedom"], "Happiness.Score")
run(GDRegression(), inputs, outputs)
# run(linear_model.SGDRegressor(), inputs, outputs)


x = load_linnerud()
print("Wight")
run(linear_model.SGDRegressor(), [[y[0], y[1]] for y in x.data], [y[0] for y in x.target])
print("Waist")
run(linear_model.SGDRegressor(), [[y[1], y[2]] for y in x.data], [y[1] for y in x.target])
print("Pulse")
run(linear_model.SGDRegressor(), [[y[0], y[2]] for y in x.data], [y[2] for y in x.target])
sys.stdout.close()


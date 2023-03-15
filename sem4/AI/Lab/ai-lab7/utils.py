import csv

from matplotlib import pyplot as plt
from sklearn import linear_model


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

    calcInputs = []
    calcOutputs = []
    Hindex = dataNames.index("Happiness.Score")
    Windex = dataNames.index("Whisker.high")
    Eindex = dataNames.index("Economy..GDP.per.Capita.")
    for i in range(30):
        if data[i][Hindex] != '' and data[i][Eindex] != '' and data[i][Windex] != '':
            calcInputs.append([float(data[i][Hindex]), float(data[i][Windex])])
            calcOutputs.append(float(data[i][Eindex]))
    regressor = linear_model.LinearRegression()
    regressor.fit(calcInputs, calcOutputs)
    w0, w1, w2 = regressor.intercept_, regressor.coef_[0], regressor.coef_[1]

    inputs = []
    for i in range(len(data)):
        rowValues = []
        for feature in inputVariabNames:
            featureIndex = dataNames.index(feature)
            if data[i][featureIndex] == '':
                rez = w0 + float(data[i][Hindex]) * w1 + float(data[i][Windex]) * w2
                rowValues.append(rez)
            else:
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

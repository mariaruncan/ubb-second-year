import math
from cmath import sqrt

from sklearn.metrics import confusion_matrix, accuracy_score, precision_score, recall_score
import pandas as pd


def runSport():
    filepath = 'sport.csv'
    data = pd.read_csv(filepath)

    realWeight = data['Weight'].values
    realWaist = data['Waist'].values
    realPulse = data['Pulse'].values

    computedWeight = data['PredictedWeight'].values
    computedWaist = data['PredictedWaist'].values
    computedPulse = data['PredictedPulse'].values

    # Loss functions for regression
    # Mean Absolute Error = sum(|y_real - y_computed|)
    # R? Mean Squared Error = sum((y_real - y_computed)^2)

    errorL1Weight = sum(abs(r - c) for r, c in zip(realWeight, computedWeight)) / len(realWeight)
    print('Error Weight(MAE): ', errorL1Weight)
    errorL2Weight = sqrt(sum((r - c) ** 2 for r, c in zip(realWeight, computedWeight)) / len(realWeight))
    print('Error Weight(RMSE): ', errorL2Weight)

    print('')

    errorL1Waist = sum(abs(r - c) for r, c in zip(realWaist, computedWaist)) / len(realWaist)
    print('Error Waist(MAE): ', errorL1Waist)
    errorL2Waist = sqrt(sum((r - c) ** 2 for r, c in zip(realWaist, computedWaist)) / len(realWaist))
    print('Error Waist(RMSE): ', errorL2Waist)

    print('')

    errorL1Pulse = sum(abs(r - c) for r, c in zip(realPulse, computedPulse)) / len(realPulse)
    print('Error Pulse(MAE): ', errorL1Pulse)
    errorL2Pulse = sqrt(sum((r - c) ** 2 for r, c in zip(realPulse, computedPulse)) / len(realPulse))
    print('Error Pulse(RMSE): ', errorL2Pulse)


def evaluationClassification(realLabels, computedLabels, labelNames):
    acc = accuracy_score(realLabels, computedLabels)
    precision = precision_score(realLabels, computedLabels, average=None, labels=labelNames)
    recall = recall_score(realLabels, computedLabels, average=None, labels=labelNames)
    return acc, precision, recall


def runFlower():
    filepath = 'flowers.csv'
    data = pd.read_csv(filepath)

    realLabels = data['Type'].values
    computedLabels = data['PredictedType'].values
    labelNames = []
    for label in realLabels:
        if label not in labelNames:
            labelNames.append(label)

    acc, prec, recall = evaluationClassification(realLabels, computedLabels, labelNames)
    print('\nlabels: ', labelNames, '\nacc: ', acc, '\nprecision: ', prec, '\nrecall: ', recall)


# log loss for classification
# sample = [1, 0, 0], in computedOutputs are probabilities like [0.2, 0.6, 0.2]
# sampleLoss = -1/N * sum(i=1,noClasses)(sample[i] * log(prob[i]))
# logLoss = sum(i=1,noSamples)(sampleLoss[i])
def logLoss(realLabels, computedOutputs):
    names = list(set(realLabels))
    # suppose that names[0] is the positive class
    realOutputs = [[1, 0] if label == names[0] else [0, 1] for label in realLabels]
    datasetSize = len(realLabels)
    noClasses = len(names)
    datasetCE = 0.0
    for i in range(datasetSize):
        sampleCE = - sum([realOutputs[i][j] * math.log(computedOutputs[i][j]) for j in range(noClasses)])
        datasetCE += sampleCE
    meanCE = datasetCE / datasetSize
    return meanCE


# runSport()
# runFlower()
loss = logLoss(['spam', 'spam', 'ham', 'ham', 'spam', 'ham'],
               [[0.7, 0.3], [0.2, 0.8], [0.4, 0.6], [0.9, 0.1], [0.7, 0.3], [0.4, 0.6]])
print('Loss: ', loss)

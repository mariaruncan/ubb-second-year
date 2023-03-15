import random

from sklearn.datasets import load_iris
from sklearn.metrics import multilabel_confusion_matrix

from regression import MyLogisticRegression
from utils import plotDataHistogram, normalisation, plotClassificationData, plotPredictions, mse, sigmoid

data = load_iris()
inputs = data['data']
outputs = data['target']
outputNames = data['target_names']
featureNames = list(data['feature_names'])
name1 = 'sepal length'
name2 = 'petal length'
feature1 = [feat[featureNames.index(name1 + ' (cm)')] for feat in inputs]
feature2 = [feat[featureNames.index(name2 + ' (cm)')] for feat in inputs]
inputs = [[feat[featureNames.index(name1 + ' (cm)')], feat[featureNames.index(name2 + ' (cm)')]] for feat in inputs]

# shuffle data
indexes = [i for i in range(len(outputs))]
random.shuffle(indexes)
inputs = [inputs[i] for i in indexes]
outputs = [outputs[i] for i in indexes]


import matplotlib.pyplot as plt

labels = set(outputs)
noData = len(inputs)
for crtLabel in labels:
    x = [feature1[i] for i in range(noData) if outputs[i] == crtLabel]
    y = [feature2[i] for i in range(noData) if outputs[i] == crtLabel]
    plt.scatter(x, y, label=outputNames[crtLabel])
plt.xlabel(name1)
plt.ylabel(name2)
plt.legend()
plt.show()

# plot the data distribution
plotDataHistogram(feature1, name1)
plotDataHistogram(feature2, name2)
plotDataHistogram(outputs, 'flower class')

leftEnd = 0
rightEnd = 0
finalErrorSum = 99999999
finalTrainIn = []
finalTestIn = []
finalTrainOut = []
finalTestOut = []
finalClassifierSetosa = None
finalClassifierVersicolor = None
finalClassifierVirginica = None

for i in range(5):
    rightEnd = int(leftEnd + noData / 5)  # se impart datele in 5 sectiuni egale, din care una va fi pt test
    testSample = [i for i in range(leftEnd, rightEnd, 1)]
    trainSample = [i for i in range(noData) if i not in testSample]

    trainInputs = [inputs[i] for i in trainSample]
    trainOutputs = [outputs[i] for i in trainSample]
    testInputs = [inputs[i] for i in testSample]
    testOutputs = [outputs[i] for i in testSample]

    # normalise the features
    trainInputs, testInputs = normalisation(trainInputs, testInputs)

    trainOutSetosa = [1 if el == 0 else 0 for el in trainOutputs]
    trainOutVersicolor = [1 if el == 1 else 0 for el in trainOutputs]
    trainOutVirginica = [1 if el == 2 else 0 for el in trainOutputs]

    testOutSetosa = [1 if el == 0 else 0 for el in testOutputs]
    testOutVersicolor = [1 if el == 1 else 0 for el in testOutputs]
    testOutVirginica = [1 if el == 2 else 0 for el in testOutputs]

    classifierSetosa = MyLogisticRegression()
    classifierVersicolor = MyLogisticRegression()
    classifierVirginica = MyLogisticRegression()

    classifierSetosa.fit(trainInputs, trainOutSetosa)
    classifierVersicolor.fit(trainInputs, trainOutVersicolor)
    classifierVirginica.fit(trainInputs, trainOutVirginica)

    computedOutSetosa = classifierSetosa.predict(testInputs)
    computedOutVersicolor = classifierVersicolor.predict(testInputs)
    computedOutVirginica = classifierVirginica.predict(testInputs)

    errorSetosa = mse(computedOutSetosa, testOutSetosa)
    errorVersicolor = mse(computedOutVersicolor, testOutVersicolor)
    errorVirginica = mse(computedOutVirginica, testOutVirginica)

    crtErrorSum = errorSetosa + errorVersicolor + errorVirginica

    if crtErrorSum < finalErrorSum:
        finalErrorSum = crtErrorSum
        finalTrainIn = trainInputs
        finalTrainOut = trainOutputs
        finalTestIn = testInputs
        finalTestOut = testOutputs
        finalClassifierSetosa = classifierSetosa
        finalClassifierVersicolor = classifierVersicolor
        finalClassifierVirginica = classifierVirginica

    leftEnd = rightEnd


w0, w1, w2 = finalClassifierSetosa.intercept_, finalClassifierSetosa.coef_[0], finalClassifierSetosa.coef_[1]
print('classification model(setosa vs. others): y(feat1, feat2) = ', w0, ' + ', w1, ' * feat1 + ', w2, ' * feat2')
modelSetosa = [w0, w1, w2]
w0, w1, w2 = finalClassifierVersicolor.intercept_, finalClassifierVersicolor.coef_[0], finalClassifierVersicolor.coef_[1]
print('classification model(versicolor vs. others): y(feat1, feat2) = ', w0, ' + ', w1, ' * feat1 + ', w2, ' * feat2')
modelVersicolor = [w0, w1, w2]
w0, w1, w2 = finalClassifierVirginica.intercept_, finalClassifierVirginica.coef_[0], finalClassifierVirginica.coef_[1]
print('classification model(virginica vs. others): y(feat1, feat2) = ', w0, ' + ', w1, ' * feat1 + ', w2, ' * feat2')
modelVirginica = [w0, w1, w2]


# compute test data
computedTestOutputs = []
computedTestOutSetosa = []
for x in finalTestIn:
    computedTestOutSetosa.append(sigmoid(modelSetosa[0] + modelSetosa[1] * x[0] + modelSetosa[2] * x[1]))
computedTestOutVersicolor = []
for x in finalTestIn:
    computedTestOutVersicolor.append(sigmoid(modelVersicolor[0] + modelVersicolor[1] * x[0] + modelVersicolor[2] * x[1]))
computedTestOutVirginica = []
for x in finalTestIn:
    computedTestOutVirginica.append(sigmoid(modelVirginica[0] + modelVirginica[1] * x[0] + modelVirginica[2] * x[1]))
predictions = [[x, y, z] for x, y, z in zip(computedTestOutSetosa, computedTestOutVersicolor, computedTestOutVirginica)]
for pred in predictions:
    if pred[0] > pred[1] and pred[0] > pred[2]: # setosa
        computedTestOutputs.append(0)
    elif pred[1] > pred[0] and pred[1] > pred[2]: # versicolor
        computedTestOutputs.append(1)
    else: # virginica
        computedTestOutputs.append(2)
# for x in finalTestIn:
#     computedTestOutSetosa.append(finalClassifierSetosa.predictOneSample(x))
# computedTestOutVersicolor = []
# for x in finalTestIn:
#     computedTestOutVersicolor.append(finalClassifierVersicolor.predictOneSample(x))
# computedTestOutVirginica = []
# for x in finalTestIn:
#     computedTestOutVirginica.append(finalClassifierVirginica.predictOneSample(x))
# predictions = [[x, y, z] for x, y, z in zip(computedTestOutSetosa, computedTestOutVersicolor, computedTestOutVirginica)]
# for pred in predictions:
#     if pred[0] > pred[1] and pred[0] > pred[2]: # setosa
#         computedTestOutputs.append(0)
#     elif pred[1] > pred[0] and pred[1] > pred[2]: # versicolor
#         computedTestOutputs.append(1)
#     else: # virginica
#         computedTestOutputs.append(2)

plotPredictions([x[0] for x in finalTestIn], [x[1] for x in finalTestIn], finalTestOut, computedTestOutputs, "real test data", outputNames)

expected = []
for x in finalTestOut:
    if x == 0:
        expected.append([1, 0, 0])
    elif x == 1:
        expected.append([0, 1, 0])
    else:
        expected.append([0, 0, 1])
for i in range(len(predictions)):
    for j in range(3):
        trueVal = max(predictions[i])
        if predictions[i][j] == trueVal:
            predictions[i][j] = 1
        else:
            predictions[i][j] = 0
confMat = multilabel_confusion_matrix(expected, predictions)
print('confusion matrix \n', confMat)

precision = confMat[0][0][0] / (confMat[0][0][0] + confMat[0][0][1])
recall = confMat[0][0][0] / (confMat[0][0][0] + confMat[0][1][0])
fscore = (2 * precision * recall) / (precision + recall)
print('precision setosa: ', precision)
print('recall setosa: ', recall)
print('f-score setosa: ', fscore)
precision = confMat[1][0][0] / (confMat[1][0][0] + confMat[1][0][1])
recall = confMat[1][0][0] / (confMat[1][0][0] + confMat[1][1][0])
fscore = (2 * precision * recall) / (precision + recall)
print('precision versicolor: ', precision)
print('recall versicolor: ', recall)
print('f-score versicolor: ', fscore)
precision = confMat[2][0][0] / (confMat[2][0][0] + confMat[2][0][1])
recall = confMat[2][0][0] / (confMat[2][0][0] + confMat[2][1][0])
fscore = (2 * precision * recall) / (precision + recall)
print('precision virginica: ', precision)
print('recall virginica: ', recall)
print('f-score virginica: ', fscore)

from sklearn.datasets import load_iris

from utils import plotDataHistogram, normalisation, plotClassificationData, plotPredictions

data = load_iris()
inputs = data['data']
outputs = data['target']
outputNames = data['target_names']
featureNames = list(data['feature_names'])
name1 = 'sepal width'
name2 = 'petal length'
feature1 = [feat[featureNames.index(name1 + ' (cm)')] for feat in inputs]
feature2 = [feat[featureNames.index(name2 + ' (cm)')] for feat in inputs]
inputs = [[feat[featureNames.index(name1 + ' (cm)')], feat[featureNames.index(name2 + ' (cm)')]] for feat in inputs]

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

import numpy as np

# split data into train and test subsets
np.random.seed(5)
indexes = [i for i in range(len(inputs))]
trainSample = np.random.choice(indexes, int(0.8 * len(inputs)), replace=False)
testSample = [i for i in indexes if i not in trainSample]

trainInputs = [inputs[i] for i in trainSample]
trainOutputs = [outputs[i] for i in trainSample]
testInputs = [inputs[i] for i in testSample]
testOutputs = [outputs[i] for i in testSample]

# normalise the features
trainInputs, testInputs = normalisation(trainInputs, testInputs)

# plot the normalised data
feature1train = [ex[0] for ex in trainInputs]
feature2train = [ex[1] for ex in trainInputs]
feature1test = [ex[0] for ex in testInputs]
feature2test = [ex[1] for ex in testInputs]

plotClassificationData(feature1train, feature2train, trainOutputs, outputNames, 'normalised train data')

# step3: invatare model
from sklearn import linear_model

classifier = linear_model.LogisticRegression()
classifier.fit(trainInputs, trainOutputs)
w0, w1, w2 = classifier.intercept_[0], classifier.coef_[0][0], classifier.coef_[0][1]
print('classification model(setosa vs. others): y(feat1, feat2) = ', w0, ' + ', w1, ' * feat1 + ', w2, ' * feat2')
w0, w1, w2 = classifier.intercept_[1], classifier.coef_[1][0], classifier.coef_[1][1]
print('classification model(versicolor vs. others): y(feat1, feat2) = ', w0, ' + ', w1, ' * feat1 + ', w2, ' * feat2')
w0, w1, w2 = classifier.intercept_[2], classifier.coef_[2][0], classifier.coef_[2][1]
print('classification model(virginica vs. others): y(feat1, feat2) = ', w0, ' + ', w1, ' * feat1 + ', w2, ' * feat2')

# step4: testare model, plot rezultate, forma outputului si interpretarea lui
# makes predictions for test data
computedTestOutputs = classifier.predict(testInputs)

plotPredictions(feature1test, feature2test, testOutputs, computedTestOutputs, "real test data", outputNames)

# step5: calcul metrici de performanta (acc)

# evalaute the classifier performance
# compute the differences between the predictions and real outputs
print("acc score: ", classifier.score(testInputs, testOutputs))

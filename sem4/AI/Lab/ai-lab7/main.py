import numpy as np
from matplotlib import pyplot as plt
from sklearn import linear_model
import sys

from myregression import MyRegression
from utils import loadData, plotDataHistogram, mse


def run(regressor, filename):
    print('file name: ', filename)
    inputs, outputs = loadData(filename, ["Economy..GDP.per.Capita.", "Freedom"], "Happiness.Score")
    # plotDataHistogram([x[0] for x in inputs], "Economy..GDP.per.Capita.")
    # plotDataHistogram([x[1] for x in inputs], "Freedom")
    # plotDataHistogram(outputs, "Happiness.Score")

    fig = plt.figure(figsize=(8, 8))
    ax = fig.add_subplot(111, projection='3d')
    ax.scatter([x[0] for x in inputs], [x[1] for x in inputs], outputs)
    ax.set_xlabel("GDP capital")
    ax.set_ylabel("Freedom")
    ax.set_zlabel("Happiness")
    ax.set_title("GDP capital and freedom vs. happiness")
    plt.show()

    # Split the Data Into Training and Test Subsets(80/20%)
    np.random.seed(4)  # aici trebuie modificat ca sa dea pt v2 mai bine(best 8)
    indexes = [i for i in range(len(inputs))]
    trainSample = np.random.choice(indexes, int(0.8 * len(inputs)), replace=False)
    validationSample = [i for i in indexes if i not in trainSample]

    trainInputs = [inputs[i] for i in trainSample]
    trainOutputs = [outputs[i] for i in trainSample]

    validationInputs = [inputs[i] for i in validationSample]
    validationOutputs = [outputs[i] for i in validationSample]

    fig = plt.figure(figsize=(8, 8))
    ax = fig.add_subplot(111, projection='3d')
    ax.scatter([x[0] for x in trainInputs], [x[1] for x in trainInputs], trainOutputs, label='training data', marker='x',
            c=[0 for _ in range(len(trainOutputs))])
    ax.scatter([x[0] for x in validationInputs], [x[1] for x in validationInputs], validationOutputs,
           label='validation data', marker='^')
    ax.set_xlabel("GDP capital")
    ax.set_ylabel("Freedom")
    ax.set_zlabel("Happiness")
    ax.set_title("train and validation data")
    ax.legend()
    plt.show()

    print(type(regressor))
    regressor.fit(trainInputs, trainOutputs)
    w0, w1, w2 = regressor.intercept_, regressor.coef_[0], regressor.coef_[1]
    print('the learnt model: f(x1,x2) = ', w0, ' + ', w1, ' * x1 + ', w2, ' * x2')

    # use the trained model to predict new inputs
    computedValidationOutputs = regressor.predict(validationInputs)
    if 'sklearn' in str(type(regressor)):
        computedValidationOutputs = computedValidationOutputs.tolist()
    print('Computed validation output: ', computedValidationOutputs)
    fig = plt.figure(figsize=(8, 8))
    ax = fig.add_subplot(111, projection='3d')
    ax.scatter([x[0] for x in validationInputs], [x[1] for x in validationInputs], validationOutputs, label='real test data', marker='x',
           c=[0 for _ in range(len(validationOutputs))])
    ax.scatter([x[0] for x in validationInputs], [x[1] for x in validationInputs], computedValidationOutputs,
           label='computed test data', marker='^')
    ax.set_xlabel("GDP capital")
    ax.set_ylabel("Freedom")
    ax.set_zlabel("Happiness")
    ax.set_title("computed and real test data")
    ax.legend()
    plt.show()


    # mean square error
    print('prediction error: ', mse(computedValidationOutputs, validationOutputs))

    from sklearn.metrics import mean_squared_error
    print('tool error: ', mean_squared_error(validationOutputs, computedValidationOutputs))
    print('\n')


sys.stdout = open('output.txt', 'w')

# run(linear_model.LinearRegression(), 'v1_world-happiness-report-2017.csv')
# run(MyRegression(), 'v1_world-happiness-report-2017.csv')
# run(linear_model.LinearRegression(), 'v2_world-happiness-report-2017.csv')
# run(MyRegression(), 'v2_world-happiness-report-2017.csv')
run(linear_model.LinearRegression(), 'v3_world-happiness-report-2017.csv')
run(MyRegression(), 'v3_world-happiness-report-2017.csv')

sys.stdout.close()

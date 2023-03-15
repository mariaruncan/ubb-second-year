import random

from matplotlib import pyplot as plt


class GDRegression:
    def __init__(self):
        self.intercept_ = 0.0
        self.coef_ = []
        self.error_list = []

    # simple stochastic GD
    def fit(self, inputs, outputs, learningRate = 0.001, noEpochs = 1000):
        # self.coef_ = [0.0 for _ in range(len(inputs[0]) + 1)]    #beta or w coefficients y = w0 + w1 * x1 + w2 * x2 + ...
        self.coef_ = [random.random() for _ in range(len(inputs[0]) + 1)]    #beta or w coefficients
        errors = []
        for epoch in range(noEpochs):
            n = len(inputs)
            noFeaturess = len(inputs[0])
            # TBA: shuffle the training examples in order to prevent cycles
            error = []
            for i in range(n): # for each sample from the training data
                ycomputed = self.eval(inputs[i])     # estimate the output
                crtError = ycomputed - outputs[i]     # compute the error for the current sample
                error.append(crtError)
                for j in range(noFeaturess):   # calculate errors
                    self.coef_[j] = self.coef_[j] - learningRate * crtError * inputs[i][j]
                self.coef_[noFeaturess] = self.coef_[noFeaturess] - learningRate * crtError * 1
            errors.append(sum(error) / len(error))

        self.intercept_ = self.coef_[-1]
        self.coef_ = self.coef_[:-1]

        plt.plot(errors, [x for x in range(len(errors))])

    def eval(self, xi):
        yi = self.coef_[-1]
        for j in range(len(xi)):
            yi += self.coef_[j] * xi[j]
        return yi 

    def predict(self, x):
        yComputed = [self.eval(xi) for xi in x]
        return yComputed


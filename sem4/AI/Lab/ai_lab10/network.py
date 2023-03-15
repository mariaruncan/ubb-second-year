import numpy as np
from matplotlib import pyplot as plt


class Layer:
    def __init__(self, number_of_inputs_per_neuron, number_of_neurons, func):
        self.weights = 2 * np.random.random((number_of_inputs_per_neuron, number_of_neurons)) - 1
        self.func = func


class Network:
    def __init__(self):
        self.layers = []
        self.inputs = []
        self.outputs = []
        self.error_per_layer = []
        self.deltas = []

    def add_layer(self, layer):
        self.layers.append(layer)
        self.inputs.append(None)
        self.outputs.append(None)
        self.error_per_layer.append(None)
        self.deltas.append(None)

    def fit(self, x_train, y_train, epochs=1000, learning_rate=0.05):
        ep = []
        errors = []
        self.inputs[0] = x_train
        for epoch in range(epochs):
            self.forward_propagation()

            net_output = self.outputs[-1]
            self.error_per_layer[-1] = y_train - net_output

            self.backward_propagation()
            self.update_weights(learning_rate)

            ep.append(epoch)
            errors.append(self.compute_error(net_output, y_train))

        plt.scatter(ep, errors, s=1)
        plt.show()

    def compute_error(self, output, real):
        sum = 0
        for i in range(len(real)):
            sum -= np.sum(real[i] * np.log(output[i]))
        sum /= len(real)
        return sum

    def update_weights(self, learning_rate):
        for i in range(len(self.layers)):
            adjustment = learning_rate * self.inputs[i].T.dot(self.deltas[i])
            self.layers[i].weights += adjustment

    def forward_propagation(self):
        for i in range(len(self.layers)):
            if i != 0:
                self.inputs[i] = self.outputs[i - 1]
            ws = self.layers[i].weights
            ins = self.inputs[i]
            self.outputs[i] = self.layers[i].func(np.dot(ins, ws))

    def backward_propagation(self):
        for i in range(len(self.layers) - 1, -1, -1):
            if i != len(self.layers) - 1:
                delta = self.deltas[i + 1]
                ws = self.layers[i + 1].weights
                self.error_per_layer[i] = delta.dot(ws.T)
            err = self.error_per_layer[i]
            layer = self.layers[i]
            out = self.outputs[i]
            self.deltas[i] = err * layer.func(out, der=True)

    def predict(self, x_test):
        self.inputs[0] = x_test
        self.forward_propagation()
        return self.outputs[-1]

    def predict_digits(self, x_test):
        rez = []
        self.inputs[0] = x_test
        self.forward_propagation()
        outputs = self.outputs[-1]

        for i in range(len(outputs)):
            if outputs[i][0] == max(outputs[i]):
                rez.append(0)
            elif outputs[i][1] == max(outputs[i]):
                rez.append(1)
            elif outputs[i][2] == max(outputs[i]):
                rez.append(2)
            elif outputs[i][3] == max(outputs[i]):
                rez.append(3)
            elif outputs[i][4] == max(outputs[i]):
                rez.append(4)
            elif outputs[i][5] == max(outputs[i]):
                rez.append(5)
            elif outputs[i][6] == max(outputs[i]):
                rez.append(6)
            elif outputs[i][7] == max(outputs[i]):
                rez.append(7)
            elif outputs[i][8] == max(outputs[i]):
                rez.append(8)
            elif outputs[i][9] == max(outputs[i]):
                rez.append(9)
        return rez

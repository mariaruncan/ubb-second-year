import numpy as np
from matplotlib import pyplot as plt

from network import Network, Layer
from utils import split_data, normalisation, transform_outputs, load_digits_data, transform_outputs_digits, sigmoid, \
    eval_multi_class, plot_confusion_matrix


def plot_features_and_labels(inputs, outputs):
    labels = set(outputs)
    no_data = len(inputs)
    for crtLabel in labels:
        x = [inputs[i][0] for i in range(no_data) if outputs[i] == crtLabel]
        y = [inputs[i][1] for i in range(no_data) if outputs[i] == crtLabel]
        plt.scatter(x, y, label=output_names[crtLabel])
    plt.xlabel('feat1')
    plt.ylabel('feat2')
    plt.legend()
    plt.show()


def flatten(mat):
    x = []
    for line in mat:
        for el in line:
            x.append(el)
    return x


inputs, outputs, output_names = load_digits_data()
x_train, y_train, x_test, y_test = split_data(inputs, outputs)

# plot the training data distribution on classes
plt.hist(y_train, rwidth=0.8)
plt.xticks(np.arange(len(output_names)), output_names)
plt.show()

x_train_flatten = [flatten(el) for el in x_train]
x_test_flatten = [flatten(el) for el in x_test]

# normalise the data
x_train, x_test = normalisation(x_train_flatten, x_test_flatten)

y_train = transform_outputs_digits(y_train)

# are 64 feats, +1 pt termenul liber
layer1 = Layer(65, 100, sigmoid)
layer2 = Layer(100, 10, sigmoid)

myNetwork = Network()
myNetwork.add_layer(layer1)
myNetwork.add_layer(layer2)

myNetwork.fit(x_train, y_train, 1000, 0.0008)
predictions = myNetwork.predict(x_test)

y_computed = []
for pred in predictions:
    y_computed.append(np.argmax(pred, axis=0))

acc, prec, recall, cm = eval_multi_class(np.array(y_test), y_computed, output_names)
plot_confusion_matrix(cm, output_names, "digits classification")

print('acc: ', acc)
print('precision: ', prec)
print('recall: ', recall)

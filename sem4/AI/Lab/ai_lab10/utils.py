import numpy as np
from matplotlib import pyplot as plt
from sklearn.datasets import load_iris, load_digits
from sklearn.preprocessing import StandardScaler


def load_irisi_data():
    data = load_iris()
    inputs = data['data']
    outputs = data['target']
    output_names = data['target_names']
    feature_names = list(data['feature_names'])
    inputs = [[feat[feature_names.index('sepal length (cm)')], feat[feature_names.index('petal length (cm)')]] for feat in
              inputs]

    # shuffle the original data
    no_data = len(inputs)
    permutation = np.random.permutation(no_data)
    inputs = np.array(inputs)[permutation]
    outputs = np.array(outputs)[permutation]

    return inputs.tolist(), outputs.tolist(), output_names


def split_data(inputs, outputs):
    np.random.seed(47)
    indexes = [i for i in range(len(inputs))]
    train_sample = np.random.choice(indexes, int(0.8 * len(inputs)), replace=False)
    test_sample = [i for i in indexes if i not in train_sample]

    x_train = [inputs[i] for i in train_sample]
    y_train = [outputs[i] for i in train_sample]
    x_test = [inputs[i] for i in test_sample]
    y_test = [outputs[i] for i in test_sample]

    return x_train, y_train, x_test, y_test


def normalisation(x_train, x_test):
    scaler = StandardScaler()
    if not isinstance(x_train[0], list):
        # encode each sample into a list
        x_train = [[d] for d in x_train]
        x_test = [[d] for d in x_test]

        scaler.fit(x_train)  # fit only on training data
        x_train_normalised = scaler.transform(x_train)  # apply same transformation to train data
        x_test_normalised = scaler.transform(x_test)  # apply same transformation to test data

        # decode from list to raw values
        x_train_normalised = [el[0] for el in x_train_normalised]
        x_test_normalised = [el[0] for el in x_test_normalised]

    else:
        scaler.fit(x_train)  # fit only on training data
        x_train_normalised = scaler.transform(x_train)  # apply same transformation to train data
        x_test_normalised = scaler.transform(x_test)  # apply same transformation to test data

    x_train_normalised2 = []
    for el in x_train_normalised:
        x_train_normalised2.append(np.append(el, 1))

    x_test_normalised2 = []
    for el in x_test_normalised:
        x_test_normalised2.append(np.append(el, 1))

    return np.asarray(x_train_normalised2), np.asarray(x_test_normalised2)


def transform_outputs(outputs):
    rez = []
    for output in outputs:
        if output == 0:
            rez.append([1, 0, 0])
        elif output == 1:
            rez.append([0, 1, 0])
        else:
            rez.append([0, 0, 1])
    return rez


def load_digits_data():
    data = load_digits()
    inputs = data.images
    outputs = data['target']
    output_names = data['target_names']

    # shuffle the original data
    no_data = len(inputs)
    permutation = np.random.permutation(no_data)
    inputs = inputs[permutation]
    outputs = outputs[permutation]

    return inputs, outputs, output_names


def transform_outputs_digits(outputs):
    rez = []
    for x in outputs:
        y = []
        curr_pos = 0
        while curr_pos < 10:
            if curr_pos == x:
                y.append(1)
            else:
                y.append(0)
            curr_pos += 1
        rez.append(y)
    return rez


def eval_multi_class(real_labels, computed_labels, label_names):
    from sklearn.metrics import confusion_matrix

    conf_matrix = confusion_matrix(real_labels, computed_labels)
    acc = sum([conf_matrix[i][i] for i in range(len(label_names))]) / len(real_labels)
    precision = {}
    recall = {}
    for i in range(len(label_names)):
        precision[label_names[i]] = conf_matrix[i][i] / sum([conf_matrix[j][i] for j in range(len(label_names))])
        recall[label_names[i]] = conf_matrix[i][i] / sum([conf_matrix[i][j] for j in range(len(label_names))])
    return acc, precision, recall, conf_matrix


def plot_confusion_matrix(cm, class_names, title):
    import itertools
    classes = class_names
    plt.figure()
    plt.imshow(cm, interpolation='nearest', cmap='Blues')
    plt.title('Confusion Matrix ' + title)
    plt.colorbar()
    tick_marks = np.arange(len(class_names))
    plt.xticks(tick_marks, class_names, rotation=45)
    plt.yticks(tick_marks, class_names)

    text_format = 'd'
    thresh = cm.max() / 2.
    for row, column in itertools.product(range(cm.shape[0]), range(cm.shape[1])):
        plt.text(column, row, format(cm[row, column], text_format),
                 horizontalalignment='center',
                 color='white' if cm[row, column] > thresh else 'black')

    plt.ylabel('True label')
    plt.xlabel('Predicted label')
    plt.tight_layout()

    plt.show()


def sigmoid(x, der=False):
    if not der:
        return 1 / (1 + np.exp(-x))
    else:
        return x * (1 - x)


def relu(x, der=False):
    if der:
        return np.heaviside(x, 1)
    else:
        return np.maximum(x, 0)

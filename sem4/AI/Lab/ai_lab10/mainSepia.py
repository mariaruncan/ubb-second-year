import os
from statistics import mean

import cv2
import numpy as np
from skimage.io import imread
from skimage.transform import resize
from sklearn import neural_network
from utils import split_data, eval_multi_class, plot_confusion_matrix, normalisation


def load_images():
    data = dict()
    data['label'] = []
    data['filename'] = []
    data['data'] = []

    for file in os.listdir('data'):
        if file[-3:] in {'jpg'}:
            im = imread(os.path.join('data', file))
            im = resize(im, (100, 100))
            data['filename'].append(file)
            data['data'].append(im)
            if 'sepia' in file:
                data['label'].append('sepia')
            else:
                data['label'].append('normal')
    return data['data'], data['label']


def transform(images):
    transformed = []
    for img in images:
        im = []
        for i in range(len(img)):
            line = []
            for j in range(len(img[0])):
                pixel = img[i][j]
                pixel *= 255
                color = int(pixel[0]) << 16 + int(pixel[1]) << 8 + int(pixel[2])
                line.append(color / 255)
            im.append(line)
        transformed.append(im)

    return flatten(transformed)


def flatten(xs):
    result = []
    for x in xs:
        y = []
        for line in x:
            for el in line:
                y.append(el)
        result.append(y)
    return result


def ANN_Classification():
    inputs, outputs = load_images()
    x_train, y_train, x_test, y_test = split_data(inputs, outputs)

    x_train = transform(x_train)
    x_test = transform(x_test)

    classifier = neural_network.MLPClassifier()

    classifier.fit(x_train, y_train)
    y_predicted = classifier.predict(x_test)

    acc, prec, recall, cm = eval_multi_class(np.array(y_test), y_predicted, ['normal', 'sepia'])
    plot_confusion_matrix(cm, ['normal', 'sepia'], "photos classification")

    print('acc: ', acc)
    print('precision: ', prec)
    print('recall: ', recall)


ANN_Classification()

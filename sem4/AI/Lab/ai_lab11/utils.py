import csv
import re


def loadData(filename):
    data = []
    dataNames = []
    with open(filename) as csv_file:
        csv_reader = csv.reader(csv_file, delimiter=',')
        line_count = 0
        for row in csv_reader:
            if line_count == 0:
                dataNames = row
            else:
                data.append(row)
            line_count += 1

    inputs = [data[i][0] for i in range(len(data))]
    outputs = [data[i][1] for i in range(len(data))]
    return inputs, outputs, dataNames


def getWords(sentences):
    allWords = []
    for s in sentences:
        words = re.split('[ \n?,.()-:!&]', s)
        cleanText = [w.lower() for w in words if w != '']
        for el in cleanText:
            allWords.append(el)
    return allWords


def getVocabulary(sentences):
    words = getWords(sentences)
    dic = {}
    for w in words:
        if w not in dic.keys():
            dic[w] = 1
        else:
            dic[w] += 1
    return list(dic)


def vectorizeSentences(sentences, vocab):
    results = []
    for sentence in sentences:
        result = [0] * len(vocab)
        for word in getWords([sentence]):
            for i, w in enumerate(vocab):
                if word == w:
                    result[i] += 1
        results.append(result)
    return results

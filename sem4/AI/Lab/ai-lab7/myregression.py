class MyRegression:
    def __init__(self):
        self.intercept_ = 0.0
        self.coef_ = []

    def fit(self, X, Y):
        for el in X:
            el.insert(0, 1)
        Y = self.transpose([Y])
        rez1 = self.transpose(X)
        rez2 = self.multiply(rez1, X)
        rez3 = self.inverse(rez2)
        rez4 = self.multiply(rez3, rez1)
        rez5 = self.multiply(rez4, Y)
        self.intercept_ = rez5[0][0]
        for r in rez5[1:]:
            self.coef_.append(r[0])

    def predict(self, inputs):
        result = []
        for input in inputs:
            sum = self.intercept_
            for x, coef in zip(input, self.coef_):
                sum += x * coef
            result.append(sum)
        return result


    def transpose(self, matrix):
        transpose = []
        for j in range(len(matrix[0])):   # no columns
            line = []
            for i in range(len(matrix)):  # no rows
                line.append(matrix[i][j])
            transpose.append(line)
        return transpose

    def minor(self, matrix, i, j):
        result = []
        for x in range(len(matrix)):
            if x != i:
                line = []
                for y in range(len(matrix[0])):
                    if y != j:
                        line.append(matrix[x][y])
                result.append(line)
        return result

    def determinant(self, matrix):
        if len(matrix) == 1:
            return matrix[0][0]

        if len(matrix) == 2:
            return matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0]

        det = 0.0
        for j in range(len(matrix)): # dezvoltam dupa prima linie
            det += matrix[0][j] * ((-1) ** j) * self.determinant(self.minor(matrix, 0, j))
        return det

    def inverse(self, matrix):
        if len(matrix) != len(matrix[0]):
            raise Exception("Different number of rows and columns!")

        detMatrix = self.determinant(matrix)
        if detMatrix == 0:
            raise Exception("Matrix is not invertible!")

        result = self.adj(matrix)
        for i in range(len(result)):
            for j in range(len(result)):
                result[i][j] /= detMatrix
        return result

    def adj(self, matrix):
        matrix = self.transpose(matrix)
        result = []
        for i in range(len(matrix)):
            line = []
            for j in range(len(matrix)):
                elem = ((-1) ** (i + j)) * self.determinant(self.minor(matrix, i, j))
                line.append(elem)
            result.append(line)
        return result

    def multiply(self, a, b):
        if len(a[0]) != len(b):
            raise Exception("Can not multiply these matrices!")

        result = []
        for i in range(len(a)):
            result.append([0 for _ in range(len(b[0]))])

        for i in range(len(a)):
            for j in range(len(b[0])):
                for k in range(len(b)):
                    result[i][j] += a[i][k] * b[k][j]
        return result


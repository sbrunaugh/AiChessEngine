import pandas as pd
from keras.models import Sequential
from keras.layers import Dense
from sklearn.model_selection import KFold
import linecache
import random

# Define your model
model = Sequential()
model.add(Dense(32, activation='relu', input_dim=64))
model.add(Dense(16, activation='relu', input_dim=32))
model.add(Dense(8, activation='relu', input_dim=16))
model.add(Dense(1, activation='tanh'))  # Adjusted to output between -1 and 1
model.compile(optimizer='rmsprop', loss='mean_squared_error')  # Adjusted for regression problem

training_file = '../train_data.txt'
total_rows = 21762469

def formulate_chunk(chunk_size):
    result = []
    idx = random.randint(1, total_rows)
    for i in range(chunk_size):
        line = linecache.getline(training_file, idx).strip()
        values = line.split(',')

        if(values == None or len(values) != 65):
            continue

        for i in range(64):
            values[i] = int(values[i])
        values[64] = float(values[64])
        result.append(values)
        idx += 100

    print(result[0])
    print(result[1])
    return result

chunksize = 10000  # Adjust based on your system's memory

# Define the number of splits for the KFold cross-validation
n_splits = 5
kf = KFold(n_splits=n_splits)

counter = 1
num_iterations = 20
while counter < num_iterations:
    rawChunk = formulate_chunk(chunk_size=chunksize)
    chunk = pd.DataFrame(rawChunk)
    X = chunk.iloc[:, :-1]  # All columns except the last
    y = chunk.iloc[:, -1]  # Only the last column

    print(f"Performing cross-validation on chunk {counter} of {num_iterations}...")

    # Perform cross-validation
    for train_index, test_index in kf.split(X):
        X_train, X_test = X.iloc[train_index], X.iloc[test_index]
        y_train, y_test = y.iloc[train_index], y.iloc[test_index]

        # Train your model
        model.fit(X_train, y_train, epochs=10, batch_size=64)

        # Evaluate your model
        score = model.evaluate(X_test, y_test, verbose=0)
        print(f"Chunk {counter}, Fold {train_index//len(X_train) + 1}, Score: {score}")

    print(f"Finished cross-validation on chunk {counter} of {num_iterations}.")
    counter += 1

model.save('sbrunaugh_chess_model_v4.keras')  # Saves model to disk
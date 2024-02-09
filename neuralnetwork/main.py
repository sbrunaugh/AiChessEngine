import pandas as pd
from keras.models import Sequential
from keras.layers import Dense
from sklearn.model_selection import KFold

# Define your model
model = Sequential()
model.add(Dense(32, activation='relu', input_dim=64))
model.add(Dense(16, activation='relu', input_dim=32))
model.add(Dense(8, activation='relu', input_dim=16))
model.add(Dense(1, activation='tanh'))  # Adjusted to output between -1 and 1
model.compile(optimizer='rmsprop', loss='mean_squared_error')  # Adjusted for regression problem

chunksize = 100000  # Adjust based on your system's memory
total_rows = 800000
print(f"Total of {total_rows} in training data.")
num_chunks = total_rows // chunksize + 1  # Total number of chunks

# Define the number of splits for the KFold cross-validation
n_splits = 3
kf = KFold(n_splits=n_splits)

counter = 1
for chunk in pd.read_csv('../train_data.txt', chunksize=chunksize):
    # Separate features and labels
    X = chunk.iloc[:, :-1]  # All columns except the last
    y = chunk.iloc[:, -1]  # Only the last column

    print(f"Performing cross-validation on chunk {counter} of {num_chunks}...")

    # Perform cross-validation
    for train_index, test_index in kf.split(X):
        X_train, X_test = X.iloc[train_index], X.iloc[test_index]
        y_train, y_test = y.iloc[train_index], y.iloc[test_index]

        # Train your model
        model.fit(X_train, y_train, epochs=10, batch_size=64)

        # Evaluate your model
        score = model.evaluate(X_test, y_test, verbose=0)
        print(f"Chunk {counter}, Fold {train_index//len(X_train) + 1}, Score: {score}")

    print(f"Finished cross-validation on chunk {counter} of {num_chunks}.")
    counter += 1

model.save('sbrunaugh_chess_model_v3-lite.keras')  # Saves model to disk
import pandas as pd
from keras.models import Sequential
from keras.layers import Dense
from sklearn.model_selection import KFold

chunksize = 100000  # Adjust based on your system's memory
total_rows_white = 395657#0
total_rows_black = 402352#9

num_chunks_white = total_rows_white // chunksize + 1  # Total number of chunks
num_chunks_black = total_rows_white // chunksize + 1  # Total number of chunks

# Define the number of splits for the KFold cross-validation
n_splits = 5
kf = KFold(n_splits=n_splits)

# --- BLACK ---

# Define your model
model_black = Sequential()
model_black.add(Dense(64, activation='relu', input_dim=64))
model_black.add(Dense(64, activation='relu', input_dim=64))
model_black.add(Dense(32, activation='relu', input_dim=64))
model_black.add(Dense(16, activation='relu', input_dim=32))
model_black.add(Dense(8, activation='relu', input_dim=16))
model_black.add(Dense(4, activation='relu', input_dim=8))
model_black.add(Dense(2, activation='relu', input_dim=8))
model_black.add(Dense(1, activation='tanh'))  # Adjusted to output between -1 and 1
model_black.compile(optimizer='rmsprop', loss='mean_squared_error')  # Adjusted for regression problem

counter = 1
for chunk in pd.read_csv('../train_data_black.txt', chunksize=chunksize):
    # Separate features and labels
    X = chunk.iloc[:, :-1]  # All columns except the last
    y = chunk.iloc[:, -1]  # Only the last column

    print(f"Performing cross-validation on chunk {counter} of {num_chunks_black}...")

    # Perform cross-validation
    for train_index, test_index in kf.split(X):
        X_train, X_test = X.iloc[train_index], X.iloc[test_index]
        y_train, y_test = y.iloc[train_index], y.iloc[test_index]

        # Train your model
        model_black.fit(X_train, y_train, epochs=10, batch_size=64)

        # Evaluate your model
        score = model_black.evaluate(X_test, y_test, verbose=0)
        print(f"Chunk {counter}, Fold {train_index//len(X_train) + 1}, Score: {score}")

    print(f"Finished cross-validation on chunk {counter} of {num_chunks_black}.")
    counter += 1
    if(counter >= 5):
        break

model_black.save('sbrunaugh_chess_model_v7_black.keras')

# --- WHITE ---

# Define your model
model_white = Sequential()
model_white.add(Dense(64, activation='relu', input_dim=64))
model_white.add(Dense(64, activation='relu', input_dim=64))
model_white.add(Dense(32, activation='relu', input_dim=64))
model_white.add(Dense(16, activation='relu', input_dim=32))
model_white.add(Dense(8, activation='relu', input_dim=16))
model_white.add(Dense(4, activation='relu', input_dim=8))
model_white.add(Dense(2, activation='relu', input_dim=8))
model_white.add(Dense(1, activation='tanh'))  # Adjusted to output between -1 and 1
model_white.compile(optimizer='rmsprop', loss='mean_squared_error')  # Adjusted for regression problem

counter = 1
for chunk in pd.read_csv('../train_data_white.txt', chunksize=chunksize):
    # Separate features and labels
    X = chunk.iloc[:, :-1]  # All columns except the last
    y = chunk.iloc[:, -1]  # Only the last column

    print(f"Performing cross-validation on chunk {counter} of {num_chunks_white}...")

    # Perform cross-validation
    for train_index, test_index in kf.split(X):
        X_train, X_test = X.iloc[train_index], X.iloc[test_index]
        y_train, y_test = y.iloc[train_index], y.iloc[test_index]

        # Train your model
        model_white.fit(X_train, y_train, epochs=10, batch_size=64)

        # Evaluate your model
        score = model_white.evaluate(X_test, y_test, verbose=0)
        print(f"Chunk {counter}, Fold {train_index//len(X_train) + 1}, Score: {score}")

    print(f"Finished cross-validation on chunk {counter} of {num_chunks_white}.")
    counter += 1
    if(counter >= 5):
        break

model_white.save('sbrunaugh_chess_model_v7_white.keras')
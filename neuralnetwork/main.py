import pandas as pd
from keras.models import Sequential
from keras.layers import Dense

# Define your model
model = Sequential()
model.add(Dense(32, activation='relu', input_dim=64))
model.add(Dense(16, activation='relu', input_dim=32))
model.add(Dense(8, activation='relu', input_dim=16))
model.add(Dense(1, activation='tanh'))  # Adjusted to output between -1 and 1
model.compile(optimizer='rmsprop', loss='mean_squared_error')  # Adjusted for regression problem

chunksize = 100000  # Adjust based on your system's memory
total_rows = 79000000
print(f"Total of {total_rows} in training data.")
num_chunks = total_rows // chunksize + 1  # Total number of chunks

counter = 1

for chunk in pd.read_csv('../train_data.csv', chunksize=chunksize):
    # Separate features and labels
    X_train = chunk.iloc[:, :-1]  # All columns except the last
    y_train = chunk.iloc[:, -1]  # Only the last column

    print(f"Training on chunk {counter+1} of {num_chunks}...")

    # Train your model
    model.fit(X_train, y_train, epochs=10, batch_size=64)
    print(f"Finished training on chunk {counter} of {num_chunks}.")
    counter += 1

model.save('sbrunaugh_chess_model_v2.keras')  # Saves model to disk
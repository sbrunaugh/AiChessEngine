import pandas as pd
from keras.models import Sequential
from keras.layers import Dense

# Define your model
model = Sequential()
model.add(Dense(32, activation='relu', input_dim=64))
model.add(Dense(1, activation='tanh'))  # Adjusted to output between -1 and 1
model.compile(optimizer='rmsprop', loss='mean_squared_error')  # Adjusted for regression problem

chunksize = 100000  # Adjust based on your system's memory
for chunk in pd.read_csv('../train_data.csv', chunksize=chunksize):
    # Separate features and labels
    X_train = chunk.iloc[:, :-1]  # All columns except the last
    y_train = chunk.iloc[:, -1]  # Only the last column

    # Train your model
    model.fit(X_train, y_train, epochs=10, batch_size=64)

model.save('sbrunaugh_chess_model_v1-1.h5')  # Creates a HDF5 file 'my_model.h5'
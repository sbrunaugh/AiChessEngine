import os
from keras.models import load_model
import numpy as np

# Load the model
model = load_model('./sbrunaugh_chess_model_v1-1.h5')

inputFilePath = './input.csv'
outputFilePath = './output.txt'

if os.path.exists(outputFilePath):
    os.remove(outputFilePath)

# Open the input file and output file
with open(inputFilePath, 'r') as infile, open(outputFilePath, 'w') as outfile:
    for line in infile:
        input_data = np.array(line.strip().split(','), dtype=int).reshape(1, -1)

        # Run the forward pass (i.e., make a prediction)
        prediction = model.predict(input_data)

        # Write the prediction to the output file
        outfile.write(f'{prediction[0][0]}\n')

print("predictions saved to ", outputFilePath)
import os
import json
from keras.models import load_model
import numpy as np

# Load the model
model = load_model('./sbrunaugh_chess_model_v3-lite.keras')

inputFilePath = './input.json'
outputFilePath = './output.json'

if os.path.exists(outputFilePath):
    os.remove(outputFilePath)

data = []

with open(inputFilePath, 'r') as inputJson:
    data = json.load(inputJson)
    for futureCalc in data:
        for nextMove in futureCalc['NextMoves']:
            model_input = np.array(nextMove['Position'], dtype=int).reshape(1, -1)

            # Run the forward pass
            eval = model.predict(model_input)
            nextMove['Evaluation'] = float(eval[0][0])
            print(nextMove['Evaluation'])

with open(outputFilePath, 'w') as outputJson:
    json.dump(data, outputJson)

print("JSON data has been written to ", outputFilePath)
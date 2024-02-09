import os
import json
from keras.models import load_model
import numpy as np

# Load the model
model = load_model('./sbrunaugh_chess_model_v4.keras')

inputFilePath = './input.json'
outputFilePath = './output.json'

if os.path.exists(outputFilePath):
    os.remove(outputFilePath)

data = []

with open(inputFilePath, 'r') as inputJson:
    data = json.load(inputJson)
    for futureCalc in data:
        for futureEvaluatedMove in futureCalc['FutureEvaluatedMoves']:
            model_input = np.array(futureEvaluatedMove['Move']['NewPosition'], dtype=int).reshape(1, -1)

            # Run the forward pass
            eval = model.predict(model_input)
            futureEvaluatedMove['Evaluation'] = float(eval[0][0])
            print(futureEvaluatedMove['Evaluation'])

with open(outputFilePath, 'w') as outputJson:
    json.dump(data, outputJson)

print("JSON data has been written to ", outputFilePath)
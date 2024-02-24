import os
from keras.models import load_model
import numpy as np
import json

# Load the models
#file_path = './sbrunaugh_chess_model_v10_white.keras'
#model_white = load_model(file_path)
file_path = './old_models/sbrunaugh_chess_model_v8_black.keras'
model_black = load_model(file_path)

inputFilePath = './input.json'
outputFilePath = './output.json'

if os.path.exists(outputFilePath):
    os.remove(outputFilePath)

data = []

with open(inputFilePath, 'r') as inputJson:
    data = json.load(inputJson)
    for move in data:
        is_white = bool(move['Move']['Player'])

        model_input = np.array(move['Move']['NewPosition'], dtype=int).reshape(1, -1)

        # Run the forward pass
        #if (is_white):
            #eval = model_white.predict(model_input)
        #else:
        eval = model_black.predict(model_input)
        
        move['Evaluation'] = float(eval[0][0])
        print(move['Evaluation'])

with open(outputFilePath, 'w') as outputJson:
    json.dump(data, outputJson)

print("JSON data has been written to ", outputFilePath)
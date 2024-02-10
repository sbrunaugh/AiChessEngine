import os
import json
from keras.models import load_model
import numpy as np
import argparse

parser = argparse.ArgumentParser(description="A program that receives a string as an argument")
parser.add_argument('player', type=str, help='The string to be passed as argument')
args = parser.parse_args()
print(f"Received string: {args.player}")
assert(args.player == 'white' or 'black')

# Load the model
file_path = './sbrunaugh_chess_model_v8_' + args.player + '.keras'
model = load_model(file_path)

inputFilePath = './input.json'
outputFilePath = './output.json'

if os.path.exists(outputFilePath):
    os.remove(outputFilePath)

data = []

with open(inputFilePath, 'r') as inputJson:
    data = json.load(inputJson)
    for move_eval in data:
        model_input = np.array(move_eval['Move']['NewPosition'], dtype=int).reshape(1, -1)

        # Run the forward pass
        eval = model.predict(model_input)
        move_eval['Evaluation'] = float(eval[0][0])
        print(move_eval['Evaluation'])

with open(outputFilePath, 'w') as outputJson:
    json.dump(data, outputJson)

print("JSON data has been written to ", outputFilePath)
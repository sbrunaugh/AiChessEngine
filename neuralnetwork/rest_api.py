from flask import Flask, jsonify, request
import ChessModel as cm

app = Flask(__name__)

# A simple in-memory data store
data = {}

chessModel = cm.ChessModel()

@app.route('/api/forward', methods=['POST'])
def handle_data():
    if request.method != 'POST':
        raise ValueError("API call must be a POST request.")
    
    body = request.get_json()
    for obj in body:
        # If looking at move made by white, then black to move
        if obj['move']['player'] == 1:
            obj['evaluation'] = chessModel.forward_pass(obj['move']['newPosition'], False)
        # If looking at move made by black, then white to move
        else:
            obj['evaluation'] = chessModel.forward_pass(obj['move']['newPosition'], True)

    return body

if __name__ == '__main__':
    app.run(debug=True)

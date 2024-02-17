from keras.models import load_model
import numpy as np

class ChessModel:
    w_file_path = './sbrunaugh_chess_model_v11_white.keras'
    b_file_path = './sbrunaugh_chess_model_v11_black.keras'

    def __init__(self) -> None:
        self.model_white = load_model(self.w_file_path)
        self.model_black = load_model(self.b_file_path)

    def forward_pass(self, position, is_white_to_move) -> float:
        if len(position) != 64:
            raise ValueError("Position array must have exactly 64 items.")
        
        model_input = np.array(position, dtype=int).reshape(1, -1)
        
        eval = None

        # Run the forward pass
        if (is_white_to_move):
            eval = self.model_white.predict(model_input)
            print('white-to-move-model evaluated position as ', eval[0][0])
        else:
            eval = self.model_black.predict(model_input)
            print('black-to-move-model evaluated position as ', eval[0][0])
        
        return float(eval[0][0])
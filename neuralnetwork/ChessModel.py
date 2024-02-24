from keras.models import load_model
import numpy as np

class ChessModel:
    file_path = './sbrunaugh_chess_model_v13.keras'

    def __init__(self) -> None:
        self.model = load_model(self.file_path)

    def forward_pass(self, position, is_white_to_move) -> float:
        if len(position) != 64:
            raise ValueError("Position array must have exactly 64 items.")
        
        model_input = np.array(position, dtype=int).reshape(1, -1)
        
        # Run the forward pass
        eval = self.model.predict(model_input)
        print('model evaluated position as ', eval[0][0])
        
        return float(eval[0][0])
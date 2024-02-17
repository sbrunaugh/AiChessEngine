//using DecisionEngine.Models;
//using Keras.Models;

//namespace DecisionEngine
//{
//    internal class NeuralNetwork
//    {
//        private BaseModel _whiteModel;
//        private BaseModel _blackModel;

//        public NeuralNetwork(string whiteModelFilePath, string blackModelFilePath)
//        {
//            string pythonHome = @"C:\Users\bruna\AppData\Local\Programs\Python\Python38";  // replace with your Python path
//            string pythonLib = pythonHome + @"\Lib";
//            Environment.SetEnvironmentVariable("PYTHONHOME", pythonHome);
//            Environment.SetEnvironmentVariable("PYTHONPATH", pythonLib);

//            _whiteModel = BaseModel.LoadModel(whiteModelFilePath);
//            _blackModel = BaseModel.LoadModel(blackModelFilePath);
//        }

//        public void ForwardPass(EvaluatedMove move)
//        {
//            var modelInput = move.Move.NewPosition;

//            if(move.Move.Player == Player.White)
//            {
//                var eval = _whiteModel.Predict(modelInput);
//                move.Evaluation = 0f;
//            }
//            else
//            {
//                var eval = _blackModel.Predict(modelInput);
//                move.Evaluation = 0f;
//            }
//        }
//    }
//}

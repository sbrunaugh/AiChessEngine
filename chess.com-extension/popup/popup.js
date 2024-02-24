const sendMessageId = document.getElementById("sendmessageid");
if (sendMessageId) {
    sendMessageId.onclick = function() {
        chrome.tabs.query({ active: true, currentWindow: true }, function(tabs) {
            chrome.scripting.executeScript({
                target: { tabId: tabs[0].id },
                function: function() {
                    var board = document.getElementById("board-analysis-board");
                    var pieces = [];

                    for (var i = 0; i < board.children.length; i++) {
                        var piece = board.children[i].className;
                        if(typeof(piece) !== 'string') {
                            continue;
                        }
                        
                        if(piece.split(" ")[0] != "piece") {
                            continue;
                        }

                        pieces.push(piece);
                    }

                    var message = "";

                    for (var row = 1; row <= 8; row++) {
                        for (var col = 1; col <= 8; col++) {
                            var nextPieceStr = "";

                            for(var i = 0; i < pieces.length; i++) {
                                var classSplit = pieces[i].split(" ");
                                if(classSplit[2] == `square-${col}${row}`) {
                                    nextPieceStr = " " + classSplit[1] + ",";
                                    break;
                                }
                            }

                            if(nextPieceStr == "") {
                                nextPieceStr = " 0,"
                            }

                            message = message + nextPieceStr;
                        }
                    }

                    message = message.replace(/bb/g, "b3");
                    message = message.replace(/wb/g, "w3");
                    message = message.replace(/w/g, "",);
                    message = message.replace(/b/g, "-");
                    message = message.replace(/p/g, "1");
                    message = message.replace(/r/g, "5");
                    message = message.replace(/n/g, "2");
                    message = message.replace(/k/g, "9");
                    message = message.replace(/q/g, "8");
                    
                    var final = message.substring(0, message.length - 1);

                    final = './DecisionEngine.exe "' + final;
                    final = final + '" 0 1';

                    console.log(final);
                }
            });
        });
    };
}
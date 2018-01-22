    var express = require('express');
var app = express();
var path = require('path');
var bodyParser = require('body-parser');
var mongodb = require('mongodb').MongoClient;
var assert = require('assert');
var url = 'mongodb://localhost:27017/';
var Player = require('./Modules/Player.js');
var splitting = require('./Modules/splitting.js');
var resStatus = 200;
var resMessage = "";
var dbName = "PongDatabase";
var tableNamePlayers = "Players";
var tableNameKI = "KIVSP";
app.use(bodyParser.urlencoded({
    extended: false
}));
app.use(bodyParser.json());
app.use(express.static(path.join(__dirname, 'pages')));
app.use(express.static(path.join(__dirname, 'styles')));
app.use(express.static(path.join(__dirname, 'images')));
app.use(express.static(path.join(__dirname, 'jsfiles')));
app.get('/', function (req, res) {
    res.send("Hello World");
    res.end(200);
});
app.post('/addscore', function (req, res) {
    try {
        if (req.body != undefined) {
            var usernames = req.body.Usernames;
            var scores = req.body.Scores;
            var results = req.body.Results;
            checkIfPlayerExists(usernames[0], tableNamePlayers);
            checkIfPlayerExists(usernames[1], tableNamePlayers);
            addPlayerScore(usernames[0], scores[0]);
            addPlayerScore(usernames[1], scores[1]);
            setWinOrLossForPlayer(usernames[0], results[0]);
            setWinOrLossForPlayer(usernames[1], results[1]);
            resStatus = 200;
            resMessage = "Everything went fine";
        }
        else {
            resStatus = 404;
            resMessage = "No information in body recieved.";
        }
    }
    catch (err) {
        console.error("Error occured: " + err);
    }
    res.send(resMessage);
    res.end(resStatus);
});

app.post('/addkiscore', function (req, res) {
    try {
        if (req.body != undefined) {
            var usernames = req.body.Username;
            var times = req.body.Time;
            checkIfPlayerExists(usernames[0], tableNameKI);
            checkIfPlayerExists(usernames[1], tableNameKI);
            setKITime(usernames[0], times[0]);
            setKITime(usernames[1], times[1]);
            resStatus = 200;
            resMessage = "Everything went fine";
        }
        else {
            resStatus = 404;
            resMessage = "No information in body recieved.";
        }
    }
    catch (err) {
        console.error("Error occured: " + err);
    }
    res.send(resMessage);
    res.end(resStatus);
});
app.get('/leaderboard', function (req, res) {
    try {
        res.setHeader('Content-Type', 'application/json');
        var queryPart = splitting.splitLeaderboardQuery(res, req.originalUrl);
        if (queryPart != null) {
            switch (queryPart) {
                case 'pvsp':
                    getLeaderboardRecords(res, "playervsplayer");
                    break;
                case 'aivsp':
                    getLeaderboardRecords(res, "kivsplayer");
                    break;
            }
        }
        else {
            res.send("Invalid request query");
            res.end(404);
        }
    }
    catch (err) {
        console.error(err);
    }
});
app.listen(3000, function () {
    console.log('Example app listening on port 3000!');
});
/* Some database functions */
var insertPlayer = function (player) {
    mongodb.connect(url, function (err, client) {
        if (err)
            throw err;
        var db = client.db(dbName);
        db.collection(tableNamePlayers).insert(player, function (insertErr, result) {
            if (insertErr)
                throw insertErr;
        });
        client.close();
    });
};
var addPlayerScore = function (username, scoreToIncrease) {
    mongodb.connect(url, function (err, client) {
        if (err)
            throw err;
        var db = client.db(dbName);
        db.collection(tableNamePlayers).update({ username: username }, {
            $inc: { score: scoreToIncrease }
        });
        client.close();
    });
};

var setKITime = function (username, _time) {
    mongodb.connect(url, function (err, client) {
        if (err)
            throw err;
        var db = client.db(dbName);
        db.collection(tableNameKI).update({ username: username }, {
            $set: { time: _time }
        });
        client.close();
    });
};

var getLeaderboardRecords = function (res, category) {
    mongodb.connect(url, function (err, client) {
        if (err)
            throw err;
        resStatus = 200;
        var db = client.db(dbName);
        switch (category) {
            case "playervsplayer":
                db.collection(tableNamePlayers).find({}).sort({ wins: -1 }).toArray(function (err, allPlayers) {
                    res.status(resStatus).json(allPlayers);
                    res.end();
                });
                break;
            case "kivsplayer":
                db.collection(tableNameKI).find({}).sort({ time: -1 }).toArray(function (err, allPlayers) {
                    res.status(resStatus).json(allPlayers);
                    res.end();
                });
                break;
        }
        client.close();
    });
};
var checkIfPlayerExists = function (username, tableName) {
    mongodb.connect(url, function (err, client) {
        if (err)
            throw err;
        var db = client.db(dbName);
        var playerExists = db.collection(tableName).findOne({ username: username }, function (err, user) {
            if (err) {
                throw err;
            }
            if (user) {
            }
            else {
                insertPlayer(new Player(username));
            }
        });
        client.close();
    });
};
var setWinOrLossForPlayer = function (username, result) {
    mongodb.connect(url, function (err, client) {
        if (err)
            throw err;
        var db = client.db(dbName);
        switch (result) {
            case 0:
                db.collection(tableNamePlayers).update({ username: username }, {
                    $inc: {
                        losses: 1
                    }
                });
                client.close();
                break;
            case 1:
                db.collection(tableNamePlayers).update({ username: username }, {
                    $inc: {
                        wins: 1
                    }
                });
                client.close();
                break;
        }
    });
};
//# sourceMappingURL=server.js.map
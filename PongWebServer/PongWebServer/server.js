var express = require('express');
var app = express();
var bodyParser = require('body-parser');
var mongodb = require('mongodb').MongoClient;
var assert = require('assert');
var url = 'mongodb://localhost:27017/PongDatabase';
var Player = require('./Modules/Player.js');
var splitting = require('./Modules/splitting.js');
var exists;
var resStatus = 200;
var resMessage = "";
app.use(bodyParser.urlencoded({
    extended: false
}));
app.use(bodyParser.json());
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
            checkIfPlayerExists(usernames[0]);
            checkIfPlayerExists(usernames[1]);
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
app.get('/leaderboard', function (req, res) {
    try {
        res.setHeader('Content-Type', 'application/json');
        var queryPart = splitting.splitLeaderboardQuery(res, req.originalUrl);
        if (queryPart != null) {
            switch (queryPart) {
                case 'mg':
                    getLeaderboardRecords(res, "score");
                    break;
                case 'mw':
                    getLeaderboardRecords(res, "wins");
                    break;
                case 'ml':
                    getLeaderboardRecords(res, "losses");
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
        var db = client.db('PongDatabase');
        db.collection('Players').insert(player, function (insertErr, result) {
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
        var db = client.db('PongDatabase');
        console.log(username + " in with " + scoreToIncrease + " points");
        db.collection('Players').update({ username: username }, {
            $inc: { score: scoreToIncrease }
        });
        client.close();
    });
};
var getLeaderboardRecords = function (res, category) {
    mongodb.connect(url, function (err, client) {
        if (err)
            throw err;
        var db = client.db('PongDatabase');
        switch (category) {
            case "wins":
                db.collection('Players').find({}).sort({ wins: -1 }).toArray(function (err, allPlayers) {
                    res.status(200).json(allPlayers);
                    res.end();
                });
                break;
            case "losses":
                db.collection('Players').find({}).sort({ losses: -1 }).toArray(function (err, allPlayers) {
                    res.status(200).json(allPlayers);
                    res.end();
                });
                break;
            case "score":
                db.collection('Players').find({}).sort({ score: -1 }).toArray(function (err, allPlayers) {
                    res.status(200).json(allPlayers);
                    res.end();
                });
                break;
        }
        client.close();
    });
};
var checkIfPlayerExists = function (username) {
    mongodb.connect(url, function (err, client) {
        if (err)
            throw err;
        var db = client.db('PongDatabase');
        var playerExists = db.collection('Players').findOne({ username: username }, function (err, user) {
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
        var db = client.db('PongDatabase');
        switch (result) {
            case 0:
                db.collection('Players').update({ username: username }, {
                    $inc: {
                        losses: 1
                    }
                });
                break;
            case 1:
                db.collection('Players').update({ username: username }, {
                    $inc: {
                        wins: 1
                    }
                });
                break;
        }
        client.close();
    });
};
//# sourceMappingURL=server.js.map
var username;
var score;
var wins;
var losses;

function Player(username) {
    this.username = username;
    this.score = 0;
    this.wins = 0;
    this.losses = 0;
}

Player.prototype.addScore = function (score) {
    this.score += score;
};

Player.prototype.addWins = function () {
    this.wins++;
};

Player.prototype.addLosses = function () {
    this.losses++;
};

Player.prototype.getUsername = function () {
    return this.username;
};

Player.prototype.getScore = function () {
    return parseInt(this.score);
};

Player.prototype.getWins = function () {
    return this.wins;
};

Player.prototype.getLosses = function () {
    return this.losses;
};

Player.prototype.returnJSON = function () {
    return '{ username: "' + this.getUsername() + '", score: ' + this.getScore() 
            + ', wins: ' + this.getWins() + ', losses: ' + this.getLosses() + '}';
};

module.exports = Player;
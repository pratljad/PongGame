module.exports = {
    splitQuery: function (res, query) {
        var result = null;
        if (query.indexOf("?") != -1) {
            result = query.split('?')[1].split('=');
        }

        return result;
    },

    checkIfUserExists: function (players, username) {
        var user = null;
        players.forEach(function (element) {
            if (element.getUsername() == username.toString()) {
                console.log("im in score");
                user = element;
            }
        });

        return user;
    },

    setWinOrLossForPlayer: function (player, result) {
        switch (result) {
            case 0:
                player.addLosses();
                break;

            case 1:
                player.addWins();
                break;
        }
    }
};
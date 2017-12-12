module.exports = {
    splitLeaderboardQuery: function (res, query) {
        var result = null;
        if (query.indexOf("?") != -1) {
            result = query.split('?')[1].split('=')[1];
        }

        return result;
    },

    getMostLosses: function (players) {
        players.sort(function (firstElement, secondElement) {
            var keyA = firstElement.losses,
                keyB = secondElement.losses;

            if (keyA > keyB) return -1;
            if (keyA < keyB) return 1;
            return 0;
        });

        return players;
    },

    getMostWins: function (players) {
        players.sort(function (firstElement, secondElement) {
            var keyA = firstElement.wins,
                keyB = secondElement.wins;

            if (keyA > keyB) return -1;
            if (keyA < keyB) return 1;
            return 0;
        });

        return players;
    },

    getMostGoals: function (players) {
        players.sort(function (firstElement, secondElement) {
            var keyA = firstElement.score,
                keyB = secondElement.score;

            if (keyA > keyB) return -1;
            if (keyA < keyB) return 1;
            return 0;
        });

        return players;
    }
};
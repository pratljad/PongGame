module.exports = {
    splitLeaderboardQuery: function (res, query) {
        var result = null;
        if (query.indexOf("?") != -1) {
            result = query.split('?')[1].split('=')[1];
        }

        return result;
    }
};
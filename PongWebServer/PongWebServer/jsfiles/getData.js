createInterval(getCurrentLeaderboardStatus, getCategory(), 5000);
getCurrentLeaderboardStatus(getCategory());
function getCurrentLeaderboardStatus(category) {
    switch(category) {
        case 'index':
            category = 'pvsp';
            break;
        case 'leaderboardKI':
            category = 'aivsp';
            break;
    }
    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        url: 'http://localhost:3000/leaderboard?req='+category,						
        success: function(data) {
            console.log(category);
            console.log(data);
            renewTableStructureWithData(data, category);
        },
        error: function() {
            console.log("failed");
        }
    });
}

function createInterval(fnc,category,interval) { 
    setInterval(function() 
    { 
        fnc(category); 
    }, interval); 
} 

function getCategory() {
    return window.location.href.split('/')[3].split('.')[0];;
}

function renewTableStructureWithData(data, category) {
    var tableToAppend = '<table class="points_table"><thead><tr>';
    console.log("category: " + category);
    switch (category) {
        case 'pvsp':
            tableToAppend += '<th class="col-md-2">Rank</th><th class="col-md-4">User</th><th class="col-md-2">Wins</th><th class="col-md-2">Losses</th><th class="col-md-2">Total Goals</th></tr></thead><tbody class="points_table_scrollbar">';
            tableToAppend += renewTablesForPvsP(data);
            console.log(tableToAppend);
            $('.points_table').empty();
            $('.points_table').append(tableToAppend);
            break;
        case 'aivsp':
            tableToAppend += '<th class="col-md-2">Rank</th><th class="col-md-4">User</th><th class="col-md-2">Wins</th><th class="col-md-2">Losses</th><th class="col-md-2">Time Played</th></tr></thead><tbody class="points_table_scrollbar">';
            tableToAppend += renewTablesForPvsAI(data);
            console.log(tableToAppend);
            $('.points_table').empty();
            $('.points_table').append(tableToAppend);
            break;
    }
}

function renewTablesForPvsP(data) {
    var returnString = "";
    for (var i = 0; i < data.length; i++) {
        if (i % 2 == 0) {
            returnString += '<tr class="even"><td class="col-md-2">' + (i+1) + '</td><td class="col-md-4">'+ data[i].username + '</td><td class="col-md-2">' + data[i].wins + '</td><td class="col-md-2">' + data[i].losses + '</td><td class="col-md-2">' + data[i].score + '</td></tr>';
        }

        else {
            returnString += '<tr class="odd"><td class="col-md-2">' + (i + 1) + '</td><td class="col-md-4">' + data[i].username + '</td><td class="col-md-2">' + data[i].wins + '</td><td class="col-md-2">' + data[i].losses + '</td><td class="col-md-2">' + data[i].score + '</td></tr>';
        }
    }
    returnString += '</tbody></table>';

    return returnString;
}

function renewTablesForPvsAI(data) {
    var returnString = "";
    for (var i = 0; i < data.length; i++) {
        if (i % 2 == 0) {
            returnString += '<tr class="even"><td class="col-md-2">' + (i + 1) + '</td><td class="col-md-4">' + data[i].username + '</td><td class="col-md-2">' + data[i].wins + '</td><td class="col-md-2">' + data[i].losses + '</td><td class="col-md-2">' + data[i].time + '</td></tr>';
        }

        else {
            returnString += '<tr class="odd"><td class="col-md-2">' + (i + 1) + '</td><td class="col-md-4">' + data[i].username + '</td><td class="col-md-2">' + data[i].wins + '</td><td class="col-md-2">' + data[i].losses + '</td><td class="col-md-2">' + data[i].time + '</td></tr>';
        }
    }
    returnString += '</tbody></table>';

    return returnString;
}
createInterval(getCurrentLeaderboardStatus, getCategory(), 5000);
console.log(getCategory().split('/')[3]);
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
    return getCategory().split('/')[3].split('.')[0];;
}

function renewTableStructureWithData(data, category) {
    var tableToAppend = '<table class="points_table"><thead><tr>';
    switch(category) {
        case 'index':
            tableToAppend += '<th class="col-md-2">Rank</th><th class="col-md-4">User</th><th class="col-md-2">Wins</th><th class="col-md-2">Losses</th><th class="col-md-2">Total Goals</th></tr></thead><tbody class="points_table_scrollbar">';
            break;
        case 'leaderboardKI':
            tableToAppend += '<th class="col-md-4">Rank</th><th class="col-md-4">User</th><th class="col-md-4">Time Played</th></tr></thead><tbody class="points_table_scrollbar">';
            break;
    }
}
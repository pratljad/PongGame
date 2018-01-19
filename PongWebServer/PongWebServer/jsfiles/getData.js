createInterval(getCurrentLeaderboardStatus, getCategory(), 5000);

function getCurrentLeaderboardStatus(category) {
    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        url: 'http://localhost:3000/leaderboard?req='+category,						
        success: function() {
            console.log('success');
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
    //url holen und dann splitten um zu sehen auf welchem
    //leaderboard er ist
    return "";
}
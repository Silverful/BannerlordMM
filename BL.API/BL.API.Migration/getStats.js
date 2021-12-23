/** @OnlyCurrentDoc */

function getStats() {
    var stats = JSON.parse(UrlFetchApp.fetch("https://bannerlordmm.com/api/stats").getContentText());

    const playerStats = stats.playerStats;
    const iglStats = stats.iglStats;
    const divisionStats = stats.divisionStats;
    const factionStats = stats.factionStats;
    const topPlayerByClassStats = stats.topPlayerByClassStats;
    const playersByFactionStats = stats.playersByFactionStats;

    const mainSpread = SpreadsheetApp.getActiveSpreadsheet().getSheets();

    const spreadsheet = mainSpread[0];
    const pfSheet = mainSpread[2];

    let cell = parseInt(1);
    let row = 0;

    playerStats.forEach((obj, i) => {
        row = parseInt(i + 2);
        cell = parseInt(1);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(i + 1);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.nickname);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.country);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.igl);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.clan);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.mainClass);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.secondaryClass);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.mmr);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.played);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.wins);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.wr);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.rounds);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.kills);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.kr);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.assists);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.ar);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.kar);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.score);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.sr);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.mvp);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.mvpr);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.discordId);
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell++).setValue(obj.rank);
        cell = parseInt(1);
    });

    cell = 22;

    row = 19;
    cell += 2;
    const archers = topPlayerByClassStats.archer;
    Object.keys(archers).forEach((obj, i) => {
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell).setValue(i + 1);
        spreadsheet.getRange(row, cell + 1).setValue('');
        spreadsheet.getRange(row, cell + 1).setValue(obj);
        spreadsheet.getRange(row, cell + 2).setValue('');
        spreadsheet.getRange(row, cell + 2).setValue(archers[obj]);

        row++;
    })

    row = 19;
    cell += 3;

    const infs = topPlayerByClassStats.infantry;
    Object.keys(infs).forEach((obj, i) => {
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell).setValue(obj);
        spreadsheet.getRange(row, cell + 1).setValue('');
        spreadsheet.getRange(row, cell + 1).setValue(infs[obj]);

        row++;
    })

    row = 19;
    cell += 2;

    const cavalry = topPlayerByClassStats.cavalry;
    Object.keys(cavalry).forEach((obj, i) => {
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell).setValue(obj);
        spreadsheet.getRange(row, cell + 1).setValue('');
        spreadsheet.getRange(row, cell + 1).setValue(cavalry[obj]);

        row++;
    })

    const lastStatsRow = row += 3;
    row += 2;
    cell -= 5;

    Object.keys(iglStats).forEach((obj, i) => {
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell).setValue(i + 1);
        spreadsheet.getRange(row, cell + 1).setValue('');
        spreadsheet.getRange(row, cell + 1).setValue(obj);
        spreadsheet.getRange(row, cell + 2).setValue('');
        spreadsheet.getRange(row, cell + 2).setValue(iglStats[obj]);

        row++;
    })

    cell += 3;
    row = lastStatsRow + 2;

    Object.keys(divisionStats).forEach((obj, i) => {
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell).setValue(obj);
        spreadsheet.getRange(row, cell + 1).setValue('');
        spreadsheet.getRange(row, cell + 1).setValue(divisionStats[obj]);

        row++;
    })

    cell += 2;
    row = lastStatsRow + 2;

    Object.keys(factionStats).forEach((obj, i) => {
        spreadsheet.getRange(row, cell).setValue('');
        spreadsheet.getRange(row, cell).setValue(obj);
        spreadsheet.getRange(row, cell + 1).setValue('');
        spreadsheet.getRange(row, cell + 1).setValue(factionStats[obj]);

        row++;
    })

    playersByFactionStats.forEach((obj, i) => {
        row = parseInt(i + 2);
        cell = 1;

        pfSheet.getRange(row, cell).setValue('');
        pfSheet.getRange(row, cell++).setValue(obj.nickname);
        pfSheet.getRange(row, cell).setValue('');
        pfSheet.getRange(row, cell++).setValue(obj.aseraiCount);
        pfSheet.getRange(row, cell).setValue('');
        pfSheet.getRange(row, cell++).setValue(obj.aseraiWR);
        pfSheet.getRange(row, cell).setValue('');
        pfSheet.getRange(row, cell++).setValue(obj.battaniaCount);
        pfSheet.getRange(row, cell).setValue('');
        pfSheet.getRange(row, cell++).setValue(obj.battaniaWR);
        pfSheet.getRange(row, cell).setValue('');
        pfSheet.getRange(row, cell++).setValue(obj.empireCount);
        pfSheet.getRange(row, cell).setValue('');
        pfSheet.getRange(row, cell++).setValue(obj.empireWR);
        pfSheet.getRange(row, cell).setValue('');
        pfSheet.getRange(row, cell++).setValue(obj.khuzaitCount);
        pfSheet.getRange(row, cell).setValue('');
        pfSheet.getRange(row, cell++).setValue(obj.khuzaitWR);
        pfSheet.getRange(row, cell).setValue('');
        pfSheet.getRange(row, cell++).setValue(obj.vlandiaCount);
        pfSheet.getRange(row, cell).setValue('');
        pfSheet.getRange(row, cell++).setValue(obj.vlandiaWR);
    })
}

function timeoutstats() {
    ScriptApp.newTrigger('getStats')
        .timeBased()
        .everyMinutes(3)
        .create();
};
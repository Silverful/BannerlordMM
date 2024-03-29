﻿/** @OnlyCurrentDoc */

function getStats() {
    var stats = JSON.parse(UrlFetchApp.fetch("https://bannerlordmm.com/api/stats").getContentText());

    const mainSpread = SpreadsheetApp.getActiveSpreadsheet().getSheets();

    const spreadsheet = mainSpread.find(x => x.getName() == 'Stats');
    const selectedStatsSheet = mainSpread.find(x => x.getName() == 'MainStats');
    const pfSheet = mainSpread.find(x => x.getName() == 'PF');
    
    setMainStats(spreadsheet, stats);

    let cell = 28;
    let row = 32;

    setAdditionalTableStats(spreadsheet, cell, row, stats);

    setPF(pfSheet, stats);

    setMainCutStats(selectedStatsSheet, stats);

    cell = 14;
    row = 32;

    setAdditionalTableStats(selectedStatsSheet, cell, row, stats);
}

function setMainCutStats(spreadsheet, stats){
  const playerStats = stats.playerStats;

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
    spreadsheet.getRange(row, cell++).setValue(obj.clan);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.mainClass);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.rank);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.mmr);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.played);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.wins);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.wr);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.kda);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.sr);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.mvpr);
    spreadsheet.getRange(row, cell).setValue('');
    
    cell = parseInt(1);
  });
}

function setMainStats(spreadsheet, stats){
  const playerStats = stats.playerStats;

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
      spreadsheet.getRange(row, cell++).setValue(obj.rank);
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
      spreadsheet.getRange(row, cell++).setValue(obj.deaths);
      spreadsheet.getRange(row, cell).setValue('');
      spreadsheet.getRange(row, cell++).setValue(obj.dr);
      spreadsheet.getRange(row, cell).setValue('');
      spreadsheet.getRange(row, cell++).setValue(obj.kd);
      spreadsheet.getRange(row, cell).setValue('');
      spreadsheet.getRange(row, cell++).setValue(obj.assists);
      spreadsheet.getRange(row, cell).setValue('');
      spreadsheet.getRange(row, cell++).setValue(obj.ar);
      spreadsheet.getRange(row, cell).setValue('');
      spreadsheet.getRange(row, cell++).setValue(obj.kar);
      spreadsheet.getRange(row, cell).setValue('');
      spreadsheet.getRange(row, cell++).setValue(obj.kda);
      spreadsheet.getRange(row, cell).setValue('');
      spreadsheet.getRange(row, cell++).setValue(obj.score);
      spreadsheet.getRange(row, cell).setValue('');
      spreadsheet.getRange(row, cell++).setValue(obj.sr);
      spreadsheet.getRange(row, cell).setValue('');
      spreadsheet.getRange(row, cell++).setValue(obj.mvp);
      spreadsheet.getRange(row, cell).setValue('');
      spreadsheet.getRange(row, cell++).setValue(obj.mvpr);
      spreadsheet.getRange(row, cell).setValue('');
      
      cell = parseInt(1);
  });
}

function setAdditionalTableStats(spreadsheet, startCell, startRow, stats){
  const iglStats = stats.iglStats;
  const divisionStats = stats.divisionStats;
  const factionStats = stats.factionStats;
  const topPlayerByClassStats = stats.topPlayerByClassStats;

  let row = startRow;
  let cell = startCell;

  const rowsTotal = 10;
  const archers = Object.keys(topPlayerByClassStats.archer);
  const infs = Object.keys(topPlayerByClassStats.infantry);
  const cavalry = Object.keys(topPlayerByClassStats.cavalry);

  for (var i = 0; i < rowsTotal; i++){
    let arch = archers[i];
    let inf = infs[i];
    let cav = cavalry[i];

    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell).setValue(i + 1);

    spreadsheet.getRange(row, cell + 1).setValue('');
    spreadsheet.getRange(row, cell + 2).setValue('');
    if (arch){
      spreadsheet.getRange(row, cell + 1).setValue(arch);
      spreadsheet.getRange(row, cell+ 2).setValue(topPlayerByClassStats.archer[arch]);
    } 

    spreadsheet.getRange(row, cell + 3).setValue('');
    spreadsheet.getRange(row, cell + 4).setValue('');
    if (inf){
      spreadsheet.getRange(row, cell + 3).setValue(inf);
      spreadsheet.getRange(row, cell + 4).setValue(topPlayerByClassStats.infantry[inf]);
    }

    spreadsheet.getRange(row, cell + 5).setValue('');
    spreadsheet.getRange(row, cell + 6).setValue('');
    if (cav){
      spreadsheet.getRange(row, cell + 5).setValue(cav);
      spreadsheet.getRange(row, cell + 6).setValue(topPlayerByClassStats.cavalry[cav]);
    }
    row++;
  }

  row += 2;

  const iglStatsIndices = 10;

  for (var i = 0; i < iglStatsIndices; i++){
    spreadsheet.getRange(row + i, cell).setValue('');
    spreadsheet.getRange(row + i, cell).setValue(i + 1);
  }

  Object.keys(iglStats).forEach((obj, i) => {
      spreadsheet.getRange(row + i, cell + 1).setValue('');
      spreadsheet.getRange(row + i, cell + 1).setValue(obj);
      spreadsheet.getRange(row + i, cell + 2).setValue('');
      spreadsheet.getRange(row + i, cell + 2).setValue(iglStats[obj]);
  })

  cell += 3;

  Object.keys(divisionStats).forEach((obj, i) => {
      spreadsheet.getRange(row + i, cell).setValue('');
      spreadsheet.getRange(row + i, cell).setValue(obj);
      spreadsheet.getRange(row + i, cell + 1).setValue('');
      spreadsheet.getRange(row + i, cell + 1).setValue(divisionStats[obj]);
  })

  cell += 2;

  Object.keys(factionStats).forEach((obj, i) => {
      spreadsheet.getRange(row + i, cell).setValue('');
      spreadsheet.getRange(row + i, cell).setValue(obj);
      spreadsheet.getRange(row + i, cell + 1).setValue('');
      spreadsheet.getRange(row + i, cell + 1).setValue(factionStats[obj]);
  })
}

function setPF(spreadsheet, stats){
  const playersByFactionStats = stats.playersByFactionStats;
  spreadsheet.deleteRows(2, spreadsheet.getLastRow() - 1);

  playersByFactionStats.forEach((obj, i) => {
    let row = parseInt(i + 2);
    let cell = 1;

    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.nickname);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.aseraiCount);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.aseraiWR);

    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.battaniaCount);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.battaniaWR);

    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.empireCount);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.empireWR);

    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.khuzaitCount);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.khuzaitWR);

    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.sturgiaCount);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.sturgiaWR);

    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.vlandiaCount);
    spreadsheet.getRange(row, cell).setValue('');
    spreadsheet.getRange(row, cell++).setValue(obj.vlandiaWR);
  })
}
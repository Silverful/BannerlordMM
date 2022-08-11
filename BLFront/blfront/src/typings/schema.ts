export interface MainStatsItem {
    playerId: string;
    position: number;
    nickname: string;
    country: string;
    IGL: string;
    clan: string;
    mainClass: string;
    secondaryClass: string;
    rank: string;
    mmr: number;
    played: number;
    wins: number;
    wr: number;
    rounds: number;
    kills: number;
    kr: number;
    assists: number;
    ar: number;
    kar: number;
    deaths: number;
    kd: number;
    kda: number;
    dr: number;
    score: number;
    sr: number;
    mvp: number;
    mvpr: number;
    discordId: number;
}


export interface PlayerByFactionWRItem {
    nickname: string;
    aseraiCount: number;
    aseraiWR: number;
    battaniaCount: number;
    battaniaWR: number;
    empireCount: number;
    empireWR: number;
    khuzaitCount: number;
    khuzaitWR: number;
    sturgiaCount: number;
    sturgiaWR: number;
    vlandiaCount: number;
    vlandiaWR: number;
}
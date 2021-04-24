namespace GEMC.Common
{
    public enum TimingStatus
    {
        NoData = 0,

        BetweenRaces = 1,

        RaceRunning = 2,
        
        RaceEnding = 3
    }

    public enum DisplayStatus
    {
        NothingDisplayed = 0,

        ResultGridDisplayed = 1,

        RaceGridDisplayed  = 2,

        FullInfoDisplayed = 3
    }


    //{
    //NoData = 0,

    //// No planned execution
    //StandBy = 1,

    //// There is a planned race, it will come soon (>3min)
    //NextRaceSchedule = 2,

    //// There is a planned race, it will come soon (30s<X<3min)
    //RacePreparation = 3,

    //// There is race running time (from start time - 30s until endtime + 10s 
    //RaceRunning = 4,

    //// there is no race anymore, 10s seconds after endtime
    //RaceEnded = 5,

    //// 
    //RaceEnding = 6
    //}
}
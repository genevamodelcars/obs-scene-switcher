namespace GEMC.Common
{
    using System;
    using System.Collections.Generic;

    public class Configuration
    {
        public Configuration(string webServicePort, string liveDataServerPort, string rcmWebSocketUrl, string obsWebSocketUrl, string obsWebSocketPassword, string nextRaceTableShowTime, string raceResultsTableShowTime, string nextRaceTableDelay, List<SceneInfo> sceneInfos)
        {
            WebServicePort = webServicePort ?? throw new ArgumentNullException(nameof(webServicePort));
            LiveDataServerPort = liveDataServerPort ?? throw new ArgumentNullException(nameof(liveDataServerPort));
            RcmWebSocketUrl = rcmWebSocketUrl ?? throw new ArgumentNullException(nameof(rcmWebSocketUrl));
            ObsWebSocketUrl = obsWebSocketUrl ?? throw new ArgumentNullException(nameof(obsWebSocketUrl));
            ObsWebSocketPassword = obsWebSocketPassword ?? throw new ArgumentNullException(nameof(obsWebSocketPassword));
            NextRaceTableShowTime = nextRaceTableShowTime ?? throw new ArgumentNullException(nameof(nextRaceTableShowTime));
            RaceResultsTableShowTime = raceResultsTableShowTime ?? throw new ArgumentNullException(nameof(raceResultsTableShowTime));
            NextRaceTableDelay = nextRaceTableDelay ?? throw new ArgumentNullException(nameof(nextRaceTableDelay));
            SceneInfos = sceneInfos ?? throw new ArgumentNullException(nameof(sceneInfos));
        }

        public string WebServicePort { get; set; }

        public string LiveDataServerPort { get; set; }

        public string RcmWebSocketUrl { get; set; }

        public string ObsWebSocketUrl { get; set; }

        public string ObsWebSocketPassword { get; set; }

        public string NextRaceTableShowTime { get; set; }

        public string RaceResultsTableShowTime { get; set; }

        public string NextRaceTableDelay { get; set; }

        public List<SceneInfo> SceneInfos { get; set; }
    }
}
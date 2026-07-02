using System;
using System.Collections.Generic;

namespace HospitalRescue.Domain.Entities
{
    [Serializable]
    public class MissionData
    {
        public int missionId;
        public string missionName;
        public string description;
        public bool isCompleted;
        public float completionTime;
        public int scoreEarned;
        public List<string> objectivesCompleted;
        
        public MissionData()
        {
            objectivesCompleted = new List<string>();
        }
    }
}

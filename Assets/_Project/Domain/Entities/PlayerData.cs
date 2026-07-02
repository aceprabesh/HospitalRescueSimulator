using System;

namespace HospitalRescue.Domain.Entities
{
    [Serializable]
    public class PlayerData
    {
        public string playerName;
        public int health;
        public int maxHealth;
        public float[] position;
        public float[] rotation;
        public int currentMission;
        public float missionTime;
        public int score;
        public int livesRemaining;
        
        public PlayerData()
        {
            playerName = "Player";
            health = 100;
            maxHealth = 100;
            position = new float[3];
            rotation = new float[3];
            currentMission = 1;
            missionTime = 0f;
            score = 0;
            livesRemaining = 3;
        }
    }
}

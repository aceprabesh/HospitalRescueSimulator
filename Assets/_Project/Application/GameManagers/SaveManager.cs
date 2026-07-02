using UnityEngine;
using HospitalRescue.Domain.Entities;
using HospitalRescue.Domain.Interfaces;
using System.IO;
using System.Text.Json;

namespace HospitalRescue.Application.GameManagers
{
    /// <summary>
    /// Handles game save/load operations using JSON serialization
    /// </summary>
    public class SaveManager : MonoBehaviour, ISaveManager
    {
        private const string SAVE_FOLDER = "Saves";
        private const string PLAYER_DATA_FILE = "player.json";
        private const string MISSION_DATA_FILE = "missions.json";
        private const string SETTINGS_FILE = "settings.json";
        
        private string saveDirectory;
        
        public string[] GetAllSaveSlots()
        {
            if (!Directory.Exists(saveDirectory))
                return new string[0];
            
            return Directory.GetDirectories(saveDirectory);
        }
        
        public bool SaveExists(string saveSlot)
        {
            string path = GetSavePath(saveSlot);
            return Directory.Exists(path);
        }
        
        public void Save(string saveSlot)
        {
            string path = GetSavePath(saveSlot);
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            // Save player data
            string playerJson = JsonUtility.ToJson(GetPlayerData());
            File.WriteAllText(Path.Combine(path, PLAYER_DATA_FILE), playerJson);
            
            Debug.Log($"Game saved to: {path}");
        }
        
        public void Load(string saveSlot)
        {
            string path = GetSavePath(saveSlot);
            string playerFile = Path.Combine(path, PLAYER_DATA_FILE);
            
            if (File.Exists(playerFile))
            {
                string json = File.ReadAllText(playerFile);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                ApplyPlayerData(data);
                Debug.Log($"Game loaded from: {path}");
            }
            else
            {
                Debug.LogWarning($"No save found at: {path}");
            }
        }
        
        public void Delete(string saveSlot)
        {
            string path = GetSavePath(saveSlot);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                Debug.Log($"Save deleted: {path}");
            }
        }
        
        private string GetSavePath(string saveSlot)
        {
            if (string.IsNullOrEmpty(saveDirectory))
            {
                saveDirectory = Path.Combine(Application.persistentDataPath, SAVE_FOLDER);
            }
            return Path.Combine(saveDirectory, saveSlot);
        }
        
        private PlayerData GetPlayerData()
        {
            PlayerData data = new PlayerData();
            
            // Get player position
            if (GameManager.Instance != null)
            {
                // Find player object
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    data.position = new float[] 
                    { 
                        player.transform.position.x,
                        player.transform.position.y,
                        player.transform.position.z
                    };
                    data.rotation = new float[]
                    {
                        player.transform.eulerAngles.x,
                        player.transform.eulerAngles.y,
                        player.transform.eulerAngles.z
                    };
                }
            }
            
            return data;
        }
        
        private void ApplyPlayerData(PlayerData data)
        {
            // Apply loaded data to player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && data.position != null && data.position.Length == 3)
            {
                player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            }
        }
    }
}

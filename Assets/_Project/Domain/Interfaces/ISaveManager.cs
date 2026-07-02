namespace HospitalRescue.Domain.Interfaces
{
    /// <summary>
    /// Interface for save/load operations
    /// </summary>
    public interface ISaveManager
    {
        void Save(string saveSlot);
        void Load(string saveSlot);
        void Delete(string saveSlot);
        bool SaveExists(string saveSlot);
        string[] GetAllSaveSlots();
    }
}

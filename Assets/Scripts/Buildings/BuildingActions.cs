[System.Flags]
public enum BuildingActions
{
    None = 0,
    SendWorker = 1,
    RetrieveWorker = 1 << 1,
    RepairBuilding = 1 << 2,
    DestroyBuilding = 1 << 3,
    IncreasePopulation = 1 << 4
}

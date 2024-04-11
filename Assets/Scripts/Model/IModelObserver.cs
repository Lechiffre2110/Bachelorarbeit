public interface IModelObserver
{
    void OnModelUpdated(UpdateType updateType = UpdateType.General);
}

public enum UpdateType
{
    General,
    NearbyLocationsUpdated,
    PersonaUpdated
}
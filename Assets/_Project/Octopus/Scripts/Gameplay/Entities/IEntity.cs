namespace VContainer.Unity
{
    public interface IEntity
    {
        bool IsActive { get; }
        void OnDestroyed();
    }
}
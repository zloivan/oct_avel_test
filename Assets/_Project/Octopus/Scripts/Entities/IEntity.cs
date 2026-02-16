namespace Octopus.Entities
{
    public interface IEntity
    {
        bool IsActive { get; }
        void OnDestroyed();
    }
}
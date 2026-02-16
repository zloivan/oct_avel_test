namespace Octopus.Gameplay.Entities
{
    public interface IEntity
    {
        bool IsActive { get; }
        void OnDestroyed();
    }
}
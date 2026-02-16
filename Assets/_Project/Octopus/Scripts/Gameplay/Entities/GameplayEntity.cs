using Octopus.Services;
using UnityEngine;

namespace Octopus.Gameplay.Entities
{
    public class GameplayEntity : MonoBehaviour, IEntity
    {
        private EntityManager _entityManager;
        private bool _isActive = true;

        public bool IsActive => _isActive && gameObject.activeInHierarchy;

        public void Initialize(EntityManager entityManager)
        {
            _entityManager = entityManager;
            _entityManager.Register(this);
        }

        public void Deactivate()
        {
            _isActive = false;
        }

        public void OnDestroyed()
        {
            _isActive = false;
        }

        private void OnDestroy()
        {
            _entityManager?.Unregister(this);
            OnDestroyed();
        }

        private void OnDisable()
        {
            // No need to unregister—IsActive will return false
        }
    }
}
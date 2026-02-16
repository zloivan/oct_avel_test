using UnityEngine;

namespace VContainer.Unity
{
    public class EntityInitializer : MonoBehaviour
    {
        [Inject] private EntityManager _entityManager;
        [SerializeField] private GameplayEntity[] _entities;

        private void Start() =>
            InitializeEntities();

        [ContextMenu("Manual Init")]
        private void InitializeEntities()
        {
            foreach (var entity in _entities)
            {
                entity.Initialize(_entityManager);
            }

            Debug.Log($"[EntityInitializer] Initialized {_entities.Length} entities");
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace VContainer.Unity
{
    public class EntityManager
    {
        private readonly List<IEntity> _allEntities = new();

        public void Register(IEntity entity)
        {
            if (entity == null)
            {
                Debug.LogWarning("[EntityManager] Attempted to register null entity");
                return;
            }

            if (!_allEntities.Contains(entity))
            {
                _allEntities.Add(entity);
            }
        }


        public void Unregister(IEntity entity)
        {
            _allEntities.Remove(entity);
        }

        public List<IEntity> GetActiveEntities()
        {
            var activeEntities = new List<IEntity>();

            for (var i = _allEntities.Count - 1; i >= 0; i--)
            {
                var entity = _allEntities[i];

                // Remove null references (destroyed without calling Unregister)
                if (entity == null)
                {
                    _allEntities.RemoveAt(i);
                    continue;
                }

                // Add only active entities
                if (entity.IsActive)
                {
                    activeEntities.Add(entity);
                }
            }

            return activeEntities;
        }

        public int GetActiveEntityCount()
        {
            var count = 0;

            for (var i = _allEntities.Count - 1; i >= 0; i--)
            {
                var entity = _allEntities[i];

                if (entity == null)
                {
                    _allEntities.RemoveAt(i);
                    continue;
                }

                if (entity.IsActive)
                {
                    count++;
                }
            }

            return count;
        }


        public void Clear()
        {
            _allEntities.Clear();
        }
    }
}
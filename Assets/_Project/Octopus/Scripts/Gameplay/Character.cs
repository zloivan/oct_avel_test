using UnityEngine;

namespace Octopus.Gameplay
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private float _value = 10f;
        
        public float Value => _value;
    }
}
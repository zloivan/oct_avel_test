using UnityEngine;

namespace _Project.Octopus.Scripts.Gameplay
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private float _value = 10f;
        
        public float Value => _value;
    }
}
using _Project.Octopus.Scripts;
using _Project.Octopus.Scripts.Core;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Octopus.Bootstrap
{
    public class OctopusInstaller : LifetimeScope
    {
        [SerializeField] private TestingScript _testingScript;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SaveSystem>(Lifetime.Singleton).As<ISaveSystem>();
            
            // Регистрируем MonoBehaviour из сцены
            builder.RegisterComponent(_testingScript);
            
            // Services will be registered here as we progress
            Debug.Log("[OctopusInstaller] Initialized");
        }
    }
}
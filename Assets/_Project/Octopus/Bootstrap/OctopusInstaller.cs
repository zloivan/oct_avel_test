using _Project.Octopus.Scripts;
using _Project.Octopus.Scripts.Core;
using _Project.Octopus.Scripts.Gameplay;
using _Project.Octopus.Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Octopus.Bootstrap
{
    public class OctopusInstaller : LifetimeScope
    {
        //TESTING
        [SerializeField] private TestingScript _testingScript;
        
        //LOGIC
        [SerializeField] private PopupViewUI _popupPrefab;
        [SerializeField] private Transform _uiRoot;
        [SerializeField] private CharactersView _charactersView;
        
       
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ISaveSystem>(_ =>
            {
                //var saveSystem = new SaveSystem();
                return new SaveProcessor();
                //Debug Logs
            }, Lifetime.Singleton);

            builder.RegisterComponent(_testingScript);
            
            builder.Register<PopupManager>(Lifetime.Singleton)
                .WithParameter(_popupPrefab)
                .WithParameter(_uiRoot);

            builder.RegisterComponent(_charactersView);
            builder.Register<EntityManager>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<EntityInitializer>();

            Debug.Log("[OctopusInstaller] Initialized");
        }
    }
}
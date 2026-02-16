using Octopus.CharacterView;
using Octopus.Entities;
using Octopus.SaveLoadUtility;
using Octopus.Testing;
using Octopus.UI.Popups;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Octopus.Bootstrap
{
    public class OctopusInstaller : LifetimeScope
    {
        [SerializeField] private PopupViewUI _popupPrefab;
        [SerializeField] private Transform _uiRoot;
        [SerializeField] private CharactersView _charactersView;


        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SaveSystem>(Lifetime.Singleton)
                .WithParameter(false)
                .As<ISaveSystem>();

            builder.RegisterComponentInHierarchy<TestingScript>();

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
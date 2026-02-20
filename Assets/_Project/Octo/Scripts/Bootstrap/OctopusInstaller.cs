using Octo.CharacterView;
using Octo.Entities;
using Octo.SaveLoadUtility;
using Octo.Testing;
using Octo.UI.Popups;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Octo.Bootstrap
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
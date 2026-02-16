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
            builder.Register<ISaveSystem>(_ =>
            {
                //var saveSystem = new SaveSystem();
                return new SaveSystem();
                //Debug Logs
                //return new SavingLogDecorator(saveSystem);
            }, Lifetime.Singleton);

            builder.RegisterComponent(_testingScript);

            Debug.Log("[OctopusInstaller] Initialized");
        }
    }
}
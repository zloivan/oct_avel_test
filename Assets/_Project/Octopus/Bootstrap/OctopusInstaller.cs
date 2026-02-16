using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Octopus.Bootstrap
{
    public class OctopusInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Services will be registered here as we progress
            Debug.Log("[OctopusInstaller] Initialized");
        }
    }
}
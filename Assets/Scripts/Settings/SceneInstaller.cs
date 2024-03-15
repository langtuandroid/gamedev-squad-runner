using Gameplay;
using Gameplay.Road;
using JetSystems;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Settings
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField]
        private RoadManager _roadManager;
        [FormerlySerializedAs("_squadController")] [SerializeField] 
        private SquadControllersr squadControllersr;
        
        public override void InstallBindings()
        {
            Container.Bind<RoadManager>().FromInstance(_roadManager).AsSingle();
            Container.Bind<SquadControllersr>().FromInstance(squadControllersr).AsSingle();
        }
    }
}

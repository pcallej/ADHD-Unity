using Zenject;
using UnityEngine;
using System.Collections;

public class AnimalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameObject>().FromInstance(gameObject);
        Container.Bind<FoxFactory>().AsSingle().NonLazy();
    }
}

public class FoxFactory {
    public FoxFactory(GameObject foxfactory) {
        foxfactory = new GameObject("FoxFactory");
    }
}
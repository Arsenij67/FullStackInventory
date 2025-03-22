using UnityEngine;
using Zenject;

public class DependencyInstaller : MonoInstaller
{
    [SerializeField] private BagPresenter bagModelInstance;
    [SerializeField] private BagUIView bagUIInstance;
    [SerializeField] private MotionControl motionControlInstance;
    public override void InstallBindings()
    {
        BindBag();
        BindListPool();
        BindView();
        BindMControl();
    }

    private void BindBag()
    {
        Container.Bind<BagPresenter>()
            .FromInstance(bagModelInstance).
            AsSingle();
    }

    private void BindListPool()
    {
        Container.Bind<ListPool<Item>>()
                 .FromNew()
                 .AsSingle();
    }

    private void BindView()
    {
        Container.Bind<BagUIView>()
            .FromInstance(bagUIInstance)
            .AsSingle();
    }

    private void BindMControl()
    {
        Container.Bind<MotionControl>()
          .FromInstance(motionControlInstance)
          .AsSingle();
    }
}
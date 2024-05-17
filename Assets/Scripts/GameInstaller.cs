using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] CardItem _cardItemPrefab;

    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
        Container.Bind<CardManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GridSelectorElement>().FromComponentInHierarchy().AsSingle();
        Container.BindFactory<CardItem, CardItemFactory>().FromComponentInNewPrefab(_cardItemPrefab);
    }
}

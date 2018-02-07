using System;
using Zenject;

public class Installer : MonoInstaller<Installer> {
    public override void InstallBindings(){
        //Container.Bind<AStar>().AsSingle();
    }
}

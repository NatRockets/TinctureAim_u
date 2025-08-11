using Reflex.Core;
using UnityEngine;

public class ReflexInit : MonoBehaviour, IInstaller
{
    [SerializeField] private Linker target;
    
    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(target);
    }
}

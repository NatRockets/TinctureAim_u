using UnityEngine;

public class Linker : MonoBehaviour
{
    [SerializeField] private Tools tools;
    [SerializeField] private SourcePlay sourcePlay;
    
    public IntContainer GetData()
    {
        return tools.GetValue();
    }

    public void SetData(IntContainer data)
    {
        tools.SetValue(data);
    }

    public void PlaySound(int type)
    {
        sourcePlay.Play(type);
    }
}

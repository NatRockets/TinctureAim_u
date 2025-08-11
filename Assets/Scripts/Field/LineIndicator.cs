using UnityEngine;

public class LineIndicator : MonoBehaviour
{
    //private MeshRenderer frameRenderer;
    private Material indicatorMaterial;

    private Color targetColor;
    
    public void SetupIndicator()
    {
        MeshRenderer cellRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        indicatorMaterial = Instantiate(cellRenderer.material);
        cellRenderer.material = indicatorMaterial;

        //frameRenderer = transform.GetChild(1).GetComponent<MeshRenderer>();
    }

    public void ResetIndicator(Color initColor)
    {
        targetColor = initColor;
        indicatorMaterial.color = initColor;
        //frameRenderer.enabled = false;
    }

    public void SwitchFrame(bool active)
    {
        //frameRenderer.enabled = active;
    }

    public Color GetColor()
    {
        return targetColor;
    }
}

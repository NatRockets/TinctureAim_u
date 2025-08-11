using UnityEngine;

public class CellUnit : MonoBehaviour
{
    private MeshRenderer frameRenderer;
    private Material cellMaterial;
    
    private Transform cellTransform;

    private bool isFilled;
    
    public void SetupCell()
    {
        cellTransform = transform;
        MeshRenderer cellRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        cellMaterial = Instantiate(cellRenderer.material);
        cellRenderer.material = cellMaterial;

        frameRenderer = transform.GetChild(1).GetComponent<MeshRenderer>();
    }

    public void ResetCell(Color resetColor)
    {
        isFilled = false;
        cellMaterial.color = resetColor;
        SwitchFrame(false);
    }

    public void SetColor(Color newColor)
    {
        cellMaterial.color = newColor;
        isFilled = true;
    }
    
    public void SetColor1(Color newColor)
    {
        cellMaterial.color = newColor;
    }

    public void SwitchFrame(bool active)
    {
        frameRenderer.enabled = active;
    }

    public bool IsFilled()
    {
        return isFilled;
    }

    public Vector3 GetCellPosition()
    {
        return cellTransform.position;
    }
}

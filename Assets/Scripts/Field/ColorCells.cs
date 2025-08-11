using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColorCells : MonoBehaviour
{
    [SerializeField] private Transform fieldParentObject;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private float cellOffset;
    [SerializeField] private int fieldSize = 5;
    [SerializeField] private Vector3 fieldOffset;

    private CellUnit[,] colorCells;
    private LineIndicator[] rowIndicators;
    private LineIndicator[] colIndicators;

    private int colInd;
    private int rowInd;

    private int filledCells;

    private Color defaultColor;

    public void Generate()
    {
        defaultColor = Color.white;
        ColorUtility.TryParseHtmlString("#4D4747", out defaultColor);
        
        colorCells = new CellUnit[fieldSize, fieldSize];
        rowIndicators = new LineIndicator[fieldSize];
        colIndicators = new LineIndicator[fieldSize];
        
        GameObject cell;
        
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                cell = Instantiate(cellPrefab);
                cell.transform.SetParent(fieldParentObject);
                cell.transform.position =
                    new Vector3(i * cellOffset + fieldOffset.x,
                        -j * cellOffset + fieldOffset.y, 0f);

                colorCells[i, j] = cell.GetComponent<CellUnit>();
                colorCells[i, j].SetupCell();
            }

            cell = Instantiate(linePrefab);
            cell.transform.SetParent(fieldParentObject);
            cell.transform.position = new Vector3(i * cellOffset + fieldOffset.x,
                 (cellOffset + 1.5f) + fieldOffset.y, 0f);
            colIndicators[i] = cell.GetComponent<LineIndicator>();
            colIndicators[i].SetupIndicator();
        }
        
        for (int i = 0; i < fieldSize; i++)
        {
            cell = Instantiate(linePrefab);
            cell.transform.SetParent(fieldParentObject);
            cell.transform.position = new Vector3(-(cellOffset + 1.5f) + fieldOffset.x,
                -i * cellOffset + fieldOffset.y, 0f);
            rowIndicators[i] = cell.GetComponent<LineIndicator>();
            rowIndicators[i].SetupIndicator();
        }
    }

    public Color PickRandomUnfilled()
    {
        colorCells[colInd, rowInd].SwitchFrame(false);
        colIndicators[colInd].SwitchFrame(false);
        rowIndicators[rowInd].SwitchFrame(false);
        
        do
        {
            rowInd = Random.Range(0, fieldSize);
            colInd = Random.Range(0, fieldSize);
        } 
        while (colorCells[colInd, rowInd].IsFilled());
        
        colorCells[colInd, rowInd].SwitchFrame(true);
        colIndicators[colInd].SwitchFrame(true);
        rowIndicators[rowInd].SwitchFrame(true);

        Color col1 = colIndicators[colInd].GetColor();
        Color col2 = rowIndicators[rowInd].GetColor();
        
        //colorCells[colInd, rowInd].SetColor1(new Color((col1.r + col2.r) / 2, (col1.g + col2.g) / 2, (col1.b + col2.b) / 2, 1f));
        
        return new Color((col1.r + col2.r) / 2, (col1.g + col2.g) / 2, (col1.b + col2.b) / 2, 1f);
    }

    public bool SetCurrentCell(Color target)
    {
        colorCells[colInd, rowInd].SetColor(target);

        filledCells++;
        //Debug.Log("filled: "+ filledCells);

        return filledCells >= (fieldSize * fieldSize);
    }

    public Vector3 GetCurrentCellPosition()
    {
        return colorCells[colInd, rowInd].GetCellPosition();
    }

    public void ResetCells()
    {
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                colorCells[i, j].ResetCell(defaultColor);
            }
        }
        
        //generate colors and set indicators
        List<Color> colColors = new List<Color>();
        List<Color> rowColors = new List<Color>();
        
        for (int i = 0; i < fieldSize; i++)
        {
            colColors.Add(Color.HSVToRGB((i + Random.Range(0f, 1f)) / fieldSize, 1f, 1f));
            rowColors.Add(Color.HSVToRGB((i + Random.Range(0f, 1f)) / fieldSize, 1f, 1f));
        }
        Shuffle(colColors);
        Shuffle(rowColors);
        
        for (int i = 0; i < fieldSize; i++)
        {
            colIndicators[i].ResetIndicator(colColors[i]);
            rowIndicators[i].ResetIndicator(rowColors[i]);
        }

        filledCells = 0;
    }
    
    private void Shuffle<T> (List<T> array)
    {
        int n = array.Count;
        while (n > 1) 
        {
            int k = Random.Range(0, n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }
}

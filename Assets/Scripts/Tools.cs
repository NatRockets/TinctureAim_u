using UnityEngine;
using Newtonsoft.Json;

public class Tools : MonoBehaviour
{
    private IntContainer intContainer = new();

    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey("ta_d_pv"))
        {
            intContainer.automoves = 10;
            intContainer.clears = 10;
            return;
        }

        string saveData = PlayerPrefs.GetString("ta_d_pv");
        intContainer = JsonConvert.DeserializeObject<IntContainer>(saveData);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetString("ta_d_pv", JsonConvert.SerializeObject(intContainer));
    }

    public void SetValue(IntContainer data)
    {
        intContainer = data;
    }

    public IntContainer GetValue()
    {
        return intContainer;
    }
}
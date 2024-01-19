using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPackManager : MonoBehaviour
{
    public List<BloodPackUI> SelectedBloodPacks { get; private set; } = new List<BloodPackUI>();

    public void SelectBloodPack(BloodPackUI bloodPack)
    {
        if (!SelectedBloodPacks.Contains(bloodPack))
        {
            SelectedBloodPacks.Add(bloodPack);
        }
    }

    public void DeselectBloodPack(BloodPackUI bloodPack)
    {
        if (SelectedBloodPacks.Contains(bloodPack))
        {
            SelectedBloodPacks.Remove(bloodPack);
        }
    }
}

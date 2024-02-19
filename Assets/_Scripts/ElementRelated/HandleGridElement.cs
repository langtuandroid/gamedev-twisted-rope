using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGridElement : MonoBehaviour
{
    public GameObject selectedVisual;

    private bool _isBusy;
    public bool IsBusy => _isBusy;

    public void ChangeVisual(bool state)
    {
        selectedVisual.SetActive(state);
    }

    public void SetIsBusy(bool isBusy) => _isBusy = isBusy;
}

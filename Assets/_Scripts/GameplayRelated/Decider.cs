using System.Collections;
using _Twisted._Scripts.ControllerRelated;
using UnityEngine;

public class Decider : MonoBehaviour
{
    public static Decider instance;
    private int _activeRopes = 0;
    private bool _levelComplete;

    private void Awake() => instance = this;

    public void RopeAdded() => _activeRopes++;

    public void RopeCollapsed()
    {
        _activeRopes--;
        if (_activeRopes <= 0 && !_levelComplete) StartCoroutine(LevelEndCoroutine());
    }

    private IEnumerator LevelEndCoroutine()
    {
        _levelComplete = true;
        yield return new WaitForSeconds(0.1f);
        MainController.instance.SetActionType(GameState.Levelwin);
    }
}

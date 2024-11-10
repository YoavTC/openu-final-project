using UnityEngine;

public class TowerIdle : TowerBase
{
    protected override void CooldownAction()
    {
        Debug.Log("Action!");
    }
}

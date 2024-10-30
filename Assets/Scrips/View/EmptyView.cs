using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyView : BaseView
{
    public void PlayGame()
    {
        ViewManager.instance.SwitchView(ViewIndex.IngameView);
        Time.timeScale = 1;
    }
}

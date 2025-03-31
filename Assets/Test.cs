using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    public RectTransform testTransform;


    [ContextMenu(nameof(ForceUpdate))]
    public void ForceUpdate()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(testTransform);
    }
}

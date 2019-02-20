/** Copyright (c) 2019 mrma617@gmail.com
 *  Author: MrBaoquan
 *  CreateTime: 2019-2-18 16:46
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMeshSettings : MonoBehaviour
{
    public InputField if_NumRows;
    public InputField if_NumColumns;
    public Button btn_Apply;

    public void OnApply()
    {
        int numRows = System.Convert.ToInt32(if_NumRows.text);
        int numColumns = System.Convert.ToInt32(if_NumColumns.text);
        GameController.Instance.SetMeshSize(numRows, numColumns);
        GameController.Instance.ReGeneratePlane();
        Debug.Log("test");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game _InsGame;
    public Transform HightSel;
    bool isSelect = false;
    //选中的元素
    Box SelBox;

    void Awake()
    {
        _InsGame = this;
    }

    public void Select(Box box)
    {
        //如果已经选择了
        if(isSelect && CheckNear(SelBox, box))
        {
            StartCheck(SelBox, box);
            SetSelPos(false, box.transform);
        }
        else
        {
            SelBox = box;
            SetSelPos(true, box.transform);
        }
    }

    void StartCheck(Box b1, Box b2)
    {
        MapClass._InsMap.StartCheck(b1, b2);
    }

    //设置光标位置
    void SetSelPos(bool sel, Transform ts)
    {
        if(sel)
        {
            HightSel.parent = ts;
            HightSel.localPosition = Vector3.zero;
        }
        isSelect = sel;
        HightSel.gameObject.active = sel;
    }

    //检测是否是相邻的元素
    bool CheckNear(Box b1, Box b2)
    {
        return System.Math.Abs(b1.PosX - b2.PosX)
            + System.Math.Abs(b1.PosY - b2.PosY) == 1;
    }
}
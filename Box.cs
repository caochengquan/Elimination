using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    //偏移量
    int offsetX = 140;
    int offsetY = 140;
    TweenPosition tweenPos;

    public int type;//图片类型
    private bool boxActive;//激活状态
    private int posx;
    private int posy;

    public int Type
    {
        set
        {
            type = value;
            transform.GetComponent<UISprite>().spriteName = "button_" + type.ToString();
            transform.GetComponent<UIButton>().normalSprite = "button_" + type.ToString();
        }
        get
        {
            return type;
        }
    }
    public int PosX
    {
        set
        {
            posx = value;
            UpdatePos();
        }
        get
        {
            return posx;
        }
    }
    public int PosY
    {
        set
        {
            posy = value;
            UpdatePos();
        }
        get
        {
            return posy;
        }
    }
    public bool BoxActive
    {
        set
        {
            boxActive = value;
            UpdateActive();
        }
        get
        {
            return boxActive;
        }
    }

    public void OnMouseDown()
    {
        Game._InsGame.Select(this);
    }

    private void UpdatePos()
    {
        transform.localPosition = new Vector3(PosX * offsetX, PosY * offsetY, 0);
        transform.name = PosX.ToString() + PosY.ToString();
    }

    private void UpdateActive()
    {
        gameObject.active = boxActive;
    }
}
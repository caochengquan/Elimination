using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClass : MonoBehaviour
{
    public static MapClass _InsMap;
    public int Length;
    public int Width;
    public int BoxNum = 5;
    public GameObject BoxPrefab;

    private Box[,] Map;
    //public GameObject[] GoArray;

    void Awake()
    {
        _InsMap = this;
    }

    void Start()
    {
        if (Length > 5 && Width > 5)
        {
            Map = new Box[Length, Width];
        }
        CreatePrefab();
        ForCheck();
    }

    void CreatePrefab()
    {
        GameObject go;
        int randNum;
        for (int i = 0; i < Length; ++i)
        {
            for (int j = 0; j < Width; ++j)
            {
                //int randNum = Random.Range(0, GoArray.Length);
                //go = Instantiate(GoArray[randNum], transform.position, Quaternion.identity, transform);
                randNum = Random.Range(0, BoxNum);
                go = Instantiate(BoxPrefab, transform.position, Quaternion.identity, transform);
                Box box = go.GetComponent<Box>();
                Map[i, j] = box;
                box.PosX = i;
                box.PosY= j;
                box.BoxActive = true;
                box.Type = randNum;
            }
        }
    }

    public void StartCheck(Box b1, Box b2)
    {
        ExchangePos(b1, b2);
        bool canClean = CheckClean(b1) || CheckClean(b2);
        if (canClean)
        {
            FillBox();
            ForCheck();
        }
        else
        {
            ExchangePos(b1, b2);
        }
    }

    //循环检测
    void ForCheck()
    {
        for (int i = 0; i < Length; ++i)
        {
            for (int j = 0; j < Width; ++j)
            {
                if(CheckClean(Map[i, j]))
                {
                    FillBox();
                    ForCheck();
                }
            }
        }
    }

    //单个检测
    bool CheckClean(Box b1)
    {
        bool canClean = false;
        List<Box> list = CheckAlgorithms(b1);
        if (list != null)
        {
            HideBox(list);
            canClean = true;
        }
        return canClean;
    }

    //消除检测
    List<Box> CheckAlgorithms(Box box)
    {
        int boxType = box.Type;
        List<Box> boxList = new List<Box>() { box };
        bool HorClean = true;

        int num = 1;
        int y = box.PosY;

        for (int i = box.PosX - 1; i > -1; --i)
        {
            if (Map[i, y].Type == boxType)
            {
                ++num;
                boxList.Add(Map[i, y]);
            }
            else
                break;
        }

        for (int i = box.PosX + 1; i < Length; ++i)
        {
            if (Map[i, y].Type == boxType)
            {
                ++num;
                boxList.Add(Map[i, y]);
            }
            else
                break;
        }
        if (num < 3)
        {
            HorClean = false;
            boxList.Clear();
            boxList.Add(box);
            num = 1;
        }

        int numV = 0;
        int x = box.PosX;

        for (int i = box.PosY - 1; i > -1; --i)
        {
            if (Map[x, i].Type == boxType)
            {
                ++numV;
                boxList.Add(Map[x, i]);
            }
            else
                break;
        }

        for (int i = box.PosY + 1; i < Length; ++i)
        {
            if (Map[x, i].Type == boxType)
            {
                ++numV;
                boxList.Add(Map[x, i]);
            }
            else
                break;
        }

        //水平能消除
        if (HorClean)
        {
            //垂直不能消除
            if (numV < 2)
            {
                if(boxList.Count > num)
                boxList.RemoveRange(num, numV);
            }
            return boxList;
        }
        else
        {
            if (numV < 2)
            {
                boxList.Clear();
                return null;
            }
            return boxList;
        }

        return null;
    }

    //隐藏（销毁）元素
    void HideBox(List<Box> list)
    {
        foreach(Box v in list)
        {
            Map[v.PosX, v.PosY].BoxActive = false;
        }
    }

    //逐个入位算法填充    
    void FillBox()
    {
        Queue<Box> showQue = new Queue<Box>();
        Stack<Box> hideSta = new Stack<Box>();
        int num = 0;
        //逐列
        for (int i = 0; i < Length; ++i)
        {
            //逐行
            for (int j = 0; j < Width; ++j)
            {
                if(Map[i, j].BoxActive)
                {
                    ++num;
                    showQue.Enqueue(Map[i, j]);
                }
                else
                {
                    hideSta.Push(Map[i, j]);
                }
            }

            if (num < Width)
            {
                for (int k = 0; k < num; ++k)
                {
                    BoxDown(showQue.Dequeue(), i, k);
                }
                for (int k = num; k < Width; ++k)
                {
                    //重新利用
                    hideSta.Peek().Type = Random.Range(0, BoxNum);
                    BoxDown(hideSta.Pop(), i, k);
                }
            }
            showQue.Clear();
            hideSta.Clear();
            num = 0;
        }
    }

    //元素下降操作
    void BoxDown(Box b1, int x, int y)
    {
        Map[x, y] = null;
        Map[x, y] = b1;

        b1.PosX = x;
        b1.PosY = y;
        b1.BoxActive = true;
    }

    //交换元素位置
    void ExchangePos(Box b1, Box b2)
    {
        Box temp = b1;
        Map[b1.PosX, b1.PosY] = Map[b2.PosX, b2.PosY];
        Map[b2.PosX, b2.PosY] = temp;

        b1.PosX = b1.PosX ^ b2.PosX;
        b2.PosX = b1.PosX ^ b2.PosX;
        b1.PosX = b1.PosX ^ b2.PosX;

        b1.PosY = b1.PosY ^ b2.PosY;
        b2.PosY = b1.PosY ^ b2.PosY;
        b1.PosY = b1.PosY ^ b2.PosY;
    }
}

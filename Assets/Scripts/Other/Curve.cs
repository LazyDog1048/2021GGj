using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurveSet
{
    public float delayTime;
    public int pointNum;
    public int arrNum;
    public Curve curve;

    [HideInInspector] public List<Vector3> trajectoryPoint = new List<Vector3>();
}




public  class Curve 
{
    bool isManual;
    int pointCount;
    int nodeNum;
    Vector3 startPoint;
    Vector3 targetPoint;
    Vector3 movePoint;
    float randomLR;
    private List<Vector3> trajectoryPoint = new List<Vector3>(); //弹道
    public Curve(int pointCount,int nodeNum,Vector3 startPoint,Vector3 targetPoint)
    {
        isManual = false;
        this.pointCount = pointCount;
        this.nodeNum = nodeNum;
        this.startPoint = startPoint;
        this.targetPoint = targetPoint;
    }
    public Curve(int pointCount, Vector3 startPoint, Vector3 targetPoint,Vector3 movePoint)
    {
        isManual = true;
        this.pointCount = pointCount;
        this.startPoint = startPoint;
        this.targetPoint = targetPoint;
        this.movePoint = movePoint;
    }
    public void SetStartAndEnd(Vector3 startPoint, Vector3 targetPoint)
    {
        this.startPoint = startPoint;
        this.targetPoint = targetPoint;
    }

  
    void Init(Vector3[] gameObjectPos, int i)
    {
        Vector3[] points = new Vector3[gameObjectPos.Length - 1];
        
        for (int j = 0; j < points.Length; j++)
        {
            points[j] = Vector3.Lerp(gameObjectPos[j], gameObjectPos[j + 1], i /(float)pointCount);

        }
        
        if (points.Length > 1)
            Init(points, i);
        else
        {
            trajectoryPoint.Add(points[0]);
            return;
        }

    }

    public List<Vector3> pointPos()
    {
        Vector3[] nodeArr;
        if (!isManual)
            nodeArr = NodePos();
       else
        {
            nodeArr = new Vector3[] { startPoint,movePoint, targetPoint };
        }
            
        trajectoryPoint.Clear();
        for (int i = 0; i < pointCount; i++)
        {
            Init(nodeArr, i);
        }
        trajectoryPoint.Add(nodeArr[nodeArr.Length - 1]);

        return trajectoryPoint;
    }

    //时间任意结点

    Vector3[] NodePos()
    {
        Vector3[] pointPos = new Vector3[nodeNum];
        pointPos[0] = startPoint;

        pointPos[nodeNum - 1] = targetPoint;

        if(nodeNum>2)
        {
            float distanceY = pointPos[0].y - pointPos[nodeNum - 1].y;        //坐标与目标的y轴距离
            float distanceX = pointPos[0].x - pointPos[nodeNum - 1].x;

            if (Mathf.Abs(distanceY) > Mathf.Abs(distanceX))
            {
                pointPos = setPoints(distanceY, true, pointPos);
            }

            else
            {
                pointPos = setPoints(distanceX, false, pointPos);
            }
        }else if(nodeNum<2)
        {
            Debug.Log("节点数量必须大于等于2");
        }
       
       
        return pointPos;
    }


    
     Vector3[] setPoints(float longSide, bool isDistanceY, Vector3[] nodePos)
    {
        float longSideOffset = longSide / nodeNum;

        for (int i = 1; i < nodeNum - 1; i++)
        {
           float LongSide = Random.Range(0, 1f);
           float  ShortSide = Random.Range(0, 1f);
            RandomLR();                     //随机左右

            if (isDistanceY)
            {

                nodePos[i] = new Vector3(nodePos[0].x + randomLR * longSide * ShortSide,
                                                            nodePos[0].y - longSideOffset * (i + LongSide), 0);
                //偏差为长距离乘以系数
                //pos1y轴距离不超过distanc等分点
            }
            else
            {
                nodePos[i] = new Vector3(nodePos[0].x - longSideOffset * (i + LongSide),
                                                                  nodePos[0].y + randomLR * longSide * ShortSide, 0);
            }
        }
        return nodePos;
    }

    //--------------------------------------------------------

    public Vector3 GetRandomPoint()
    {

        Vector3 randomNode;

        float distanceY = startPoint.y - targetPoint.y;        //坐标与目标的y轴距离
        float distanceX = startPoint.x - targetPoint.x;        //坐标与目标的y轴距离
        RandomLR();

        float LongSide = Random.Range(0, 1f);                       //长边系数  
        float ShortSide = Random.Range(0, 1f);                          //短边系数

        if (Mathf.Abs(distanceY) > Mathf.Abs(distanceX))            //arrNum为3
        {
            float longSideOffset = distanceY / nodeNum;          

            randomNode = new Vector3(startPoint.x + randomLR * distanceY * ShortSide,
                                                           startPoint.y - longSideOffset * (1 + LongSide), 0);
        }
        else
        {
            float longSideOffset = distanceX / nodeNum;      
    
            randomNode = new Vector3(startPoint.x - longSideOffset * (1 + LongSide),
                                                                  startPoint.y + randomLR * distanceX * ShortSide, 0);
        }
        return randomNode;
    }

    void RandomLR()
    {
        float temp = Random.Range(-1f, 1f);
        if (temp > 0)
            randomLR = 1;
        else if (temp < 0)
            randomLR = -1;
        else
            RandomLR();
    }


}

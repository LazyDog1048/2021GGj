using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FindObjectFromTag
{
    string tagName;
    float distance;        //范围之内
    Vector2 selfPos;   //GetPlayerPos().position;
    List<GameObject> findObjects;
    int enemtNums;

    public FindObjectFromTag(string tagName, Vector2 selfPos, float distance)
    {
        this.tagName = tagName;
        this.selfPos = selfPos;
        this.distance = distance;
    }


    public Transform ClosestTransform()
    {
        GameObject closestTarget;

        List<GameObject> temp = new List<GameObject>();                                         //暂存移除的obj
        findObjects = GameObject.FindGameObjectsWithTag(tagName).ToList();

        if (findObjects.Count == 0)
            return null;
        closestTarget = findObjects[0];

        float minDistanceSqr = ((Vector2)findObjects[0].transform.position - selfPos).sqrMagnitude;         //距离的平方        

        for (int i = 0; i < findObjects.Count; i++)
        {
            if (((Vector2)findObjects[i].transform.position - selfPos).sqrMagnitude > distance * distance)              //如果距离大于最大距离           
            {
                temp.Add(findObjects[i]);                           //添加进移除
                //continue;
            }
            else if (minDistanceSqr > ((Vector2)findObjects[i].transform.position - selfPos).sqrMagnitude)
            {
                minDistanceSqr = ((Vector2)findObjects[i].transform.position - selfPos).sqrMagnitude;
                closestTarget = findObjects[i];
            }
        }
        foreach (GameObject tempObj in temp)
        {
            findObjects.Remove(tempObj);
        }

        if (minDistanceSqr > distance * distance)                     //如果最小距离都大于攻击距离
        {
            return null;
        }

        enemtNums = findObjects.Count;
        return closestTarget.transform;
    }

    public List<GameObject> FindEnemyList(int listCount)
    {
        if (ClosestTransform() == null)
        {
            return null;
        }
        else if (listCount == 999)                                                                     //如果输入为999则对附近所有敌人进行攻击
        {
            listCount = enemtNums;
        }

        for (int i = 1; i < findObjects.Count; i++)                                     //根据距离对敌人进行排序
        {
            for (int j = i; j > 0; j--)
            {
                Vector2 point1 = findObjects[j - 1].transform.position;
                Vector2 point2 = findObjects[j].transform.position;
                //比较两个点的距离
                if ((point1 - selfPos).sqrMagnitude > (point2 - selfPos).sqrMagnitude)
                {
                    GameObject temp = findObjects[j - 1];
                    findObjects[j - 1] = findObjects[j];
                    findObjects[j] = temp;

                }
            }
        }

        for (int i = listCount; i < findObjects.Count; i++)
        {
            findObjects.Remove(findObjects[listCount - 1]);
        }
        if (listCount > enemtNums)                 //如果敌人数量小于标记数量       少的标记全部添加到最近的目标
        {
            int addnum = listCount - enemtNums;
            while (addnum > 0)
            {
                findObjects.Add(findObjects[0]);
                addnum--;
            }
        }
        return findObjects;
    }

    private static void swap<T>(ref T x, ref T y)
    {
        T temp = x;
        x = y;
        y = temp;
    }
}

public class Vec3ToVer3Int
{
    public Vector3Int transition(Vector3 vec3)
    {
        return new Vector3Int((int)vec3.x, (int)vec3.y, 0);
    }
}


public class GetMousePos
{
    public static Vector2 GetMousePosition()                                               //通过mainCamera获取
    {
        return  GetMousePositionWithZ();                                          //通过mainCamera获取
    }
    public static Vector3 GetMousePositionWithZ()                                               //通过mainCamera获取
    {
        return GetMousePositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMousePositionWithZ(Camera worldCamera)                 //通过指定Camera获取
    {
        return GetMousePositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMousePositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
public class GetAngle                                       //仅获取2d xy的夹角
{
    public static float  Angle(Vector3 target, Vector3 bullet)
    {
        Vector3 positionOne = target;
        Vector3 aimDirection =( target - bullet).normalized;

        //Vector2 relative = PointOne.transform.InverseTransformPoint(PointTwo).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;


        return angle;
    }
    
    public static void RotaWithZ(Vector3 target, Vector3 bullet,GameObject Obj)
    {
        Vector3 positionOne = target;
        Vector3 aimDirection =( target - bullet).normalized;

        //Vector2 relative = PointOne.transform.InverseTransformPoint(PointTwo).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        Obj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
}

public class Lookdir 
{
    public enum lookdir { down, right, up, left };
    public static lookdir GetLookDir(float angle)
    {

        float rota = angle - 90;
        if (-135 > rota && rota < 135)                 //下
        {
            return lookdir.down;
        }
        else if (-135 < rota && rota < -45)     //右
        {
            return lookdir.right;
        }
        else if (-45 < rota && rota < 45)    //上
        {
            return lookdir.up;
        }
        else                                                                    //左
        {
            return lookdir.left;
        }
    }
    
}

public class TimeToText
{
    //仅限分秒
    public static string time(int second)
    {
        int minute = 0;
        string minStr = "";
        string secStr = "";

        if (second > 60)
        {
            minute = second / 60;                   //119 /60 =1  119%60 =59 
            second %= 60;
        }
        if (minute < 10)
            minStr += "0";
        if (second < 10)
            secStr += "0";

        minStr += minute.ToString();
        secStr += second.ToString();

        return minStr + ":" + secStr;
    }
}

public class FindFiles 
{
    public static void FindObjPath(ref List<string> dirs, string targetPath = "")
    {
        DirectoryInfo root = new DirectoryInfo("Assets/Resources/" + targetPath);
        for (int i = 0; i < root.GetFiles().Length; i++)
        {
            if (Path.GetExtension(root.GetFiles()[i].Name) == ".prefab")
            {
                dirs.Add(targetPath +"/"+ Path.GetFileNameWithoutExtension(root.GetFiles()[i].Name));
                //Debug.Log(targetPath + Path.GetFileNameWithoutExtension(root.GetFiles()[i].Name));
            }
        }
    }
    #region GetDirs
    // 在菜单来创建 选项 ， 点击该选项执行搜索代码
    //[MenuItem("Tools/遍历项目所有文件夹")]
    //static void CheckSceneSetting()
    //{
    //    List<string> dirs = new List<string>();
    //    GetDirs(Application.dataPath, ref dirs);
    //}
    //参数1 为要查找的总路径， 参数2 保存路径
    /*
    public static void GetDirs(string dirPath, ref List<string> dirs, string targetPath = "")
    {

        foreach (string path in Directory.GetFiles(dirPath))
        {
            //    //获取所有文件夹中包含后缀为 .prefab 文件的路径
            //    //如果后缀为获取后缀为prefab的文件路径
            //Path.GetFileNameWithoutExtension(path)获取不带后缀的文件名
            if (path.Contains(targetPath) && Path.GetExtension(path) == ".prefab")
            {
                //添加相对路径 从Assests开始添加  
                //targetPath
                int startIndex = path.IndexOf(targetPath);
                int endIndex = path.IndexOf(".");
                dirs.Add(path.Substring(startIndex, endIndex - startIndex));
                Debug.Log(Path.GetFileNameWithoutExtension(path));
                Debug.Log(path.Substring(startIndex, endIndex-startIndex));

            }       

        }
        //遍历所有文件夹
        if (Directory.GetDirectories(dirPath).Length > 0)  
        {
            foreach (string path in Directory.GetDirectories(dirPath))
            {
                GetDirs(path, ref dirs, targetPath);
            }
        }
    }
    */
    #endregion
}

public class GetDir
{
     int dir;               //因为要保留上次的dir
    public  int Dir(float dir)
    {
        if (dir > 0)
        {
            this.dir = 1;
        }
        else if(dir<0)
        {
            this.dir = -1;
        }
        return this.dir;
    }
}

public class GetAColor
{
    static List<float> arr = new List<float>();
    public static Color RandomColor(float max = 1,float min = 0.7f,float alpha=0.7f)
    {
        arr = new List<float>();
        float random = Random.Range(min, max);
        Color randomColor;
        arr.Add(max);
        arr.Add(min);
        arr.Add(random);
        randomColor = Color.white;
        randomColor.a = alpha;
        for (int i = arr.Count - 1; i >= 0; i--)
        {
            int num = Random.Range(0, arr.Count);
            if (i == 0)
                randomColor.r = arr[num];
            else if (i == 1)
                randomColor.g = arr[num];
            else if (i == 2)
                randomColor.b = arr[num];
            arr.RemoveAt(num);
        }
        return randomColor;
    }
   
}
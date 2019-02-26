using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{

    public enum Face
    {
        Up, Down, Left, Right, Front, Back
    }
    public Face face;

    private Vector3 a;
    private Vector3 b;
    private Vector3 c;
    public Vector3 r;

    public float size = 100;
    public int x, y;
    public Vector2 percent;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnValidate()
    {
        // 确定朝向
        switch (face)
        {
            case Face.Up:
                a = Vector3.up;
                break;
            case Face.Down:
                a = Vector3.down;
                break;
            case Face.Left:
                a = Vector3.left;
                break;
            case Face.Right:
                a = Vector3.right;
                break;
            case Face.Front:
                a = Vector3.forward;
                break;
            case Face.Back:
                a = Vector3.back;
                break;
        }
        // 确定偏移量
        b = new Vector3(a.y, a.z, a.x);
        c = Vector3.Cross(a, b);

        // 限定遍历范围
        x = Mathf.Max(0, x);
        x = Mathf.Min(x, (int)size);
        y = Mathf.Max(0, y);
        y = Mathf.Min(y, (int)size);
        // 遍历到的位置
        percent = new Vector2(x, y)/(size-1);


        r = a + (percent.x - .5f) * 2 * b + (percent.y - .5f) * 2 * c;
    }
}

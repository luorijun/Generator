using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player: MonoBehaviour {

    public Text textA;
    public Text textB;
    public Text textC;

    public float speed = 300;

    private float mouseX;
    private float mouseY;
    private float rotate;

    private float vertical;
    private float horizontal;
    private float raise;

    private void Start() {
        Debug.Log(SystemInfo.graphicsDeviceName);
        Debug.Log(SystemInfo.graphicsMultiThreaded);
        Debug.Log(SystemInfo.graphicsDeviceType);
    }

    private void Update() {

        UpdateInput();
        UpdateState();

        textA.text = "速度：" + speed;
    }

    private void UpdateInput() {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        rotate = Input.GetAxis("Rotate");

        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        raise = Input.GetAxis("Raise");
    }

    private void UpdateState() {

        transform.localEulerAngles += new Vector3(-mouseY, mouseX, -rotate);

        transform.position += transform.TransformVector(
            new Vector3(horizontal, 0, vertical)*speed*Time.deltaTime);
    }
}

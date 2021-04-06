using UnityEngine;

public class CameraSeting : MonoBehaviour
{
    const float devHcight = 10f;//设计的尺寸高度
    const float devwidth = 5.625f; //设计的尺寸宽度

    private void Start()
    {
        //获取屏幕宽度
        float screenHeight = Screen.height;

        //Debug.Log("screenHeight = " + screenHeight);

        //拿到相机的正交属性设置摄像机大小
        float orthographicSize = GetComponent<Camera>().orthographicSize;

        //得到宽高比
        float aspectRatio = Screen.width * 1.0f / Screen.height;

        //实际的宽高比和摄像机的orthographicSize值来计算出摄像机的宽度值
        float cameraWidth = orthographicSize * 2 * aspectRatio;

        //Debug.Log("cameraWidth= " + cameraWidth);
        //Debug.Log("aspectRatio= " + aspectRatio);

        // 如果摄像机宽度小于设计的尺寸宽度,将尺寸宽度/2倍的宽高比 = 相机的大小
        if (cameraWidth < devwidth)
        {
            orthographicSize = devwidth / (2 * aspectRatio); 

            //Debug.Log("new orthographicSize = " + orthographicSize);

            GetComponent<Camera>().orthographicSize = orthographicSize;
        }
    }
}




using UnityEngine;
public class Cube : MonoBehaviour
{
    //скрипт для куба
    public CubeSaveData GetData()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        //не обязательно чтобы сам куб эти данные передавал, можно и другим скриптом собирать данные
        return new CubeSaveData()
        {
            cubePosition = transform.position
        };
    }
    
    public void SetData(CubeSaveData data)
    {
        transform.position = data.cubePosition;
    }
}
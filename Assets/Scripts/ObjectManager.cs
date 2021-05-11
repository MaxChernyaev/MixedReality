using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// скрипт для сбора объектов и их инфы
/// </summary>
public class ObjectManager : MonoBehaviour
{
    [SerializeField] private Saver _saver;

    private Plane[] planes;
    private Cube[] cubes;

    public Text TextLog;  // лог на canvas
    public GameObject CubeToSpawn; // объект, который будем ставить на сцену

    private void Start()
    {
        planes = GetComponentsInChildren<Plane>();
        //cubes = GetComponentsInChildren<Cube>();

        //LoadObject();
    }

    public void SaveButton()
    {
        GameObject[] allGo = FindObjectsOfType<GameObject>();
        cubes = allGo.Select(go => go.GetComponent<Cube>()).ToArray();

        SaveObjects();
    }
    public void LoadButton()
    {
        LoadObject();
    }

    private void OnDisable()
    {
        SaveObjects();
    }

    void OnDestroy()
    {
        SaveObjects();
    }

    private void LoadObject()
    {
        CommonSaveData commonSaveData = _saver.Load();

        if (commonSaveData == null)
        {
            Debug.Log("commonSaveData is null");
            TextLog.text = "commonSaveData is null";
            return;
        }
        
        for (int i = 0; i < planes.Length && i < commonSaveData.planesList.Count; i++)
        {
            planes[i].SetData(commonSaveData.planesList[i]);
        }

        for (int i = 0; /*i < cubes.Length &&*/ i < commonSaveData.cubesList.Count; i++)
        {
            TextLog.text = "ЗАГРУЖАЮ КУБЫ";
            var entry = Instantiate(CubeToSpawn); // СДЕЛАТЬ ИХ НАСЛЕДНИКАМИ ПЛОСКОСТИ!!!
            //entry.transform.SetParent();
            cubes = GetComponentsInChildren<Cube>();
            cubes[i].SetData(commonSaveData.cubesList[i]);
        }
    }

    private void SaveObjects()
    {
        TextLog.text = "СОХРАНЯЮ КУБЫ";
        //собираем данные с кубов и плоскостей.
        //LINQ цикл в одну строчку чтобы со всех взять GetData()
        CommonSaveData commonSaveData = new CommonSaveData()
        {
            planesList = planes.Select(plane => plane.GetData()).ToList(),
            cubesList = cubes.Select(cube => cube.GetData()).ToList()
        };
        TextLog.text = "вызываю Save, передаю объекты";
        _saver.Save(commonSaveData);
    }
}
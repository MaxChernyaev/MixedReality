using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;


/// <summary>
/// Этот скрипт создаёт плоскость, если найден якорь (базовая точка), затем
/// размешает маркер на плоскости и позволяет создавать объекты в нужных местах с линиями между ними
/// т.е. реализует процесс создания AR сцены
/// </summary>

public class ARSceneMakingManager : MonoBehaviour
{
    public GameObject parentPlane; // плоскость-родитель для всех остальных объектов
    public GameObject PlaneMarkerPrefab; // маркер (картинка круга)
    public GameObject ObjectToSpawn; // объект, который будем ставить на сцену
    private Vector3 point; // точка перечения луча и плоскости
    public Text TextLog;  // лог на canvas
    private ARTrackedImageManager myARTrackedImageManager; // чтобы выключать/выключать компонент ARTrackedImageManager
    private GameObject FindObject; // найденный объект (для переименования)
    private GameObject FindPlane; // найденная плоскость
    private int NumObject  = 0; // порядковый номер созданного объекта
    LineRenderer lineRenderer; // для отрисовки линий между объектами (треки)

    void Start()
    {   
        PlaneMarkerPrefab.SetActive(false); // убираем маркер до нахождения плоскости
        myARTrackedImageManager = GetComponent<ARTrackedImageManager>(); // компонент распознавания изображений
    }

    void Update()
    {
        FindGizmo(); // при нахождении базовой точки создать или позиционировать плоскость относительно неё

        ShowMarker(); // отображение маркера на плоскости

        InstantiateMyObject(); // установка объекта по нажатию
        //MyVRControllerTEST();
    }

    void FindGizmo()
    {
        GameObject[] allGo = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allGo)
        {
            if(go.CompareTag("BaseGizmo"))
            {
                if ((FindPlane = GameObject.Find("BasePlane(Clone)")) == true) // если плоскость уже создана позиционируем её относительно Gizmo
                {
                    FindPlane.transform.position = go.transform.position;
                    FindPlane.transform.rotation = go.transform.rotation;
                }
                else
                {
                    Instantiate(parentPlane, go.transform.position, go.transform.rotation); // в другом случае, создаём её
                }
            }
        }
    }

    // отображение маркера (картинка круга)
    void ShowMarker()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 4, Screen.height / 4, 0)); // отправляем луч из центра экрана
        
        if(Physics.Raycast(ray, out hit) == true) // ели пересечение было записываем в hit
        {
            point = hit.point; // точка пересечения луча с объектом
            //TextLog.text = hit.collider.gameObject.name; // выводим имя объекта на который смотрим
            PlaneMarkerPrefab.transform.position = point; // ставим в это место маркер (картинка круга)
            PlaneMarkerPrefab.SetActive(true); // показываем маркер
        }
    }

    // установка новой копии объекта на сцену
    void InstantiateMyObject()
    {
        // Нажали на экран или на нижнюю кнопку на VR контроллере под указательным пальцем
        if((Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)||(Input.GetMouseButtonDown(0)))
        {
            //TextLog.text = "НАЖАЛ НИЖНЮЮ КНОПКУ";
            Instantiate(ObjectToSpawn, point, ObjectToSpawn.transform.rotation, FindPlane.transform); // ставим в определенное заранее место наш объект + делаем его потомком плоскости
            NumObject++;
            FindObject = GameObject.Find("Cube(Clone)"); // находим свежесозданный объект
            FindObject.name = ("Cube" + NumObject); // переименовываем его с добавлением порядкового номера
        }
    }

    // создаю линию между двумя кубами
    public void LineDrawingButton(/*string obj1, string obj2*/)
    { 
        lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;    

        FindObject = GameObject.Find("Cube1");
        lineRenderer.SetPosition(0, FindObject.transform.position);
        FindObject = GameObject.Find("Cube2");
        lineRenderer.SetPosition(1, FindObject.transform.position);
    }

    // включение/выключение компонента ARTrackedImageManager, т.е. отслеживания QR-кода
    public void Test1Button()
    {
        myARTrackedImageManager.enabled = !myARTrackedImageManager.enabled;
    }

    // для тестирования какие команды посылает VR контроллер
    void MyVRControllerTEST()
    {
        if(Input.anyKeyDown)
            Debug.Log(Input.inputString);

        if (Input.GetMouseButtonDown(0)) // (нижняя кнопка на VR контроллере под указательным пальцем)
        {
            Debug.Log("Pressed primary button.");
            TextLog.text = "НАЖАЛ НИЖНЮЮ КНОПКУ 00000000000";
        }

        if (Input.GetMouseButtonDown(1)) // (верхняя кнопка на VR контроллере под указательным пальцем)
        {
            Debug.Log("Pressed secondary button.");
            TextLog.text = "НАЖАЛ ВЕРХНЮЮ КНОПКУ 111111111111";
        }

        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Pressed middle click.");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Debug.Log("JoystickButton0");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            Debug.Log("JoystickButton1");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            Debug.Log("JoystickButton2");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            Debug.Log("JoystickButton3");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            Debug.Log("JoystickButton4");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            Debug.Log("JoystickButton5");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            Debug.Log("JoystickButton6");
        }
    }
}

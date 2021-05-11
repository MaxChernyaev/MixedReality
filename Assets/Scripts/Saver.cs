using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Класс для сохранения и загрузки.
/// </summary>
public class Saver : MonoBehaviour
{
    public Text TextLog;  // лог на canvas

    /// <summary>
    /// Относительный путь к файлу загрузки данных.
    /// </summary>
    [SerializeField]
    private string _cubeDataJsonPath;
    
    /// <summary>
    /// Полный путь к файлу загрузки данных.
    /// </summary>
    private string _saveDataPath;

    /// <summary>
    /// Полный путь к файлу загрузки данных.
    /// </summary>
    public string SaveDataPath {
        get
        {
            if (_saveDataPath == null)
            {
                _saveDataPath = Path.Combine(Application.persistentDataPath, _cubeDataJsonPath);
            }

            return _saveDataPath;
        }
    }

    /// <summary>
    /// Загрузить данные кубов.
    /// </summary>
    public CommonSaveData Load()
    {
        if(File.Exists(SaveDataPath))
        {
            TextLog.text = "ФАЙЛ НАЙДЕН, ЗАГРУЖАЕМ";
            using (FileStream fileStream = File.Open(SaveDataPath, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                CommonSaveData loadData = JsonUtility.FromJson<CommonSaveData>(reader.ReadToEnd());

                return loadData;
            }
        }
        return null;

    }
    
    /// <summary>
    /// Сохранить данные кубов.
    /// </summary>
    public void Save(CommonSaveData saveData)
    {
        TextLog.text = "СОЗДАЮ ФАЙЛ СОХРАНЕНИЯ, ПИШЕМ ДАННЫЕ";
        using (FileStream fileStream = File.Open(SaveDataPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            fileStream.SetLength(0);

            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(JsonUtility.ToJson(saveData));
            }
        }
    }
}

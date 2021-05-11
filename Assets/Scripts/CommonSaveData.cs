using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CommonSaveData
{
    [SerializeField]
    public List<PlaneSaveData> planesList;
    
    [SerializeField]
    public List<CubeSaveData> cubesList;
    
    //и так далее для каждого вида данных
}
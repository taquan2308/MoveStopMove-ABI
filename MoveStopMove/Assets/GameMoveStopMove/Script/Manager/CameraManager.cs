using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Transform mainCameraTrans;
    [SerializeField] private Transform sub01CameraTrans;
    public Transform MainCameraTrans { get => mainCameraTrans;}
    public Transform Sub01CameraTrans { get => sub01CameraTrans;}
}

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using Vector3 = UnityEngine.Vector3;

public class MouseHover : MonoBehaviour
{
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    public LayerMask layersToHit;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        screenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if(Physics.Raycast(ray, out RaycastHit hitData)){
            worldPosition = hitData.point;
        }
        
        transform.position = worldPosition;
    }
}

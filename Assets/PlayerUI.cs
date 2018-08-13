using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour 
{
	//[SerializeField] Canvas playerCanvas;

	[Tooltip("The UI canvas prefab")]
    [SerializeField] GameObject playerCanvasPrefab = null;

    Camera cameraToLookAt;

    // Use this for initialization 
    void Start()
    {
        cameraToLookAt = Camera.main;
        Instantiate(playerCanvasPrefab, transform.position, Quaternion.identity, transform);
    }

    // Update is called once per frame 
    void LateUpdate()
    {
        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
	}
}

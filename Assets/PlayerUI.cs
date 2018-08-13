using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour 
{
	//[SerializeField] Canvas playerCanvas;

	public Image HealthBar{
		get { return healthbar; }
	}
	public Image EnergyBar{
		get { return energyBar; }
	}

	[Tooltip("The UI canvas prefab")]
    [SerializeField] GameObject playerCanvasPrefab = null;
	[SerializeField] Image healthbar;
	[SerializeField] Image energyBar;

    Camera cameraToLookAt;

    void Start()
    {
        cameraToLookAt = Camera.main;
        //Instantiate(playerCanvasPrefab, transform.position, Quaternion.identity, transform);
    }

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.G))
		{
			healthbar.fillAmount -= .1f;
			energyBar.fillAmount -= .1f;
		}
	}

    void LateUpdate()
    {
        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
	}
}

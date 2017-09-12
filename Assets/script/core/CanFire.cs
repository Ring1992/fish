using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CanFire : MonoBehaviour {
	public bool fingerDown = false;
	// Use this for initialization
	void Start () {
	
	}

	void OnMouseDown()
	{
		if (IsTouchedUI())
			return;
		
		fingerDown = true;
		GameMain.Singleton.StartFire();
	}

	void OnMouseUp()
	{
		
		fingerDown = false;
		GameMain.Singleton.StopFire();
	}

	// Update is called once per frame
	void Update () {
		if (fingerDown)
		{
			Vector3 worldpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			if (worldpos.y < GameMain.Singleton.GunTrans.position.y)
				worldpos.y = GameMain.Singleton.GunTrans.position.y;

			GameMain.Singleton.PlayGunAnim(worldpos);
		}
	}

	bool IsTouchedUI()
	{
		bool touchedUI = false;
		if (Application.isMobilePlatform)
		{
			if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
			{
				touchedUI = true;
			}
		}
		else if (EventSystem.current.IsPointerOverGameObject())
		{
			touchedUI = true;
		}
		return touchedUI;
	}
}

using UnityEngine;
using System.Collections;

public class FixBoxSize : MonoBehaviour {
	public BoxCollider box;
	public RectTransform canvasRectTransform;
	// Use this for initialization
	void Start () {
		Defines.CanvasWidthUnit = canvasRectTransform.rect.width;
		Defines.CanvasHeightUnit = canvasRectTransform.rect.height;
		SetBoxSize();
	}

	void SetBoxSize()
	{
		box.size = new Vector3(Defines.CanvasWidthUnit, Defines.CanvasHeightUnit, 1);
	}
}

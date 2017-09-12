using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitProcessor : MonoBehaviour {
	public static void ProcessHit(Bullet b, Fish f)
	{
		switch (f.HittableTypeS)
		{
			case "Normal":
				Process_NormalFish(b, f);
				break;
			case "SameTypeBomb":
				Process_FishTypeBomb2(b, f);
				break;
		}
	}

	static void Process_NormalFish(Bullet b, Fish fishFirst)
	{
		if (!fishFirst.Attackable)
			return;

		Transform bulletTs = b.transform;

		//生成碰撞范围
        RaycastHit[] results = Physics.SphereCastAll(bulletTs.position, b.RadiusStandardBoom * 0.6F/** useWebScaleRatio.ScaleCollider //将要修正,不同子弹不同大小 */, Vector3.forward);

		List<Fish> fishAll = new List<Fish>();
		fishAll.Add(fishFirst);
		Fish fishTmp = null;
		for (int i = 0; i != results.Length; ++i)
		{
			fishTmp = results[i].transform.GetComponent<Fish>();

			if (fishTmp != null
				&& fishFirst != fishTmp
				&& fishTmp.Attackable
				&& fishTmp.HittableTypeS == "Normal"
				&& !fishTmp.HitByBulletOnly)
			{
				fishAll.Add(fishTmp);
			}
		}
	}

	static void Process_FishTypeBomb2(Bullet b, Fish fishFirst)
	{
		if (!fishFirst.Attackable)
			return;
	}
}

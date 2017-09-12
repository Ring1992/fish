using UnityEngine;
using System.Collections;

public class FishGeneratSetter : MonoBehaviour
{
	public int curConfigIdx = 0;
	public FishGeneratConfig[] Configs;//数组跟ArenaType对应(Small,Medium,Larger)
	void Awake()
	{
		if (Configs == null || curConfigIdx >= Configs.Length)
			return;

		FishGenerator fg = GetComponent<FishGenerator>();
		if (fg == null)
			return;
		FishGeneratConfig fgc = Configs[curConfigIdx];
		fg.FishGenerateDatas = fgc.FishGenerateDatas;
		fg.FishGenerateUniqueDatas = fgc.FishGenerateUniqueDatas;
		fg.FishQueueData = fgc.FishQueueData;


		//fg.Interval_FishGenerate = fgc.Interval_FishGenerate;
		//fg.Interval_FishUniqueGenerate = fgc.Interval_FishUniqueGenerate;
		//fg.Interval_QueuGenerate = fgc.Interval_QueuGenerate;
		//fg.Interval_FlockGenerate = fgc.Interval_FlockGenerate;

		fg.Interval_FishGenerate.Min = fgc.Interval_FishGenerate.Min;
		fg.Interval_FishGenerate.Max = fgc.Interval_FishGenerate.Max;
		fg.Interval_FishUniqueGenerate.Min = fgc.Interval_FishUniqueGenerate.Min;
		fg.Interval_FishUniqueGenerate.Max = fgc.Interval_FishUniqueGenerate.Max;
		fg.Interval_QueuGenerate.Min = fgc.Interval_QueuGenerate.Min;
		fg.Interval_QueuGenerate.Max = fgc.Interval_QueuGenerate.Max;
		fg.Interval_FlockGenerate.Min = fgc.Interval_FlockGenerate.Min;
		fg.Interval_FlockGenerate.Max = fgc.Interval_FlockGenerate.Max;
	}
}

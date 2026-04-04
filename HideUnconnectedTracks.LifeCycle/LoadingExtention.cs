using ICities;

namespace HideUnconnectedTracks.LifeCycle;

public class LoadingExtention : LoadingExtensionBase
{
	public override void OnLevelLoaded(LoadMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)mode == 3 || (int)mode == 2 || (int)mode == 11)
		{
			LifeCycle.Load();
		}
	}

	public override void OnLevelUnloading()
	{
		LifeCycle.Release();
	}
}

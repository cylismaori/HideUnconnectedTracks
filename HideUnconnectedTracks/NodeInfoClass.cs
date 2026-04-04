using System.Runtime.CompilerServices;

namespace HideUnconnectedTracks;

public struct NodeInfoClass
{
	public int InconsistencyLevel;

	public int UsedIndex;

	public TrackType Track;

	public bool RequireWindSpeed;

	public Flags RequiredFlags;

	public Flags ForbiddenFlags;

	public int Layer;

	public bool EmptyTransparent;

	public bool RequireSurfaceMaps;

	public NodeInfoClass(Node nodeInfo, int usedIndex, int inconsistencyLevel)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		InconsistencyLevel = inconsistencyLevel;
		UsedIndex = usedIndex;
		Track = nodeInfo.m_connectGroup.GetTrackType();
		RequireWindSpeed = nodeInfo.m_requireWindSpeed;
		RequiredFlags = (Flags)0;
		ForbiddenFlags = (Flags)0;
		Layer = -1;
		EmptyTransparent = false;
		RequireSurfaceMaps = false;
		if (inconsistencyLevel <= 1)
		{
			RequiredFlags = nodeInfo.m_flagsRequired;
		}
		if (inconsistencyLevel == 0)
		{
			ForbiddenFlags = nodeInfo.m_flagsForbidden;
			Layer = nodeInfo.m_layer;
			EmptyTransparent = nodeInfo.m_emptyTransparent;
			RequireSurfaceMaps = nodeInfo.m_requireSurfaceMaps;
		}
	}

	public override string ToString()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Invalid comparison between Unknown and I4
		string text = $"NodeInfoClass(UsedIndex={UsedIndex}|Track={Track}|Wire={RequireWindSpeed}";
		if (Layer != -1)
		{
			text = text + "|layer=" + Layer;
		}
		if ((int)RequiredFlags > 0)
		{
			text = text + "|RequiredFlags=" + ((object)System.Runtime.CompilerServices.Unsafe.As<Flags, Flags>(ref RequiredFlags)/*cast due to .constrained prefix*/).ToString();
		}
		if ((int)ForbiddenFlags > 0)
		{
			text = text + "|ForbiddenFlags=" + ((object)System.Runtime.CompilerServices.Unsafe.As<Flags, Flags>(ref ForbiddenFlags)/*cast due to .constrained prefix*/).ToString();
		}
		if (RequireSurfaceMaps)
		{
			text += "|SurfaceMaps";
		}
		if (EmptyTransparent)
		{
			text += "|EmptyTransparent";
		}
		return text + ")";
	}
}

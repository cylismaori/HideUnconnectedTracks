namespace ObjUnity3D;

public class OBJFaceVertex
{
	public int m_VertexIndex = -1;

	public int m_UVIndex = -1;

	public int m_UV2Index = -1;

	public int m_NormalIndex = -1;

	public int m_ColorIndex = -1;

	public override int GetHashCode()
	{
		return m_VertexIndex ^ m_UVIndex ^ m_UV2Index ^ m_NormalIndex ^ m_ColorIndex;
	}

	public override bool Equals(object obj)
	{
		OBJFaceVertex oBJFaceVertex = (OBJFaceVertex)obj;
		return m_VertexIndex == oBJFaceVertex.m_VertexIndex && m_UVIndex == oBJFaceVertex.m_UVIndex && m_UV2Index == oBJFaceVertex.m_UV2Index && m_NormalIndex == oBJFaceVertex.m_NormalIndex && m_ColorIndex == oBJFaceVertex.m_ColorIndex;
	}
}

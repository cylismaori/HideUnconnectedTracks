using System;

namespace KianCommons;

internal class NetServiceException : Exception
{
	public NetServiceException(string m)
		: base(m)
	{
	}

	public NetServiceException()
	{
	}

	public NetServiceException(string m, Exception e)
		: base(m, e)
	{
	}
}

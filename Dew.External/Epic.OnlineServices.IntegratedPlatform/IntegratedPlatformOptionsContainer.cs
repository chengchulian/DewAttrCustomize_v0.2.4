using System;

namespace Epic.OnlineServices.IntegratedPlatform;

public sealed class IntegratedPlatformOptionsContainer : Handle
{
	public const int IntegratedplatformoptionscontainerAddApiLatest = 1;

	public IntegratedPlatformOptionsContainer()
	{
	}

	public IntegratedPlatformOptionsContainer(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result Add(ref IntegratedPlatformOptionsContainerAddOptions inOptions)
	{
		IntegratedPlatformOptionsContainerAddOptionsInternal inOptionsInternal = default(IntegratedPlatformOptionsContainerAddOptionsInternal);
		inOptionsInternal.Set(ref inOptions);
		Result result = Bindings.EOS_IntegratedPlatformOptionsContainer_Add(base.InnerHandle, ref inOptionsInternal);
		Helper.Dispose(ref inOptionsInternal);
		return result;
	}

	public void Release()
	{
		Bindings.EOS_IntegratedPlatformOptionsContainer_Release(base.InnerHandle);
	}
}

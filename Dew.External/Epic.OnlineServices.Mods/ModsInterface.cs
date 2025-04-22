using System;

namespace Epic.OnlineServices.Mods;

public sealed class ModsInterface : Handle
{
	public const int CopymodinfoApiLatest = 1;

	public const int EnumeratemodsApiLatest = 1;

	public const int InstallmodApiLatest = 1;

	public const int ModIdentifierApiLatest = 1;

	public const int ModinfoApiLatest = 1;

	public const int UninstallmodApiLatest = 1;

	public const int UpdatemodApiLatest = 1;

	public ModsInterface()
	{
	}

	public ModsInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopyModInfo(ref CopyModInfoOptions options, out ModInfo? outEnumeratedMods)
	{
		CopyModInfoOptionsInternal optionsInternal = default(CopyModInfoOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outEnumeratedModsAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Mods_CopyModInfo(base.InnerHandle, ref optionsInternal, ref outEnumeratedModsAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<ModInfoInternal, ModInfo>(outEnumeratedModsAddress, out outEnumeratedMods);
		if (outEnumeratedMods.HasValue)
		{
			Bindings.EOS_Mods_ModInfo_Release(outEnumeratedModsAddress);
		}
		return result;
	}

	public void EnumerateMods(ref EnumerateModsOptions options, object clientData, OnEnumerateModsCallback completionDelegate)
	{
		EnumerateModsOptionsInternal optionsInternal = default(EnumerateModsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnEnumerateModsCallbackInternal completionDelegateInternal = OnEnumerateModsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Mods_EnumerateMods(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void InstallMod(ref InstallModOptions options, object clientData, OnInstallModCallback completionDelegate)
	{
		InstallModOptionsInternal optionsInternal = default(InstallModOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnInstallModCallbackInternal completionDelegateInternal = OnInstallModCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Mods_InstallMod(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void UninstallMod(ref UninstallModOptions options, object clientData, OnUninstallModCallback completionDelegate)
	{
		UninstallModOptionsInternal optionsInternal = default(UninstallModOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUninstallModCallbackInternal completionDelegateInternal = OnUninstallModCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Mods_UninstallMod(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void UpdateMod(ref UpdateModOptions options, object clientData, OnUpdateModCallback completionDelegate)
	{
		UpdateModOptionsInternal optionsInternal = default(UpdateModOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUpdateModCallbackInternal completionDelegateInternal = OnUpdateModCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Mods_UpdateMod(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnEnumerateModsCallbackInternal))]
	internal static void OnEnumerateModsCallbackInternalImplementation(ref EnumerateModsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<EnumerateModsCallbackInfoInternal, OnEnumerateModsCallback, EnumerateModsCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnInstallModCallbackInternal))]
	internal static void OnInstallModCallbackInternalImplementation(ref InstallModCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<InstallModCallbackInfoInternal, OnInstallModCallback, InstallModCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUninstallModCallbackInternal))]
	internal static void OnUninstallModCallbackInternalImplementation(ref UninstallModCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<UninstallModCallbackInfoInternal, OnUninstallModCallback, UninstallModCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUpdateModCallbackInternal))]
	internal static void OnUpdateModCallbackInternalImplementation(ref UpdateModCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<UpdateModCallbackInfoInternal, OnUpdateModCallback, UpdateModCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}

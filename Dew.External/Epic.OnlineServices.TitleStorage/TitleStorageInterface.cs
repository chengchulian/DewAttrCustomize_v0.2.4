using System;

namespace Epic.OnlineServices.TitleStorage;

public sealed class TitleStorageInterface : Handle
{
	public const int CopyfilemetadataatindexApiLatest = 1;

	public const int CopyfilemetadataatindexoptionsApiLatest = 1;

	public const int CopyfilemetadatabyfilenameApiLatest = 1;

	public const int CopyfilemetadatabyfilenameoptionsApiLatest = 1;

	public const int DeletecacheApiLatest = 1;

	public const int DeletecacheoptionsApiLatest = 1;

	public const int FilemetadataApiLatest = 2;

	public const int FilenameMaxLengthBytes = 64;

	public const int GetfilemetadatacountApiLatest = 1;

	public const int GetfilemetadatacountoptionsApiLatest = 1;

	public const int QueryfileApiLatest = 1;

	public const int QueryfilelistApiLatest = 1;

	public const int QueryfilelistoptionsApiLatest = 1;

	public const int QueryfileoptionsApiLatest = 1;

	public const int ReadfileApiLatest = 1;

	public const int ReadfileoptionsApiLatest = 1;

	public TitleStorageInterface()
	{
	}

	public TitleStorageInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopyFileMetadataAtIndex(ref CopyFileMetadataAtIndexOptions options, out FileMetadata? outMetadata)
	{
		CopyFileMetadataAtIndexOptionsInternal optionsInternal = default(CopyFileMetadataAtIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outMetadataAddress = IntPtr.Zero;
		Result result = Bindings.EOS_TitleStorage_CopyFileMetadataAtIndex(base.InnerHandle, ref optionsInternal, ref outMetadataAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<FileMetadataInternal, FileMetadata>(outMetadataAddress, out outMetadata);
		if (outMetadata.HasValue)
		{
			Bindings.EOS_TitleStorage_FileMetadata_Release(outMetadataAddress);
		}
		return result;
	}

	public Result CopyFileMetadataByFilename(ref CopyFileMetadataByFilenameOptions options, out FileMetadata? outMetadata)
	{
		CopyFileMetadataByFilenameOptionsInternal optionsInternal = default(CopyFileMetadataByFilenameOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outMetadataAddress = IntPtr.Zero;
		Result result = Bindings.EOS_TitleStorage_CopyFileMetadataByFilename(base.InnerHandle, ref optionsInternal, ref outMetadataAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<FileMetadataInternal, FileMetadata>(outMetadataAddress, out outMetadata);
		if (outMetadata.HasValue)
		{
			Bindings.EOS_TitleStorage_FileMetadata_Release(outMetadataAddress);
		}
		return result;
	}

	public Result DeleteCache(ref DeleteCacheOptions options, object clientData, OnDeleteCacheCompleteCallback completionCallback)
	{
		DeleteCacheOptionsInternal optionsInternal = default(DeleteCacheOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnDeleteCacheCompleteCallbackInternal completionCallbackInternal = OnDeleteCacheCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionCallback, completionCallbackInternal);
		Result result = Bindings.EOS_TitleStorage_DeleteCache(base.InnerHandle, ref optionsInternal, clientDataAddress, completionCallbackInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetFileMetadataCount(ref GetFileMetadataCountOptions options)
	{
		GetFileMetadataCountOptionsInternal optionsInternal = default(GetFileMetadataCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_TitleStorage_GetFileMetadataCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void QueryFile(ref QueryFileOptions options, object clientData, OnQueryFileCompleteCallback completionCallback)
	{
		QueryFileOptionsInternal optionsInternal = default(QueryFileOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryFileCompleteCallbackInternal completionCallbackInternal = OnQueryFileCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionCallback, completionCallbackInternal);
		Bindings.EOS_TitleStorage_QueryFile(base.InnerHandle, ref optionsInternal, clientDataAddress, completionCallbackInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryFileList(ref QueryFileListOptions options, object clientData, OnQueryFileListCompleteCallback completionCallback)
	{
		QueryFileListOptionsInternal optionsInternal = default(QueryFileListOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryFileListCompleteCallbackInternal completionCallbackInternal = OnQueryFileListCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionCallback, completionCallbackInternal);
		Bindings.EOS_TitleStorage_QueryFileList(base.InnerHandle, ref optionsInternal, clientDataAddress, completionCallbackInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public TitleStorageFileTransferRequest ReadFile(ref ReadFileOptions options, object clientData, OnReadFileCompleteCallback completionCallback)
	{
		ReadFileOptionsInternal optionsInternal = default(ReadFileOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnReadFileCompleteCallbackInternal completionCallbackInternal = OnReadFileCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionCallback, completionCallbackInternal, options.ReadFileDataCallback, ReadFileOptionsInternal.ReadFileDataCallback, options.FileTransferProgressCallback, ReadFileOptionsInternal.FileTransferProgressCallback);
		IntPtr from = Bindings.EOS_TitleStorage_ReadFile(base.InnerHandle, ref optionsInternal, clientDataAddress, completionCallbackInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(from, out TitleStorageFileTransferRequest funcResultReturn);
		return funcResultReturn;
	}

	[MonoPInvokeCallback(typeof(OnDeleteCacheCompleteCallbackInternal))]
	internal static void OnDeleteCacheCompleteCallbackInternalImplementation(ref DeleteCacheCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<DeleteCacheCallbackInfoInternal, OnDeleteCacheCompleteCallback, DeleteCacheCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnFileTransferProgressCallbackInternal))]
	internal static void OnFileTransferProgressCallbackInternalImplementation(ref FileTransferProgressCallbackInfoInternal data)
	{
		if (Helper.TryGetStructCallback<FileTransferProgressCallbackInfoInternal, OnFileTransferProgressCallback, FileTransferProgressCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryFileCompleteCallbackInternal))]
	internal static void OnQueryFileCompleteCallbackInternalImplementation(ref QueryFileCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryFileCallbackInfoInternal, OnQueryFileCompleteCallback, QueryFileCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryFileListCompleteCallbackInternal))]
	internal static void OnQueryFileListCompleteCallbackInternalImplementation(ref QueryFileListCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryFileListCallbackInfoInternal, OnQueryFileListCompleteCallback, QueryFileListCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnReadFileCompleteCallbackInternal))]
	internal static void OnReadFileCompleteCallbackInternalImplementation(ref ReadFileCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<ReadFileCallbackInfoInternal, OnReadFileCompleteCallback, ReadFileCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnReadFileDataCallbackInternal))]
	internal static ReadResult OnReadFileDataCallbackInternalImplementation(ref ReadFileDataCallbackInfoInternal data)
	{
		if (Helper.TryGetStructCallback<ReadFileDataCallbackInfoInternal, OnReadFileDataCallback, ReadFileDataCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			return callback(ref callbackInfo);
		}
		return Helper.GetDefault<ReadResult>();
	}
}

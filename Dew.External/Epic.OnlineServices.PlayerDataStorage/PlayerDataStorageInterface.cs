using System;

namespace Epic.OnlineServices.PlayerDataStorage;

public sealed class PlayerDataStorageInterface : Handle
{
	public const int CopyfilemetadataatindexApiLatest = 1;

	public const int CopyfilemetadataatindexoptionsApiLatest = 1;

	public const int CopyfilemetadatabyfilenameApiLatest = 1;

	public const int CopyfilemetadatabyfilenameoptionsApiLatest = 1;

	public const int DeletecacheApiLatest = 1;

	public const int DeletecacheoptionsApiLatest = 1;

	public const int DeletefileApiLatest = 1;

	public const int DeletefileoptionsApiLatest = 1;

	public const int DuplicatefileApiLatest = 1;

	public const int DuplicatefileoptionsApiLatest = 1;

	public const int FilemetadataApiLatest = 3;

	public const int FilenameMaxLengthBytes = 64;

	public const int GetfilemetadatacountApiLatest = 1;

	public const int GetfilemetadatacountoptionsApiLatest = 1;

	public const int QueryfileApiLatest = 1;

	public const int QueryfilelistApiLatest = 2;

	public const int QueryfilelistoptionsApiLatest = 2;

	public const int QueryfileoptionsApiLatest = 1;

	public const int ReadfileApiLatest = 1;

	public const int ReadfileoptionsApiLatest = 1;

	public const int TimeUndefined = -1;

	public const int WritefileApiLatest = 1;

	public const int WritefileoptionsApiLatest = 1;

	public PlayerDataStorageInterface()
	{
	}

	public PlayerDataStorageInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopyFileMetadataAtIndex(ref CopyFileMetadataAtIndexOptions copyFileMetadataOptions, out FileMetadata? outMetadata)
	{
		CopyFileMetadataAtIndexOptionsInternal copyFileMetadataOptionsInternal = default(CopyFileMetadataAtIndexOptionsInternal);
		copyFileMetadataOptionsInternal.Set(ref copyFileMetadataOptions);
		IntPtr outMetadataAddress = IntPtr.Zero;
		Result result = Bindings.EOS_PlayerDataStorage_CopyFileMetadataAtIndex(base.InnerHandle, ref copyFileMetadataOptionsInternal, ref outMetadataAddress);
		Helper.Dispose(ref copyFileMetadataOptionsInternal);
		Helper.Get<FileMetadataInternal, FileMetadata>(outMetadataAddress, out outMetadata);
		if (outMetadata.HasValue)
		{
			Bindings.EOS_PlayerDataStorage_FileMetadata_Release(outMetadataAddress);
		}
		return result;
	}

	public Result CopyFileMetadataByFilename(ref CopyFileMetadataByFilenameOptions copyFileMetadataOptions, out FileMetadata? outMetadata)
	{
		CopyFileMetadataByFilenameOptionsInternal copyFileMetadataOptionsInternal = default(CopyFileMetadataByFilenameOptionsInternal);
		copyFileMetadataOptionsInternal.Set(ref copyFileMetadataOptions);
		IntPtr outMetadataAddress = IntPtr.Zero;
		Result result = Bindings.EOS_PlayerDataStorage_CopyFileMetadataByFilename(base.InnerHandle, ref copyFileMetadataOptionsInternal, ref outMetadataAddress);
		Helper.Dispose(ref copyFileMetadataOptionsInternal);
		Helper.Get<FileMetadataInternal, FileMetadata>(outMetadataAddress, out outMetadata);
		if (outMetadata.HasValue)
		{
			Bindings.EOS_PlayerDataStorage_FileMetadata_Release(outMetadataAddress);
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
		Result result = Bindings.EOS_PlayerDataStorage_DeleteCache(base.InnerHandle, ref optionsInternal, clientDataAddress, completionCallbackInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void DeleteFile(ref DeleteFileOptions deleteOptions, object clientData, OnDeleteFileCompleteCallback completionCallback)
	{
		DeleteFileOptionsInternal deleteOptionsInternal = default(DeleteFileOptionsInternal);
		deleteOptionsInternal.Set(ref deleteOptions);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnDeleteFileCompleteCallbackInternal completionCallbackInternal = OnDeleteFileCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionCallback, completionCallbackInternal);
		Bindings.EOS_PlayerDataStorage_DeleteFile(base.InnerHandle, ref deleteOptionsInternal, clientDataAddress, completionCallbackInternal);
		Helper.Dispose(ref deleteOptionsInternal);
	}

	public void DuplicateFile(ref DuplicateFileOptions duplicateOptions, object clientData, OnDuplicateFileCompleteCallback completionCallback)
	{
		DuplicateFileOptionsInternal duplicateOptionsInternal = default(DuplicateFileOptionsInternal);
		duplicateOptionsInternal.Set(ref duplicateOptions);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnDuplicateFileCompleteCallbackInternal completionCallbackInternal = OnDuplicateFileCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionCallback, completionCallbackInternal);
		Bindings.EOS_PlayerDataStorage_DuplicateFile(base.InnerHandle, ref duplicateOptionsInternal, clientDataAddress, completionCallbackInternal);
		Helper.Dispose(ref duplicateOptionsInternal);
	}

	public Result GetFileMetadataCount(ref GetFileMetadataCountOptions getFileMetadataCountOptions, out int outFileMetadataCount)
	{
		GetFileMetadataCountOptionsInternal getFileMetadataCountOptionsInternal = default(GetFileMetadataCountOptionsInternal);
		getFileMetadataCountOptionsInternal.Set(ref getFileMetadataCountOptions);
		outFileMetadataCount = Helper.GetDefault<int>();
		Result result = Bindings.EOS_PlayerDataStorage_GetFileMetadataCount(base.InnerHandle, ref getFileMetadataCountOptionsInternal, ref outFileMetadataCount);
		Helper.Dispose(ref getFileMetadataCountOptionsInternal);
		return result;
	}

	public void QueryFile(ref QueryFileOptions queryFileOptions, object clientData, OnQueryFileCompleteCallback completionCallback)
	{
		QueryFileOptionsInternal queryFileOptionsInternal = default(QueryFileOptionsInternal);
		queryFileOptionsInternal.Set(ref queryFileOptions);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryFileCompleteCallbackInternal completionCallbackInternal = OnQueryFileCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionCallback, completionCallbackInternal);
		Bindings.EOS_PlayerDataStorage_QueryFile(base.InnerHandle, ref queryFileOptionsInternal, clientDataAddress, completionCallbackInternal);
		Helper.Dispose(ref queryFileOptionsInternal);
	}

	public void QueryFileList(ref QueryFileListOptions queryFileListOptions, object clientData, OnQueryFileListCompleteCallback completionCallback)
	{
		QueryFileListOptionsInternal queryFileListOptionsInternal = default(QueryFileListOptionsInternal);
		queryFileListOptionsInternal.Set(ref queryFileListOptions);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryFileListCompleteCallbackInternal completionCallbackInternal = OnQueryFileListCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionCallback, completionCallbackInternal);
		Bindings.EOS_PlayerDataStorage_QueryFileList(base.InnerHandle, ref queryFileListOptionsInternal, clientDataAddress, completionCallbackInternal);
		Helper.Dispose(ref queryFileListOptionsInternal);
	}

	public PlayerDataStorageFileTransferRequest ReadFile(ref ReadFileOptions readOptions, object clientData, OnReadFileCompleteCallback completionCallback)
	{
		ReadFileOptionsInternal readOptionsInternal = default(ReadFileOptionsInternal);
		readOptionsInternal.Set(ref readOptions);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnReadFileCompleteCallbackInternal completionCallbackInternal = OnReadFileCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionCallback, completionCallbackInternal, readOptions.ReadFileDataCallback, ReadFileOptionsInternal.ReadFileDataCallback, readOptions.FileTransferProgressCallback, ReadFileOptionsInternal.FileTransferProgressCallback);
		IntPtr from = Bindings.EOS_PlayerDataStorage_ReadFile(base.InnerHandle, ref readOptionsInternal, clientDataAddress, completionCallbackInternal);
		Helper.Dispose(ref readOptionsInternal);
		Helper.Get(from, out PlayerDataStorageFileTransferRequest funcResultReturn);
		return funcResultReturn;
	}

	public PlayerDataStorageFileTransferRequest WriteFile(ref WriteFileOptions writeOptions, object clientData, OnWriteFileCompleteCallback completionCallback)
	{
		WriteFileOptionsInternal writeOptionsInternal = default(WriteFileOptionsInternal);
		writeOptionsInternal.Set(ref writeOptions);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnWriteFileCompleteCallbackInternal completionCallbackInternal = OnWriteFileCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionCallback, completionCallbackInternal, writeOptions.WriteFileDataCallback, WriteFileOptionsInternal.WriteFileDataCallback, writeOptions.FileTransferProgressCallback, WriteFileOptionsInternal.FileTransferProgressCallback);
		IntPtr from = Bindings.EOS_PlayerDataStorage_WriteFile(base.InnerHandle, ref writeOptionsInternal, clientDataAddress, completionCallbackInternal);
		Helper.Dispose(ref writeOptionsInternal);
		Helper.Get(from, out PlayerDataStorageFileTransferRequest funcResultReturn);
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

	[MonoPInvokeCallback(typeof(OnDeleteFileCompleteCallbackInternal))]
	internal static void OnDeleteFileCompleteCallbackInternalImplementation(ref DeleteFileCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<DeleteFileCallbackInfoInternal, OnDeleteFileCompleteCallback, DeleteFileCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnDuplicateFileCompleteCallbackInternal))]
	internal static void OnDuplicateFileCompleteCallbackInternalImplementation(ref DuplicateFileCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<DuplicateFileCallbackInfoInternal, OnDuplicateFileCompleteCallback, DuplicateFileCallbackInfo>(ref data, out var callback, out var callbackInfo))
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

	[MonoPInvokeCallback(typeof(OnWriteFileCompleteCallbackInternal))]
	internal static void OnWriteFileCompleteCallbackInternalImplementation(ref WriteFileCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<WriteFileCallbackInfoInternal, OnWriteFileCompleteCallback, WriteFileCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnWriteFileDataCallbackInternal))]
	internal static WriteResult OnWriteFileDataCallbackInternalImplementation(ref WriteFileDataCallbackInfoInternal data, IntPtr outDataBuffer, ref uint outDataWritten)
	{
		if (Helper.TryGetStructCallback<WriteFileDataCallbackInfoInternal, OnWriteFileDataCallback, WriteFileDataCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			ArraySegment<byte> outDataBufferArray;
			WriteResult result = callback(ref callbackInfo, out outDataBufferArray);
			Helper.Get(outDataBufferArray, out outDataWritten);
			Helper.Copy(outDataBufferArray, outDataBuffer);
			return result;
		}
		return Helper.GetDefault<WriteResult>();
	}
}

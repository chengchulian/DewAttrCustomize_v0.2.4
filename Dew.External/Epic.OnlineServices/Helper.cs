using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Epic.OnlineServices;

public sealed class Helper
{
	private struct Allocation
	{
		public int Size { get; private set; }

		public object Cache { get; private set; }

		public bool? IsArrayItemAllocated { get; private set; }

		public Allocation(int size, object cache, bool? isArrayItemAllocated = null)
		{
			Size = size;
			Cache = cache;
			IsArrayItemAllocated = isArrayItemAllocated;
		}
	}

	private struct PinnedBuffer
	{
		public GCHandle Handle { get; private set; }

		public int RefCount { get; set; }

		public PinnedBuffer(GCHandle handle)
		{
			Handle = handle;
			RefCount = 1;
		}
	}

	private class DelegateHolder
	{
		public Delegate Public { get; private set; }

		public Delegate Private { get; private set; }

		public Delegate[] StructDelegates { get; private set; }

		public ulong? NotificationId { get; set; }

		public DelegateHolder(Delegate publicDelegate, Delegate privateDelegate, params Delegate[] structDelegates)
		{
			Public = publicDelegate;
			Private = privateDelegate;
			StructDelegates = structDelegates;
		}
	}

	private static Dictionary<IntPtr, Allocation> s_Allocations = new Dictionary<IntPtr, Allocation>();

	private static Dictionary<IntPtr, PinnedBuffer> s_PinnedBuffers = new Dictionary<IntPtr, PinnedBuffer>();

	private static Dictionary<IntPtr, DelegateHolder> s_Callbacks = new Dictionary<IntPtr, DelegateHolder>();

	private static Dictionary<string, DelegateHolder> s_StaticCallbacks = new Dictionary<string, DelegateHolder>();

	private static long s_LastClientDataId = 0L;

	private static Dictionary<IntPtr, object> s_ClientDatas = new Dictionary<IntPtr, object>();

	internal static void AddCallback(out IntPtr clientDataAddress, object clientData, Delegate publicDelegate, Delegate privateDelegate, params Delegate[] structDelegates)
	{
		lock (s_Callbacks)
		{
			clientDataAddress = AddClientData(clientData);
			s_Callbacks.Add(clientDataAddress, new DelegateHolder(publicDelegate, privateDelegate, structDelegates));
		}
	}

	private static void RemoveCallback(IntPtr clientDataAddress)
	{
		lock (s_Callbacks)
		{
			s_Callbacks.Remove(clientDataAddress);
			RemoveClientData(clientDataAddress);
		}
	}

	internal static bool TryGetCallback<TCallbackInfoInternal, TCallback, TCallbackInfo>(ref TCallbackInfoInternal callbackInfoInternal, out TCallback callback, out TCallbackInfo callbackInfo) where TCallbackInfoInternal : struct, ICallbackInfoInternal, IGettable<TCallbackInfo> where TCallback : class where TCallbackInfo : struct, ICallbackInfo
	{
		Get<TCallbackInfoInternal, TCallbackInfo>(ref callbackInfoInternal, out callbackInfo, out var clientDataAddress);
		callback = null;
		lock (s_Callbacks)
		{
			if (s_Callbacks.TryGetValue(clientDataAddress, out var delegateHolder))
			{
				callback = delegateHolder.Public as TCallback;
				return callback != null;
			}
		}
		return false;
	}

	internal static bool TryGetAndRemoveCallback<TCallbackInfoInternal, TCallback, TCallbackInfo>(ref TCallbackInfoInternal callbackInfoInternal, out TCallback callback, out TCallbackInfo callbackInfo) where TCallbackInfoInternal : struct, ICallbackInfoInternal, IGettable<TCallbackInfo> where TCallback : class where TCallbackInfo : struct, ICallbackInfo
	{
		Get<TCallbackInfoInternal, TCallbackInfo>(ref callbackInfoInternal, out callbackInfo, out var clientDataAddress);
		callback = null;
		lock (s_Callbacks)
		{
			if (s_Callbacks.TryGetValue(clientDataAddress, out var delegateHolder))
			{
				callback = delegateHolder.Public as TCallback;
				if (callback != null)
				{
					if (!delegateHolder.NotificationId.HasValue && callbackInfo.GetResultCode().HasValue && Common.IsOperationComplete(callbackInfo.GetResultCode().Value))
					{
						RemoveCallback(clientDataAddress);
					}
					return true;
				}
			}
		}
		return false;
	}

	internal static bool TryGetStructCallback<TCallbackInfoInternal, TCallback, TCallbackInfo>(ref TCallbackInfoInternal callbackInfoInternal, out TCallback callback, out TCallbackInfo callbackInfo) where TCallbackInfoInternal : struct, ICallbackInfoInternal, IGettable<TCallbackInfo> where TCallback : class where TCallbackInfo : struct
	{
		Get<TCallbackInfoInternal, TCallbackInfo>(ref callbackInfoInternal, out callbackInfo, out var clientDataAddress);
		callback = null;
		lock (s_Callbacks)
		{
			if (s_Callbacks.TryGetValue(clientDataAddress, out var delegateHolder))
			{
				callback = delegateHolder.StructDelegates.FirstOrDefault((Delegate structDelegate) => structDelegate.GetType() == typeof(TCallback)) as TCallback;
				if (callback != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	internal static void RemoveCallbackByNotificationId(ulong notificationId)
	{
		lock (s_Callbacks)
		{
			KeyValuePair<IntPtr, DelegateHolder>[] array = s_Callbacks.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<IntPtr, DelegateHolder> p = array[i];
				if (p.Value.NotificationId.HasValue && p.Value.NotificationId == notificationId)
				{
					RemoveCallback(p.Key);
				}
			}
		}
	}

	internal static void AddStaticCallback(string key, Delegate publicDelegate, Delegate privateDelegate)
	{
		lock (s_StaticCallbacks)
		{
			s_StaticCallbacks.Remove(key);
			s_StaticCallbacks.Add(key, new DelegateHolder(publicDelegate, privateDelegate));
		}
	}

	internal static bool TryGetStaticCallback<TCallback>(string key, out TCallback callback) where TCallback : class
	{
		callback = null;
		lock (s_StaticCallbacks)
		{
			if (s_StaticCallbacks.TryGetValue(key, out var delegateHolder))
			{
				callback = delegateHolder.Public as TCallback;
				if (callback != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	internal static void AssignNotificationIdToCallback(IntPtr clientDataAddress, ulong notificationId)
	{
		if (notificationId == 0L)
		{
			RemoveCallback(clientDataAddress);
			return;
		}
		lock (s_Callbacks)
		{
			if (s_Callbacks.TryGetValue(clientDataAddress, out var delegateHolder))
			{
				delegateHolder.NotificationId = notificationId;
			}
		}
	}

	private static IntPtr AddClientData(object clientData)
	{
		lock (s_ClientDatas)
		{
			IntPtr clientDataAddress = new IntPtr(++s_LastClientDataId);
			s_ClientDatas.Add(clientDataAddress, clientData);
			return clientDataAddress;
		}
	}

	private static void RemoveClientData(IntPtr clientDataAddress)
	{
		lock (s_ClientDatas)
		{
			s_ClientDatas.Remove(clientDataAddress);
		}
	}

	private static object GetClientData(IntPtr clientDataAddress)
	{
		lock (s_ClientDatas)
		{
			s_ClientDatas.TryGetValue(clientDataAddress, out var clientData);
			return clientData;
		}
	}

	private static void Convert<THandle>(IntPtr from, out THandle to) where THandle : Handle, new()
	{
		to = null;
		if (from != IntPtr.Zero)
		{
			to = new THandle();
			to.InnerHandle = from;
		}
	}

	private static void Convert(Handle from, out IntPtr to)
	{
		to = IntPtr.Zero;
		if (from != null)
		{
			to = from.InnerHandle;
		}
	}

	private static void Convert(byte[] from, out string to)
	{
		to = null;
		if (from != null)
		{
			int len = GetAnsiStringLength(from);
			to = Encoding.ASCII.GetString(from[..len]);
		}
	}

	private static void Convert(string from, out byte[] to, int fromLength)
	{
		if (from == null)
		{
			from = "";
		}
		if (fromLength >= from.Length)
		{
			to = Encoding.ASCII.GetBytes(from.PadRight(fromLength, '\0'));
		}
		else
		{
			to = Encoding.ASCII.GetBytes(new string(from[..fromLength]).PadRight(fromLength, '\0'));
		}
	}

	private static void Convert<TArray>(TArray[] from, out int to)
	{
		to = 0;
		if (from != null)
		{
			to = from.Length;
		}
	}

	private static void Convert<TArray>(TArray[] from, out uint to)
	{
		to = 0u;
		if (from != null)
		{
			to = (uint)from.Length;
		}
	}

	private static void Convert<TArray>(ArraySegment<TArray> from, out int to)
	{
		to = from.Count;
	}

	private static void Convert<T>(ArraySegment<T> from, out uint to)
	{
		to = (uint)from.Count;
	}

	private static void Convert(int from, out bool to)
	{
		to = from != 0;
	}

	private static void Convert(bool from, out int to)
	{
		to = (from ? 1 : 0);
	}

	private static void Convert(DateTimeOffset? from, out long to)
	{
		to = -1L;
		if (from.HasValue)
		{
			DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			long unixTimestampSeconds = (from.Value.UtcDateTime - unixStart).Ticks / 10000000;
			to = unixTimestampSeconds;
		}
	}

	private static void Convert(long from, out DateTimeOffset? to)
	{
		to = null;
		if (from >= 0)
		{
			DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			long unixTimeStampTicks = from * 10000000;
			to = new DateTimeOffset(unixStart.Ticks + unixTimeStampTicks, TimeSpan.Zero);
		}
	}

	internal static void Get<TArray>(TArray[] from, out int to)
	{
		Convert(from, out to);
	}

	internal static void Get<TArray>(TArray[] from, out uint to)
	{
		Convert(from, out to);
	}

	internal static void Get<TArray>(ArraySegment<TArray> from, out uint to)
	{
		Convert(from, out to);
	}

	internal static void Get<TTo>(IntPtr from, out TTo to) where TTo : Handle, new()
	{
		Convert<TTo>(from, out to);
	}

	internal static void Get<TFrom, TTo>(ref TFrom from, out TTo to) where TFrom : struct, IGettable<TTo> where TTo : struct
	{
		from.Get(out to);
	}

	internal static void Get(int from, out bool to)
	{
		Convert(from, out to);
	}

	internal static void Get(bool from, out int to)
	{
		Convert(from, out to);
	}

	internal static void Get(long from, out DateTimeOffset? to)
	{
		Convert(from, out to);
	}

	internal static void Get<TTo>(IntPtr from, out TTo[] to, int arrayLength, bool isArrayItemAllocated)
	{
		GetAllocation<TTo>(from, out to, arrayLength, isArrayItemAllocated);
	}

	internal static void Get<TTo>(IntPtr from, out TTo[] to, uint arrayLength, bool isArrayItemAllocated)
	{
		GetAllocation<TTo>(from, out to, (int)arrayLength, isArrayItemAllocated);
	}

	internal static void Get<TTo>(IntPtr from, out TTo[] to, int arrayLength)
	{
		GetAllocation<TTo>(from, out to, arrayLength, !typeof(TTo).IsValueType);
	}

	internal static void Get<TTo>(IntPtr from, out TTo[] to, uint arrayLength)
	{
		GetAllocation<TTo>(from, out to, (int)arrayLength, !typeof(TTo).IsValueType);
	}

	internal static void Get(IntPtr from, out ArraySegment<byte> to, uint arrayLength)
	{
		to = default(ArraySegment<byte>);
		if (arrayLength != 0)
		{
			byte[] bytes = new byte[arrayLength];
			Marshal.Copy(from, bytes, 0, (int)arrayLength);
			to = new ArraySegment<byte>(bytes);
		}
	}

	internal static void GetHandle<THandle>(IntPtr from, out THandle[] to, uint arrayLength) where THandle : Handle, new()
	{
		GetAllocation<THandle>(from, out to, (int)arrayLength);
	}

	internal static void Get<TFrom, TTo>(TFrom[] from, out TTo[] to) where TFrom : struct, IGettable<TTo> where TTo : struct
	{
		to = GetDefault<TTo[]>();
		if (from != null)
		{
			to = new TTo[from.Length];
			for (int index = 0; index < from.Length; index++)
			{
				from[index].Get(out to[index]);
			}
		}
	}

	internal static void Get<TFrom, TTo>(IntPtr from, out TTo[] to, int arrayLength) where TFrom : struct, IGettable<TTo> where TTo : struct
	{
		Get(from, out TFrom[] fromIntermediate, arrayLength);
		Get(fromIntermediate, out to);
	}

	internal static void Get<TFrom, TTo>(IntPtr from, out TTo[] to, uint arrayLength) where TFrom : struct, IGettable<TTo> where TTo : struct
	{
		Get<TFrom, TTo>(from, out to, (int)arrayLength);
	}

	internal static void Get<TTo>(IntPtr from, out TTo? to) where TTo : struct
	{
		GetAllocation(from, out to);
	}

	internal static void Get(byte[] from, out string to)
	{
		Convert(from, out to);
	}

	internal static void Get(IntPtr from, out object to)
	{
		to = GetClientData(from);
	}

	internal static void Get(IntPtr from, out Utf8String to)
	{
		GetAllocation(from, out to);
	}

	internal static void Get<T, TEnum>(T from, out T to, TEnum currentEnum, TEnum expectedEnum)
	{
		to = GetDefault<T>();
		if ((int)(object)currentEnum == (int)(object)expectedEnum)
		{
			to = from;
		}
	}

	internal static void Get<TFrom, TTo, TEnum>(ref TFrom from, out TTo to, TEnum currentEnum, TEnum expectedEnum) where TFrom : struct, IGettable<TTo> where TTo : struct
	{
		to = GetDefault<TTo>();
		if ((int)(object)currentEnum == (int)(object)expectedEnum)
		{
			Get<TFrom, TTo>(ref from, out to);
		}
	}

	internal static void Get<TEnum>(int from, out bool? to, TEnum currentEnum, TEnum expectedEnum)
	{
		to = GetDefault<bool?>();
		if ((int)(object)currentEnum == (int)(object)expectedEnum)
		{
			Convert(from, out var fromIntermediate);
			to = fromIntermediate;
		}
	}

	internal static void Get<TFrom, TEnum>(TFrom from, out TFrom? to, TEnum currentEnum, TEnum expectedEnum) where TFrom : struct
	{
		to = GetDefault<TFrom?>();
		if ((int)(object)currentEnum == (int)(object)expectedEnum)
		{
			to = from;
		}
	}

	internal static void Get<TFrom, TEnum>(IntPtr from, out TFrom to, TEnum currentEnum, TEnum expectedEnum) where TFrom : Handle, new()
	{
		to = GetDefault<TFrom>();
		if ((int)(object)currentEnum == (int)(object)expectedEnum)
		{
			Get(from, out to);
		}
	}

	internal static void Get<TEnum>(IntPtr from, out IntPtr? to, TEnum currentEnum, TEnum expectedEnum)
	{
		to = GetDefault<IntPtr?>();
		if ((int)(object)currentEnum == (int)(object)expectedEnum)
		{
			Get(from, out to);
		}
	}

	internal static void Get<TEnum>(IntPtr from, out Utf8String to, TEnum currentEnum, TEnum expectedEnum)
	{
		to = GetDefault<Utf8String>();
		if ((int)(object)currentEnum == (int)(object)expectedEnum)
		{
			Get(from, out to);
		}
	}

	internal static void Get<TFrom, TTo>(IntPtr from, out TTo to) where TFrom : struct, IGettable<TTo> where TTo : struct
	{
		to = GetDefault<TTo>();
		Get(from, out TFrom? fromIntermediate);
		if (fromIntermediate.HasValue)
		{
			fromIntermediate.Value.Get(out to);
		}
	}

	internal static void Get<TFrom, TTo>(IntPtr from, out TTo? to) where TFrom : struct, IGettable<TTo> where TTo : struct
	{
		to = GetDefault<TTo?>();
		Get(from, out TFrom? fromIntermediate);
		if (fromIntermediate.HasValue)
		{
			fromIntermediate.Value.Get(out var toIntermediate);
			to = toIntermediate;
		}
	}

	internal static void Get<TFrom, TTo>(ref TFrom from, out TTo to, out IntPtr clientDataAddress) where TFrom : struct, ICallbackInfoInternal, IGettable<TTo> where TTo : struct
	{
		from.Get(out to);
		clientDataAddress = from.ClientDataAddress;
	}

	public static int GetAllocationCount()
	{
		return s_Allocations.Count + s_PinnedBuffers.Aggregate(0, (int acc, KeyValuePair<IntPtr, PinnedBuffer> x) => acc + x.Value.RefCount) + s_Callbacks.Count + s_ClientDatas.Count;
	}

	internal static void Copy(byte[] from, IntPtr to)
	{
		if (from != null && to != IntPtr.Zero)
		{
			Marshal.Copy(from, 0, to, from.Length);
		}
	}

	internal static void Copy(ArraySegment<byte> from, IntPtr to)
	{
		if (from.Count != 0 && to != IntPtr.Zero)
		{
			Marshal.Copy(from.Array, from.Offset, to, from.Count);
		}
	}

	internal static void Dispose(ref IntPtr value)
	{
		RemoveAllocation(ref value);
		RemovePinnedBuffer(ref value);
	}

	internal static void Dispose<TDisposable>(ref TDisposable disposable) where TDisposable : IDisposable
	{
		if (disposable != null)
		{
			disposable.Dispose();
		}
	}

	internal static void Dispose<TEnum>(ref IntPtr value, TEnum currentEnum, TEnum expectedEnum)
	{
		if ((int)(object)currentEnum == (int)(object)expectedEnum)
		{
			Dispose(ref value);
		}
	}

	private static int GetAnsiStringLength(byte[] bytes)
	{
		int length = 0;
		for (int i = 0; i < bytes.Length && bytes[i] != 0; i++)
		{
			length++;
		}
		return length;
	}

	private static int GetAnsiStringLength(IntPtr address)
	{
		int length;
		for (length = 0; Marshal.ReadByte(address, length) != 0; length++)
		{
		}
		return length;
	}

	internal static T GetDefault<T>()
	{
		return default(T);
	}

	private static void GetAllocation<T>(IntPtr source, out T target)
	{
		target = GetDefault<T>();
		if (source == IntPtr.Zero)
		{
			return;
		}
		if (TryGetAllocationCache(source, out var allocationCache) && allocationCache != null)
		{
			if (!(allocationCache.GetType() == typeof(T)))
			{
				throw new CachedTypeAllocationException(source, allocationCache.GetType(), typeof(T));
			}
			target = (T)allocationCache;
		}
		else
		{
			target = (T)Marshal.PtrToStructure(source, typeof(T));
		}
	}

	private static void GetAllocation<T>(IntPtr source, out T? target) where T : struct
	{
		target = GetDefault<T?>();
		if (source == IntPtr.Zero)
		{
			return;
		}
		if (TryGetAllocationCache(source, out var allocationCache) && allocationCache != null)
		{
			if (!(allocationCache.GetType() == typeof(T)))
			{
				throw new CachedTypeAllocationException(source, allocationCache.GetType(), typeof(T));
			}
			target = (T?)allocationCache;
		}
		else
		{
			target = (T?)Marshal.PtrToStructure(source, typeof(T));
		}
	}

	private static void GetAllocation<THandle>(IntPtr source, out THandle[] target, int arrayLength) where THandle : Handle, new()
	{
		target = null;
		if (source == IntPtr.Zero)
		{
			return;
		}
		if (TryGetAllocationCache(source, out var allocationCache) && allocationCache != null)
		{
			if (!(allocationCache.GetType() == typeof(THandle[])))
			{
				throw new CachedTypeAllocationException(source, allocationCache.GetType(), typeof(THandle[]));
			}
			Array cachedArray = (Array)allocationCache;
			if (cachedArray.Length != arrayLength)
			{
				throw new CachedArrayAllocationException(source, cachedArray.Length, arrayLength);
			}
			target = cachedArray as THandle[];
		}
		else
		{
			int itemSize = Marshal.SizeOf(typeof(IntPtr));
			List<THandle> items = new List<THandle>();
			for (int itemIndex = 0; itemIndex < arrayLength; itemIndex++)
			{
				Convert<THandle>(Marshal.ReadIntPtr(new IntPtr(source.ToInt64() + itemIndex * itemSize)), out var item);
				items.Add(item);
			}
			target = items.ToArray();
		}
	}

	private static void GetAllocation<T>(IntPtr from, out T[] to, int arrayLength, bool isArrayItemAllocated)
	{
		to = null;
		if (from == IntPtr.Zero)
		{
			return;
		}
		if (TryGetAllocationCache(from, out var allocationCache) && allocationCache != null)
		{
			if (allocationCache.GetType() == typeof(T[]))
			{
				Array cachedArray = (Array)allocationCache;
				if (cachedArray.Length == arrayLength)
				{
					to = cachedArray as T[];
					return;
				}
				throw new CachedArrayAllocationException(from, cachedArray.Length, arrayLength);
			}
			throw new CachedTypeAllocationException(from, allocationCache.GetType(), typeof(T[]));
		}
		int itemSize = ((!isArrayItemAllocated) ? Marshal.SizeOf(typeof(T)) : Marshal.SizeOf(typeof(IntPtr)));
		List<T> items = new List<T>();
		for (int itemIndex = 0; itemIndex < arrayLength; itemIndex++)
		{
			IntPtr itemAddress = new IntPtr(from.ToInt64() + itemIndex * itemSize);
			if (isArrayItemAllocated)
			{
				itemAddress = Marshal.ReadIntPtr(itemAddress);
			}
			T item;
			if (typeof(T) == typeof(Utf8String))
			{
				GetAllocation(itemAddress, out var str);
				item = (T)(object)str;
			}
			else
			{
				GetAllocation(itemAddress, out item);
			}
			items.Add(item);
		}
		to = items.ToArray();
	}

	private static void GetAllocation(IntPtr source, out Utf8String target)
	{
		target = null;
		if (!(source == IntPtr.Zero))
		{
			int length = GetAnsiStringLength(source);
			byte[] bytes = new byte[length + 1];
			Marshal.Copy(source, bytes, 0, length + 1);
			target = new Utf8String(bytes);
		}
	}

	internal static IntPtr AddAllocation(int size)
	{
		if (size == 0)
		{
			return IntPtr.Zero;
		}
		IntPtr address = Marshal.AllocHGlobal(size);
		Marshal.WriteByte(address, 0, 0);
		lock (s_Allocations)
		{
			s_Allocations.Add(address, new Allocation(size, null, null));
			return address;
		}
	}

	internal static IntPtr AddAllocation(uint size)
	{
		return AddAllocation((int)size);
	}

	private static IntPtr AddAllocation<T>(int size, T cache)
	{
		if (size == 0 || cache == null)
		{
			return IntPtr.Zero;
		}
		IntPtr address = Marshal.AllocHGlobal(size);
		Marshal.StructureToPtr(cache, address, fDeleteOld: false);
		lock (s_Allocations)
		{
			s_Allocations.Add(address, new Allocation(size, cache, null));
			return address;
		}
	}

	private static IntPtr AddAllocation<T>(int size, T[] cache, bool? isArrayItemAllocated)
	{
		if (size == 0 || cache == null)
		{
			return IntPtr.Zero;
		}
		IntPtr address = Marshal.AllocHGlobal(size);
		Marshal.WriteByte(address, 0, 0);
		lock (s_Allocations)
		{
			s_Allocations.Add(address, new Allocation(size, cache, isArrayItemAllocated));
			return address;
		}
	}

	private static IntPtr AddAllocation<T>(T[] array, bool isArrayItemAllocated)
	{
		if (array == null)
		{
			return IntPtr.Zero;
		}
		int itemSize = ((!isArrayItemAllocated) ? Marshal.SizeOf(typeof(T)) : Marshal.SizeOf(typeof(IntPtr)));
		IntPtr newArrayAddress = AddAllocation(array.Length * itemSize, array, isArrayItemAllocated);
		for (int itemIndex = 0; itemIndex < array.Length; itemIndex++)
		{
			T item = (T)array.GetValue(itemIndex);
			if (isArrayItemAllocated)
			{
				IntPtr newItemAddress;
				if (typeof(T) == typeof(Utf8String))
				{
					newItemAddress = AddPinnedBuffer((Utf8String)(object)item);
				}
				else if (typeof(T).BaseType == typeof(Handle))
				{
					Convert((Handle)(object)item, out newItemAddress);
				}
				else
				{
					newItemAddress = AddAllocation(Marshal.SizeOf(typeof(T)), item);
				}
				Marshal.StructureToPtr(ptr: new IntPtr(newArrayAddress.ToInt64() + itemIndex * itemSize), structure: newItemAddress, fDeleteOld: false);
			}
			else
			{
				IntPtr itemAddress = new IntPtr(newArrayAddress.ToInt64() + itemIndex * itemSize);
				Marshal.StructureToPtr(item, itemAddress, fDeleteOld: false);
			}
		}
		return newArrayAddress;
	}

	private static void RemoveAllocation(ref IntPtr address)
	{
		if (address == IntPtr.Zero)
		{
			return;
		}
		Allocation allocation;
		lock (s_Allocations)
		{
			if (!s_Allocations.TryGetValue(address, out allocation))
			{
				return;
			}
			s_Allocations.Remove(address);
		}
		if (allocation.IsArrayItemAllocated.HasValue)
		{
			int itemSize = ((!allocation.IsArrayItemAllocated.Value) ? Marshal.SizeOf(allocation.Cache.GetType().GetElementType()) : Marshal.SizeOf(typeof(IntPtr)));
			Array array = allocation.Cache as Array;
			for (int itemIndex = 0; itemIndex < array.Length; itemIndex++)
			{
				if (allocation.IsArrayItemAllocated.Value)
				{
					IntPtr itemAddress = new IntPtr(address.ToInt64() + itemIndex * itemSize);
					itemAddress = Marshal.ReadIntPtr(itemAddress);
					Dispose(ref itemAddress);
					continue;
				}
				object item = array.GetValue(itemIndex);
				if (item is IDisposable && item is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
		}
		if (allocation.Cache is IDisposable && allocation.Cache is IDisposable disposable2)
		{
			disposable2.Dispose();
		}
		Marshal.FreeHGlobal(address);
		address = IntPtr.Zero;
	}

	private static bool TryGetAllocationCache(IntPtr address, out object cache)
	{
		cache = null;
		lock (s_Allocations)
		{
			if (s_Allocations.TryGetValue(address, out var allocation))
			{
				cache = allocation.Cache;
				return true;
			}
		}
		return false;
	}

	private static IntPtr AddPinnedBuffer(byte[] buffer, int offset)
	{
		if (buffer == null)
		{
			return IntPtr.Zero;
		}
		GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
		IntPtr address = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
		lock (s_PinnedBuffers)
		{
			if (s_PinnedBuffers.ContainsKey(address))
			{
				PinnedBuffer pinned = s_PinnedBuffers[address];
				pinned.RefCount++;
				s_PinnedBuffers[address] = pinned;
			}
			else
			{
				s_PinnedBuffers.Add(address, new PinnedBuffer(handle));
			}
			return address;
		}
	}

	private static IntPtr AddPinnedBuffer(Utf8String str)
	{
		if (str == null || str.Bytes == null)
		{
			return IntPtr.Zero;
		}
		return AddPinnedBuffer(str.Bytes, 0);
	}

	internal static IntPtr AddPinnedBuffer(ArraySegment<byte> array)
	{
		if (array == null)
		{
			return IntPtr.Zero;
		}
		return AddPinnedBuffer(array.Array, array.Offset);
	}

	private static void RemovePinnedBuffer(ref IntPtr address)
	{
		if (address == IntPtr.Zero)
		{
			return;
		}
		lock (s_PinnedBuffers)
		{
			if (s_PinnedBuffers.TryGetValue(address, out var pinnedBuffer))
			{
				pinnedBuffer.RefCount--;
				if (pinnedBuffer.RefCount == 0)
				{
					s_PinnedBuffers.Remove(address);
					pinnedBuffer.Handle.Free();
				}
				else
				{
					s_PinnedBuffers[address] = pinnedBuffer;
				}
			}
		}
		address = IntPtr.Zero;
	}

	internal static void Set<T>(ref T from, ref T to) where T : struct
	{
		to = from;
	}

	internal static void Set(object from, ref IntPtr to)
	{
		RemoveClientData(to);
		to = AddClientData(from);
	}

	internal static void Set(Utf8String from, ref IntPtr to)
	{
		Dispose(ref to);
		to = AddPinnedBuffer(from);
	}

	internal static void Set(Handle from, ref IntPtr to)
	{
		Convert(from, out to);
	}

	internal static void Set<T>(T? from, ref IntPtr to) where T : struct
	{
		Dispose(ref to);
		to = AddAllocation(Marshal.SizeOf(typeof(T)), from);
	}

	internal static void Set<T>(T[] from, ref IntPtr to, bool isArrayItemAllocated)
	{
		Dispose(ref to);
		to = AddAllocation(from, isArrayItemAllocated);
	}

	internal static void Set(ArraySegment<byte> from, ref IntPtr to, out uint arrayLength)
	{
		to = AddPinnedBuffer(from);
		Get(from, out arrayLength);
	}

	internal static void Set<T>(T[] from, ref IntPtr to)
	{
		Set(from, ref to, !typeof(T).IsValueType);
	}

	internal static void Set<T>(T[] from, ref IntPtr to, bool isArrayItemAllocated, out int arrayLength)
	{
		Set(from, ref to, isArrayItemAllocated);
		Get(from, out arrayLength);
	}

	internal static void Set<T>(T[] from, ref IntPtr to, bool isArrayItemAllocated, out uint arrayLength)
	{
		Set(from, ref to, isArrayItemAllocated);
		Get(from, out arrayLength);
	}

	internal static void Set<T>(T[] from, ref IntPtr to, out int arrayLength)
	{
		Set(from, ref to, !typeof(T).IsValueType, out arrayLength);
	}

	internal static void Set<T>(T[] from, ref IntPtr to, out uint arrayLength)
	{
		Set(from, ref to, !typeof(T).IsValueType, out arrayLength);
	}

	internal static void Set(DateTimeOffset? from, ref long to)
	{
		Convert(from, out to);
	}

	internal static void Set(bool from, ref int to)
	{
		Convert(from, out to);
	}

	internal static void Set(string from, ref byte[] to, int stringLength)
	{
		Convert(from, out to, stringLength);
	}

	internal static void Set<T, TEnum>(T from, ref T to, TEnum fromEnum, ref TEnum toEnum, IDisposable disposable = null)
	{
		if (from != null)
		{
			Dispose(ref disposable);
			to = from;
			toEnum = fromEnum;
		}
	}

	internal static void Set<TFrom, TEnum, TTo>(ref TFrom from, ref TTo to, TEnum fromEnum, ref TEnum toEnum, IDisposable disposable = null) where TFrom : struct where TTo : struct, ISettable<TFrom>
	{
		Dispose(ref disposable);
		Set(ref from, ref to);
		toEnum = fromEnum;
	}

	internal static void Set<T, TEnum>(T? from, ref T to, TEnum fromEnum, ref TEnum toEnum, IDisposable disposable = null) where T : struct
	{
		if (from.HasValue)
		{
			Dispose(ref disposable);
			T value = from.Value;
			Helper.Set<T>(ref value, ref to);
			toEnum = fromEnum;
		}
	}

	internal static void Set<TEnum>(Handle from, ref IntPtr to, TEnum fromEnum, ref TEnum toEnum, IDisposable disposable = null)
	{
		if (from != null)
		{
			Dispose(ref to);
			Dispose(ref disposable);
			Set(from, ref to);
			toEnum = fromEnum;
		}
	}

	internal static void Set<TEnum>(Utf8String from, ref IntPtr to, TEnum fromEnum, ref TEnum toEnum, IDisposable disposable = null)
	{
		if (from != null)
		{
			Dispose(ref to);
			Dispose(ref disposable);
			Set(from, ref to);
			toEnum = fromEnum;
		}
	}

	internal static void Set<TEnum>(bool? from, ref int to, TEnum fromEnum, ref TEnum toEnum, IDisposable disposable = null)
	{
		if (from.HasValue)
		{
			Dispose(ref disposable);
			Set(from.Value, ref to);
			toEnum = fromEnum;
		}
	}

	internal static void Set<TFrom, TIntermediate>(ref TFrom from, ref IntPtr to) where TFrom : struct where TIntermediate : struct, ISettable<TFrom>
	{
		TIntermediate intermediate = new TIntermediate();
		intermediate.Set(ref from);
		Dispose(ref to);
		to = AddAllocation(Marshal.SizeOf(typeof(TIntermediate)), intermediate);
	}

	internal static void Set<TFrom, TIntermediate>(ref TFrom? from, ref IntPtr to) where TFrom : struct where TIntermediate : struct, ISettable<TFrom>
	{
		Dispose(ref to);
		if (from.HasValue)
		{
			TIntermediate intermediate = new TIntermediate();
			TFrom sourceValue = from.Value;
			intermediate.Set(ref sourceValue);
			to = AddAllocation(Marshal.SizeOf(typeof(TIntermediate)), intermediate);
		}
	}

	internal static void Set<TFrom, TTo>(ref TFrom from, ref TTo to) where TFrom : struct where TTo : struct, ISettable<TFrom>
	{
		to.Set(ref from);
	}

	internal static void Set<TFrom, TIntermediate>(ref TFrom[] from, ref IntPtr to, out int arrayLength) where TFrom : struct where TIntermediate : struct, ISettable<TFrom>
	{
		arrayLength = 0;
		if (from != null)
		{
			TIntermediate[] intermediate = new TIntermediate[from.Length];
			for (int index = 0; index < from.Length; index++)
			{
				intermediate[index].Set(ref from[index]);
			}
			Set(intermediate, ref to);
			Get(from, out arrayLength);
		}
	}

	internal static void Set<TFrom, TIntermediate>(ref TFrom[] from, ref IntPtr to, out uint arrayLength) where TFrom : struct where TIntermediate : struct, ISettable<TFrom>
	{
		Set<TFrom, TIntermediate>(ref from, ref to, out int arrayLengthIntermediate);
		arrayLength = (uint)arrayLengthIntermediate;
	}
}

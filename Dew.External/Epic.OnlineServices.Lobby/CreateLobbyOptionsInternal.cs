using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CreateLobbyOptionsInternal : ISettable<CreateLobbyOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private uint m_MaxLobbyMembers;

	private LobbyPermissionLevel m_PermissionLevel;

	private int m_PresenceEnabled;

	private int m_AllowInvites;

	private IntPtr m_BucketId;

	private int m_DisableHostMigration;

	private int m_EnableRTCRoom;

	private IntPtr m_LocalRTCOptions;

	private IntPtr m_LobbyId;

	private int m_EnableJoinById;

	private int m_RejoinAfterKickRequiresInvite;

	private IntPtr m_AllowedPlatformIds;

	private uint m_AllowedPlatformIdsCount;

	private int m_CrossplayOptOut;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public uint MaxLobbyMembers
	{
		set
		{
			m_MaxLobbyMembers = value;
		}
	}

	public LobbyPermissionLevel PermissionLevel
	{
		set
		{
			m_PermissionLevel = value;
		}
	}

	public bool PresenceEnabled
	{
		set
		{
			Helper.Set(value, ref m_PresenceEnabled);
		}
	}

	public bool AllowInvites
	{
		set
		{
			Helper.Set(value, ref m_AllowInvites);
		}
	}

	public Utf8String BucketId
	{
		set
		{
			Helper.Set(value, ref m_BucketId);
		}
	}

	public bool DisableHostMigration
	{
		set
		{
			Helper.Set(value, ref m_DisableHostMigration);
		}
	}

	public bool EnableRTCRoom
	{
		set
		{
			Helper.Set(value, ref m_EnableRTCRoom);
		}
	}

	public LocalRTCOptions? LocalRTCOptions
	{
		set
		{
			Helper.Set<LocalRTCOptions, LocalRTCOptionsInternal>(ref value, ref m_LocalRTCOptions);
		}
	}

	public Utf8String LobbyId
	{
		set
		{
			Helper.Set(value, ref m_LobbyId);
		}
	}

	public bool EnableJoinById
	{
		set
		{
			Helper.Set(value, ref m_EnableJoinById);
		}
	}

	public bool RejoinAfterKickRequiresInvite
	{
		set
		{
			Helper.Set(value, ref m_RejoinAfterKickRequiresInvite);
		}
	}

	public uint[] AllowedPlatformIds
	{
		set
		{
			Helper.Set(value, ref m_AllowedPlatformIds, out m_AllowedPlatformIdsCount);
		}
	}

	public bool CrossplayOptOut
	{
		set
		{
			Helper.Set(value, ref m_CrossplayOptOut);
		}
	}

	public void Set(ref CreateLobbyOptions other)
	{
		m_ApiVersion = 9;
		LocalUserId = other.LocalUserId;
		MaxLobbyMembers = other.MaxLobbyMembers;
		PermissionLevel = other.PermissionLevel;
		PresenceEnabled = other.PresenceEnabled;
		AllowInvites = other.AllowInvites;
		BucketId = other.BucketId;
		DisableHostMigration = other.DisableHostMigration;
		EnableRTCRoom = other.EnableRTCRoom;
		LocalRTCOptions = other.LocalRTCOptions;
		LobbyId = other.LobbyId;
		EnableJoinById = other.EnableJoinById;
		RejoinAfterKickRequiresInvite = other.RejoinAfterKickRequiresInvite;
		AllowedPlatformIds = other.AllowedPlatformIds;
		CrossplayOptOut = other.CrossplayOptOut;
	}

	public void Set(ref CreateLobbyOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 9;
			LocalUserId = other.Value.LocalUserId;
			MaxLobbyMembers = other.Value.MaxLobbyMembers;
			PermissionLevel = other.Value.PermissionLevel;
			PresenceEnabled = other.Value.PresenceEnabled;
			AllowInvites = other.Value.AllowInvites;
			BucketId = other.Value.BucketId;
			DisableHostMigration = other.Value.DisableHostMigration;
			EnableRTCRoom = other.Value.EnableRTCRoom;
			LocalRTCOptions = other.Value.LocalRTCOptions;
			LobbyId = other.Value.LobbyId;
			EnableJoinById = other.Value.EnableJoinById;
			RejoinAfterKickRequiresInvite = other.Value.RejoinAfterKickRequiresInvite;
			AllowedPlatformIds = other.Value.AllowedPlatformIds;
			CrossplayOptOut = other.Value.CrossplayOptOut;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_BucketId);
		Helper.Dispose(ref m_LocalRTCOptions);
		Helper.Dispose(ref m_LobbyId);
		Helper.Dispose(ref m_AllowedPlatformIds);
	}
}

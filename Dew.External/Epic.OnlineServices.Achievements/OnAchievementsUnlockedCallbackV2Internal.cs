using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void OnAchievementsUnlockedCallbackV2Internal(ref OnAchievementsUnlockedCallbackV2InfoInternal data);

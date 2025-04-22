using System;

namespace Epic.OnlineServices.UI;

[Flags]
public enum InputStateButtonFlags
{
	None = 0,
	DPadLeft = 1,
	DPadRight = 2,
	DPadDown = 4,
	DPadUp = 8,
	FaceButtonLeft = 0x10,
	FaceButtonRight = 0x20,
	FaceButtonBottom = 0x40,
	FaceButtonTop = 0x80,
	LeftShoulder = 0x100,
	RightShoulder = 0x200,
	LeftTrigger = 0x400,
	RightTrigger = 0x800,
	SpecialLeft = 0x1000,
	SpecialRight = 0x2000,
	LeftThumbstick = 0x4000
}

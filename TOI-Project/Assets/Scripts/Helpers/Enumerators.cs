public enum Layer
{
	Default = 0,
	Obstacles = 9,
	Pickupable = 10,
	Interactable = 11,
	Droppable = 12,
	Triggers = 13,
	Observable = 14,
	RaycastEndStop = -1
};

public enum PlayerMoveStatus { NotMoving, Crouching, Walking, Running, NotGrounded, Landing };

public enum CurveControlledBobCallbackType { Horizontal, Vertical };

public enum PlayerHand { Left, Right };

public enum SwitchState { Null, Up, Down };

public enum DropplableObject { None, GenericItem, Lamp, Lever, Picture_1, Picture_3 };
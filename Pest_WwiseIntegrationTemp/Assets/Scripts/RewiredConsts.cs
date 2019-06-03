// <auto-generated>
// Rewired Constants
// This list was generated on 4/19/2019 6:52:15 PM
// The list applies to only the Rewired Input Manager from which it was generated.
// If you use a different Rewired Input Manager, you will have to generate a new list.
// If you make changes to the exported items in the Rewired Input Manager, you will
// need to regenerate this list.
// </auto-generated>

namespace RewiredConsts {
    public static partial class Action {
        public static partial class Default {
        }
        public static partial class CharacterControl {
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling the playercharacter", friendlyName = "left right movement")]
            public const int MoveHorizontal = 0;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling the playercharacter", friendlyName = "forwards backwards movement")]
            public const int MoveVertical = 1;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling the playercharacter", friendlyName = "activation of sneaking mode")]
            public const int Sneak = 2;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling the playercharacter", friendlyName = "activation of runing mode")]
            public const int Run = 3;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling the playercharacter", friendlyName = "jump action")]
            public const int Jump = 4;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling the playercharacter", friendlyName = "melee attack action")]
            public const int Attack = 5;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling the playercharacter", friendlyName = "blocking defending action")]
            public const int Block = 6;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling the playercharacter", friendlyName = "interactig with objects action")]
            public const int Interact = 7;
        }
    }
    public static partial class Category {
        public const int Default = 0;
        public const int CharacterControl = 1;
    }
    public static partial class Layout {
        public static partial class Joystick {
            public const int Default = 0;
        }
        public static partial class Keyboard {
            public const int Default = 0;
        }
        public static partial class Mouse {
            public const int Default = 0;
        }
        public static partial class CustomController {
            public const int Default = 0;
        }
    }
    public static partial class Player {
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "System")]
        public const int System = 9999999;
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "Player Controller")]
        public const int Player0 = 0;
    }
}

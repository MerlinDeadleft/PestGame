// <auto-generated>
// Rewired Constants
// This list was generated on 12.08.2019 16:31:21
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
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling the playercharacter", friendlyName = "Moves camera left and right")]
            public const int LookHorizontal = 8;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling the playercharacter", friendlyName = "Moves camera up and down")]
            public const int LookVertical = 10;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling the playercharacter", friendlyName = "Resets the view if changed with mouse")]
            public const int ResetLook = 9;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling the playercharacter", friendlyName = "Action to pause the game")]
            public const int PauseGame = 14;
        }
        public static partial class TutorialControl {
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling tutorial flow", friendlyName = "Continue to next tutorial part")]
            public const int Continue = 11;
        }
        public static partial class UIControl {
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling UI Elements", friendlyName = "Up")]
            public const int Up = 15;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling UI Elements", friendlyName = "Down")]
            public const int Down = 16;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling UI Elements", friendlyName = "Right")]
            public const int Right = 17;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling UI Elements", friendlyName = "Left")]
            public const int Left = 18;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling UI Elements", friendlyName = "Accept")]
            public const int Accept = 19;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling UI Elements", friendlyName = "Decline")]
            public const int Decline = 20;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling UI Elements", friendlyName = "ResumeGame")]
            public const int ResumeGame = 21;
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling UI Elements", friendlyName = "AnyInputAction")]
            public const int Any = 22;
        }
        public static partial class LoadingScreenControl {
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Actions for controlling loading screen slides", friendlyName = "Show next loading screen illustration")]
            public const int NextIllustration = 12;
        }
    }
    public static partial class Category {
        public const int Default = 0;
        public const int CharacterControl = 1;
        public const int TutorialControl = 2;
        public const int UIControl = 3;
        public const int LoadingScreenControl = 4;
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

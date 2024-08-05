using Monocle;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using Celeste.Mod.Entities;

namespace Celeste.Mod.celeste_mod_e;

[TrackedAs(typeof(DirectionCrystal))]
[CustomEntity("celeste_mod_e/FallUpCrystal")]
class FallUpCrystal : DirectionCrystal {
	public static string idle = "CustomModECrystalUp";
	public static string flash = "CustomModECrystalUpFlash";
	public static string outline = "crystals/crystal_up/outline";

	public static FallingDirection direction = FallingDirection.UP;

	// the real constructor.
	public FallUpCrystal(Vector2 pos, bool one_use) : base(pos,one_use, direction, idle, outline, flash) {  }

	public FallUpCrystal() : this(Vector2.Zero, false) {  }

	public FallUpCrystal(EntityData data, Vector2 offset) : this( data.Position + offset, data.Bool("oneUse") ) {  }
}

[TrackedAs(typeof(DirectionCrystal))]
[CustomEntity("celeste_mod_e/FallDownCrystal")]
class FallDownCrystal : DirectionCrystal {
	public static string idle = "CustomModECrystalDown";
	public static string flash = "CustomModECrystalDownFlash";
	public static string outline = "crystals/crystal_down/outline";

	public static FallingDirection direction = FallingDirection.DOWN;

	// the real constructor.
	public FallDownCrystal(Vector2 pos, bool one_use) : base(pos,one_use, direction, idle, outline, flash) {  }

	public FallDownCrystal() : this(Vector2.Zero, false) {  }

	public FallDownCrystal(EntityData data, Vector2 offset) : this( data.Position + offset, data.Bool("oneUse") ) {  }
}

[TrackedAs(typeof(DirectionCrystal))]
[CustomEntity("celeste_mod_e/FallLeftCrystal")]
class FallLeftCrystal : DirectionCrystal {
	public static string idle = "CustomModECrystalLeft";
	public static string flash = "CustomModECrystalleftFlash";
	public static string outline = "crystals/crystal_Left/outline";

	public static FallingDirection direction = FallingDirection.LEFT;

	// the real constructor.
	public FallLeftCrystal(Vector2 pos, bool one_use) : base(pos,one_use, direction, idle, outline, flash) {  }

	public FallLeftCrystal() : this(Vector2.Zero, false) {  }

	public FallLeftCrystal(EntityData data, Vector2 offset) : this( data.Position + offset, data.Bool("oneUse") ) {  }
}

[TrackedAs(typeof(DirectionCrystal))]
[CustomEntity("celeste_mod_e/FallRightCrystal")]
class FallRightCrystal : DirectionCrystal {
	public static string idle ="CustomModECrystalRight";
	public static string flash = "CustomModECrystalRightFlash";
	public static string outline = "crystals/crystal_right/outline";

	public static FallingDirection direction = FallingDirection.RIGHT;

	// the real constructor.
	public FallRightCrystal(Vector2 pos, bool one_use) : base(pos,one_use, direction, idle, outline, flash) {  }

	public FallRightCrystal() : this(Vector2.Zero, false) {  }

	public FallRightCrystal(EntityData data, Vector2 offset) : this( data.Position + offset, data.Bool("oneUse") ) {  }
}
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.celeste_mod_e;

[TrackedAs(typeof(SpeedCrystal))]
[CustomEntity("celeste_mod_e/FallNormalSpeedCrystal")]
class FallNormalCrystal : SpeedCrystal {
	public static string idle = "CustomModESpeedNormal";
	public static string flash = "CustomModESpeedNormalFlash";
	public static string outline = "crystals/speed_normie/outline";

	public static SpeedType speed = SpeedType.Normal;

	// the real constructor.
	public FallNormalCrystal(Vector2 pos, bool one_use) : base(pos, speed, one_use, idle, outline, flash) {  }

	public FallNormalCrystal() : this(Vector2.Zero, false) {  }

	public FallNormalCrystal(EntityData data, Vector2 offset) : this( data.Position + offset, data.Bool("oneUse") ) {  }
}

[TrackedAs(typeof(SpeedCrystal))]
[CustomEntity("celeste_mod_e/FallFastSpeedCrystal")]
class FallFastCrystal : SpeedCrystal {
	public static string idle = "CustomModESpeedFaster";
	public static string flash = "CustomModESpeedFasterFlash";
	public static string outline = "crystals/speed_fast/outline";

	public static SpeedType speed = SpeedType.Faster;

	// the real constructor.
	public FallFastCrystal(Vector2 pos, bool one_use) : base(pos, speed, one_use, idle, outline, flash) {  }

	public FallFastCrystal() : this(Vector2.Zero, false) {  }

	public FallFastCrystal(EntityData data, Vector2 offset) : this( data.Position + offset, data.Bool("oneUse") ) {  }
}
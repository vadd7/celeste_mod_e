using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.celeste_mod_e;

class SpeedCrystal : DirectionCrystal {
	private Vector2 speed_multiply;
	private SpeedType _speed_type;
	public SpeedType speed_type {
		get => _speed_type;
		set {
			_speed_type = value;
			switch (value) {
				case SpeedType.Normal: speed_multiply = new(1.1f, 1f); break;
				case SpeedType.Faster: speed_multiply = new(1.35f, 1.3f); break;
			}
		}
	}

	public enum SpeedType {
		Normal,
		Faster
	}

	public SpeedCrystal(Vector2 position, SpeedType speed_multiply, bool oneuse, string spr, string outline, string flash ) 
		: base(position, oneuse, FallingDirection.OTHER, spr, outline, flash) { 
		
		speed_type = speed_multiply;

		// change particle colours.
		//switch (speed_multiply) {
		//	case SpeedType.Normal: p_shatter.Color = p_regen.Color = p_glow.Color = Color.LightGreen; p_shatter.Color2 = p_regen.Color2 = p_glow.Color2 = Color.Green; break;
		//	case SpeedType.Faster: p_shatter.Color = p_regen.Color = p_glow.Color = Color.LightPink; p_shatter.Color2 = p_regen.Color2 = p_glow.Color2 = Color.Purple; break;
		//}
	}

	public override void OnPlayer(Player player) {
		celeste_mod_eModule.enabled = true;

		if (celeste_mod_eModule.speed_multiplier != speed_multiply) {
			Audio.Play("event:/game/general/diamond_touch", Position);
			
			celeste_mod_eModule.speed_multiplier = speed_multiply;

			Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);
			Collidable = false;
			base.Add(new Coroutine(RefillRoutine(player), true));
			respawnTimer = 2.5f;
		}
	}
}
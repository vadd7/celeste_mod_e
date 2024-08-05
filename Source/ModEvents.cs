using Microsoft.Xna.Framework;

namespace Celeste.Mod.celeste_mod_e;

// ModEvents is pretty much just to reset gravity.
public class ModEvents {
	public static void AddEventListeners() {
		Everest.Events.Level.OnTransitionTo += ModOnTransitionTo;
		Everest.Events.Player.OnDie += ModOnDie;
	}

	public static void ModOnTransitionTo(Level level, LevelData next, Vector2 direction) {
		celeste_mod_eModule.gravity = new(0f, 1f);		
		celeste_mod_eModule.speed_multiplier = new(1.1f, 1f);
	}

	public static void OnExit(Level level, LevelExit exit, LevelExit.Mode mode, Session session, HiresSnow snow) {
		celeste_mod_eModule.enabled = false;
	}

	public static void ModOnDie(Player player) {
		celeste_mod_eModule.gravity = new(0f, 1f);
		celeste_mod_eModule.speed_multiplier = new(1.1f, 1f);
	}
}
using System;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.celeste_mod_e;

public class celeste_mod_eModule : EverestModule {
    public static celeste_mod_eModule Instance { get; private set; }

	public static Vector2 gravity = new(0f, 1f);
	public static bool enabled = false;

	// decided not to use settings at all. just broke shit
	// fuck that
	// didnt need them anyway, didnt store much actual settings data.

    //public override Type SettingsType => typeof(SettingsData);
    //public static SettingsData Settings => (SettingsData) Instance._Settings;

    //public override Type SessionType => typeof(SessionData);
    //public static SessionData Session => (SessionData) Instance._Session;

    public celeste_mod_eModule() {
        Instance = this;
		#if DEBUG
			Logger.SetLogLevel(nameof(celeste_mod_eModule), LogLevel.Verbose);
		#else
			Logger.SetLogLevel(nameof(celeste_mod_eModule), LogLevel.Info);
		#endif
    }

    public override void Load() {
        On.Celeste.FallingBlock.Sequence += ModFallingSequence.ModFallingSequenceFn;
		ModEvents.AddEventListeners();
    }

	public override void Unload() {
		enabled = false;
       	On.Celeste.FallingBlock.Sequence -= ModFallingSequence.ModFallingSequenceFn;
	}
}
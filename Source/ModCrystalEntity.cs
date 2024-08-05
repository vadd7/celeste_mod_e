using Monocle;
using Microsoft.Xna.Framework;
using System;
using System.Collections;

namespace Celeste.Mod.celeste_mod_e;

enum FallingDirection {
	UP,
	DOWN, 
	LEFT, 
	RIGHT, 
	OTHER
}

abstract class DirectionCrystal : Entity {
	private Vector2 fall_direction_vec2;

	private Sprite sprite;
	private Image outline;
	private Sprite flash;

	// copied params from dnspy Refill class.
	// these are commented, since we just yoink those from refill, we dont make our own particles.
	//public static ParticleType P_Shatter;
	//public static ParticleType P_Regen;
	//public static ParticleType P_Glow;
	//public static ParticleType P_ShatterTwo; 
	//public static ParticleType P_RegenTwo;
	//public static ParticleType P_GlowTwo;
	public ParticleType p_shatter;
	public ParticleType p_regen;
	public ParticleType p_glow;
	private Wiggler wiggler;
	private BloomPoint bloom;
	private VertexLight light;
	private Level level;
	private SineWave sine;
	private bool oneUse;
	public float respawnTimer;
	
	// artifact from when i actually used sessiondata
	//private static SessionData mod_settings = celeste_mod_eModule.Session;

	//custom code again.
	private FallingDirection _fall_direction;
	public FallingDirection fall_direction {
		get => _fall_direction;
		set {
			_fall_direction = value;

			switch (value) {
				case FallingDirection.UP    : fall_direction_vec2 = new(0f, -1f); break;
				case FallingDirection.DOWN  : fall_direction_vec2 = new(0f, 1f); break;
				case FallingDirection.LEFT  : fall_direction_vec2 = new(-1f, 0f); break;
				case FallingDirection.RIGHT : fall_direction_vec2 = new(1f, 0f); break;
				case FallingDirection.OTHER: fall_direction_vec2 = new(0f, 0f); break;
			}
		}
	}

	// this constructor is mostly copied from dnspy, but heavily modified
	// i used spritebamk instead of whatever was being used, which was a majority of the changes, and also took those as parameters, so different ones
	// can be assigned in the classes that derive from this.
	// other large modification was removing the variance between one dash and to dashes, since we dont even dash at all.
	// sprites are taken as strings, so it isnt a reference to the static field, and it doesnt all just change one sprite
	public DirectionCrystal(Vector2 position, bool oneuse, FallingDirection direction, string spr, string outline, string flash) : base(position) {
		celeste_mod_eModule.enabled = true;

		Collider = new Hitbox(16f, 16f, -8f, -8f);
		Add(new PlayerCollider(new Action<Player>(this.OnPlayer), null, null));

		// yoink particles from refill:
		p_shatter = Refill.P_Shatter;
		p_regen = Refill.P_Regen;
		p_glow = Refill.P_Glow;

		// change the colour of yoinked particles.
		switch (direction) {
			case FallingDirection.UP	: p_shatter.Color = p_regen.Color = p_glow.Color = Color.PaleVioletRed;	p_shatter.Color2 = p_regen.Color2 = p_glow.Color2 = Color.Red; break;
			case FallingDirection.DOWN	: p_shatter.Color = p_regen.Color = p_glow.Color = Color.LightYellow;	p_shatter.Color2 = p_regen.Color2 = p_glow.Color2 = Color.Yellow; break;
			case FallingDirection.LEFT	: p_shatter.Color = p_regen.Color = p_glow.Color = Color.LightCyan;		p_shatter.Color2 = p_regen.Color2 = p_glow.Color2 = Color.Cyan; break;
			case FallingDirection.RIGHT	: p_shatter.Color = p_regen.Color = p_glow.Color = Color.LightPink;		p_shatter.Color2 = p_regen.Color2 = p_glow.Color2 = Color.Pink; break;
		}


		oneUse = oneuse;
		fall_direction = direction;

		this.sprite = GFX.SpriteBank.Create(spr);
		this.outline = new(GFX.Game[outline]);
		this.flash = GFX.SpriteBank.Create(flash);

		Add(this.outline);
		this.outline.CenterOrigin();
		this.outline.Visible = false;		

		Add(sprite);
		sprite.Play("idle", false, false);

		// i didnt add flash as an animation to sprite, because i assume its played on top
		// probably thats why the devs did the same
		base.Add(this.flash);
		this.flash.OnFinish = delegate(string anim) {
			this.flash.Visible = false;
		};

		base.Add(wiggler = Wiggler.Create(1f, 4f, delegate(float v) {
			sprite.Scale = this.flash.Scale = Vector2.One * (1f + v * 0.2f);
		}, false, false));

		base.Add(new MirrorReflection());
		base.Add(bloom = new BloomPoint(0.8f, 16f));
		base.Add(light = new VertexLight(Color.White, 1f, 16, 48));
		base.Add(sine = new SineWave(0.6f, 0f));
		sine.Randomize();
		UpdateY();
		base.Depth = -100;
	}

	// this suprisingly didnt get modified much if at all.
	public override void Update() {
		base.Update();

		if (respawnTimer > 0f) {
			respawnTimer -= Engine.DeltaTime;
			if (respawnTimer <= 0f)	Respawn();
		} else if (base.Scene.OnInterval(0.1f)) {
			level.ParticlesFG.Emit(p_glow, 1, Position, Vector2.One * 5f);
		}

		UpdateY();

		light.Alpha = Calc.Approach(light.Alpha, this.sprite.Visible ? 1f : 0f, 4f * Engine.DeltaTime);
		bloom.Alpha = light.Alpha * 0.8f;

		if (base.Scene.OnInterval(2f) && this.sprite.Visible) {
			flash.Play("flash", true, false);
			flash.Visible = true;
		}
	}

	// couple more functions copied from refill (from dnspy)
	public void UpdateY() {
		this.flash.Y = this.sprite.Y = this.bloom.Y = sine.Value * 2f;
	}

	public override void Added(Scene scene) {
		base.Added(scene);
		level = base.SceneAs<Level>();
	}

	public override void Render() {
		if (sprite.Visible) {
			sprite.DrawOutline(1);
		}
		base.Render();
	}

	// this one is the only one that needed some modifications.
	public virtual void OnPlayer(Player player) {
		celeste_mod_eModule.enabled = true;

		if (celeste_mod_eModule.gravity != fall_direction_vec2) {
			Audio.Play("event:/game/general/diamond_touch", Position);
			
			celeste_mod_eModule.gravity = fall_direction_vec2;

			Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);
			Collidable = false;
			base.Add(new Coroutine(RefillRoutine(player), true));
			respawnTimer = 2.5f;
		}
	}

	public IEnumerator RefillRoutine(Player player) {
		Celeste.Freeze(0.05f);

		yield return null;

		level.Shake(0.3f);
		this.sprite.Visible = this.flash.Visible = false;
		if (!oneUse) this.outline.Visible = true;
		Depth = 8999;

		yield return 0.05f;

		float num = player.Speed.Angle();
		level.ParticlesFG.Emit(p_shatter, 5, Position, Vector2.One * 4f, num - 1.5707964f);
		level.ParticlesFG.Emit(p_shatter, 5, Position, Vector2.One * 4f, num + 1.5707964f);
		SlashFx.Burst(Position, num);

		if (oneUse) RemoveSelf();
		yield break;
	}

	public void Respawn() {
		if (!Collidable) {
			this.Collidable = true;
			this.sprite.Visible = true;
			this.outline.Visible = false;
			base.Depth = -100;
			wiggler.Start();
			Audio.Play("event:/game/general/diamond_return", Position);
			level.ParticlesFG.Emit(p_regen, 16, Position, Vector2.One * 2f);
		}
	}
}
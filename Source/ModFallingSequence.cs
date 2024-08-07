using System.Collections;
using Celeste;
using Celeste.Mod;
using Celeste.Mod.celeste_mod_e;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.celeste_mod_e;

class ModFallingSequence {
	// the majority of this function was generated by DNSPY, thats why it looks a bit cursed~
	// i was too lazy to work out how to create an IL hook, so this'll have to do i guess.
	public static IEnumerator ModFallingSequenceFn(On.Celeste.FallingBlock.orig_Sequence orig, FallingBlock self) {
		if (celeste_mod_eModule.enabled) {
			while (!self.Triggered && (self.finalBoss || !self.PlayerFallCheck())) {
				yield return null;
			}

			while (self.FallDelay > 0f){
				self.FallDelay -= Engine.DeltaTime;
				yield return null;
			}

			self.HasStartedFalling = true;

			// 2 custom line!
			Vector2 fall_dir = celeste_mod_eModule.gravity;
			Vector2 multiplyer = celeste_mod_eModule.speed_multiplier;

			Level level;
			for (;;) {
				self.ShakeSfx();
				self.StartShaking(0f);
				Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);

				if (self.finalBoss) {
					self.Add(new Coroutine(self.HighlightFade(1f), true));
				}

				yield return 0.2f;
				float timer = 0.4f;
				if (self.finalBoss) {
					timer = 0.2f;
				}

				while (timer > 0f && self.PlayerWaitCheck()) {
					yield return null;
					timer -= Engine.DeltaTime;
				}

				self.StopShaking();
				int num = 2;
				while ((float)num < self.Width) {
					if (self.Scene.CollideCheck<Solid>(self.TopLeft + new Vector2((float)num, -2f))) {
						self.SceneAs<Level>().Particles.Emit(FallingBlock.P_FallDustA, 2, new Vector2(self.X + (float)num, self.Y), Vector2.One * 4f, 1.5707964f);
					}
					self.SceneAs<Level>().Particles.Emit(FallingBlock.P_FallDustB, 2, new Vector2(self.X + (float)num, self.Y), Vector2.One * 4f);
					num += 4;
				}

				//float speed = 0f; changed, unused variable in updated code.
				Vector2 speed_vector2 =  new(0f,0f);
				float maxSpeed = self.finalBoss ? 130f : 160f;
				for (;;) {
					level = self.SceneAs<Level>();

					
					

					// changed code starts here!
					
					// these are always positive, cuz it messed up meth. 
					// instead, the negative is applied in the move fn call, 
					speed_vector2.Y = fall_dir.Y > 0 ? Calc.Approach(speed_vector2.Y, maxSpeed, 500f * Engine.DeltaTime) : Calc.Approach(speed_vector2.Y, -maxSpeed, 500f * Engine.DeltaTime * fall_dir.Y); 
					speed_vector2.X = fall_dir.X > 0 ? Calc.Approach(speed_vector2.X, maxSpeed, 500f * Engine.DeltaTime) : Calc.Approach(speed_vector2.X, -maxSpeed, 500f * Engine.DeltaTime * fall_dir.X); 

					//speed = Calc.Approach(speed, maxSpeed, 500f * Engine.DeltaTime); // unused, because it was replaced.
					// i assume this is used to move the object, then if it cant move anymore it'll return true, and we'll break out of the loop.
					// thats why i added the horizontal movement here.
					//self.MoveHCollideSolids(1000f, true);
					Logger.Log("custom_mod_e", "sequence speed vector x: " + speed_vector2.X + "\nsequence speed vector y: " + speed_vector2.Y);

					//self.MoveHCollideSolids(( fall_dir.X > 0 ? speed_vector2.X : -speed_vector2.X) * Engine.DeltaTime, true, null);

					if (self.MoveVCollideSolids(( fall_dir.Y > 0 ? speed_vector2.Y : -speed_vector2.Y) * Engine.DeltaTime * multiplyer.Y, true, null) ) {
						break;
					}

					if (self.MoveHCollideSolids(( fall_dir.X > 0 ? speed_vector2.X : -speed_vector2.X) * Engine.DeltaTime * multiplyer.X , true, null)) { 
						break; 
					}

					// this is probably to destroy out of bounds objects
					if 	(self.Top > (float)(level.Bounds.Bottom + 16) || 
						(self.Top > (float)(level.Bounds.Bottom - 1) && self.CollideCheck<Solid>(self.Position + fall_dir)))
					{
						goto destroy_maybe_idk;
					}

					// changed code ends here!
				
					yield return null;
					level = null;
				}
				self.ImpactSfx();
				Input.Rumble(RumbleStrength.Strong, RumbleLength.Medium);

				// and x directional shake instead.
				if (fall_dir.Y != 0) self.SceneAs<Level>().DirectionalShake(Vector2.UnitY, self.finalBoss ? 0.2f : 0.3f);
				else self.SceneAs<Level>().DirectionalShake(Vector2.UnitX, self.finalBoss ? 0.2f : 0.3f);

				if (self.finalBoss) {
					self.Add(new Coroutine(self.HighlightFade(0f), true));
				}
				self.StartShaking(0f);
				self.LandParticles();
				yield return 0.2f;
				self.StopShaking();

				// replaced new Vector2(0f, 1f) with falldir, to account for going in different directions.
				// same for that while loop.
				if (self.CollideCheck<SolidTiles>(self.Position + fall_dir)){
					goto Block_15;
				}

				while (self.CollideCheck<Platform>(self.Position + fall_dir)){
					yield return 0.1f;
				}
			}
			destroy_maybe_idk:
			self.Collidable = self.Visible = false;
			yield return 0.2f;

			if (level.Session.MapData.CanTransitionTo(level, new Vector2(self.Center.X, self.Bottom + 12f))) {
				yield return 0.2f;
				self.SceneAs<Level>().Shake(0.3f);
				Input.Rumble(RumbleStrength.Strong, RumbleLength.Medium);
			}

			self.RemoveSelf();
			self.DestroyStaticMovers();
			yield break;
			Block_15:
			self.Safe = true;
			yield break;
		} else {
			// chatgpt made this to pass up the contents of orig,
			// dont ask me how it works, it looks ok ish imo.
			var result = orig(self);
			while (result.MoveNext()) {
				yield return result.Current; 
			}
		}
	}
}
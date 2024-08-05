local crystal_fast = {}

crystal_fast.name = "celeste_mod_e/FallNormalSpeedCrystal"
crystal_fast.depth = -100
crystal_fast.placements = {
    {
        name = "Falling block - fall normal speed crystal",
		placementType = "point",
        data = {
            oneUse = false
        }
    }
}

function crystal_fast.texture(room, entity)
    return "crystals/speed_normie/idle00"
end

return crystal_fast
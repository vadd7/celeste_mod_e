local crystal_fast = {}

crystal_fast.name = "celeste_mod_e/FallFastSpeedCrystal"
crystal_fast.depth = -100
crystal_fast.placements = {
    {
        name = "Falling block - fall fast speed crystal",
		placementType = "point",
        data = {
            oneUse = false
        }
    }
}

function crystal_fast.texture(room, entity)
    return "crystals/speed_fast/idle00"
end

return crystal_fast
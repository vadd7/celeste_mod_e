local crystal_left = {}

crystal_left.name = "celeste_mod_e/FallLeftCrystal"
crystal_left.depth = -100
crystal_left.placements = {
    {
        name = "Falling block - fall left crystal",
		placementType = "point",
        data = {
            oneUse = false
        }
    }
}

function crystal_left.texture(room, entity)
    return "crystals/crystal_left/idle00"
end

return crystal_left
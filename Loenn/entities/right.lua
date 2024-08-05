local crystal_right = {}

crystal_right.name = "celeste_mod_e/FallRightCrystal"
crystal_right.depth = -100
crystal_right.placements = {
    {
        name = "Falling block - fall right crystal",
		placementType = "point",
        data = {
            oneUse = false
        }
    }
}

function crystal_right.texture(room, entity)
    return "crystals/crystal_right/idle00"
end

return crystal_right
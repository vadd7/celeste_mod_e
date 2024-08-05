local crystal_up = {}

crystal_up.name = "celeste_mod_e/FallUpCrystal"
crystal_up.depth = -100
crystal_up.placements = {
    {
        name = "Falling block - fall up crystal",
		placementType = "point",
        data = {
            oneUse = false
        }
    }
}

function crystal_up.texture(room, entity)
    return "crystals/crystal_up/idle00"
end

return crystal_up
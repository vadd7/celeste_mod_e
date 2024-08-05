local crystal_down = {}

crystal_down.name = "celeste_mod_e/FallDownCrystal"
crystal_down.depth = -100
crystal_down.placements = {
    {
        name = "Falling block - fall down crystal",
		placementType = "point",
        data = {
            oneUse = false
        }
    }
}

function crystal_down.texture(room, entity)
    return "crystals/crystal_down/idle00"
end

return crystal_down
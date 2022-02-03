using DiskCardGame;
using UnityEngine;
using APIPlugin;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

namespace OcCardPack
{
    class Utils
    {
        // Helper to add a new ability
        public static NewAbility AddAbility<T>(string name, string rulebookDescription, bool activated=false) where T : AbilityBehaviour
        {
            Type type = typeof(T);

            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = 0;
            info.rulebookName = name;
            info.rulebookDescription = rulebookDescription;
            info.activated = activated;

            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

            Texture2D icon = OcCardPackPlugin.AllAbilityIcons.Single(t => t.name.Equals(typeof(T).Name,System.StringComparison.OrdinalIgnoreCase));

            // Create ability
            NewAbility ability = new NewAbility(info, type, icon, AbilityIdentifier.GetID(OcCardPackPlugin.PluginGuid, name));

            // Get static field and set 'ability' field
            FieldInfo field = type.GetField("ability",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance
            );
            field.SetValue(null, ability.ability);

            return ability;
        }

    }
}

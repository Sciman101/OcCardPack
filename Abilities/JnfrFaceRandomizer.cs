using System.Collections;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace OcCardPack.Abilities
{
    class JnfrFaceRandomizer : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility specialTriggeredAbility;

        // Get all applicable jnfr face sprites
        public static Sprite[] jnfrFaceSprites;

        public override bool RespondsToDrawn()
        {
            return true;
        }

        public override IEnumerator OnDrawn()
        {
            // Randomize portrait
            int index = Random.Range(0, jnfrFaceSprites.Length);

            Card.renderInfo.portraitOverride = jnfrFaceSprites[index];
            Card.RenderCard();

            yield break;
        }

        public static void Create()
        {
            StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
            NewSpecialAbility ability = new NewSpecialAbility(typeof(JnfrFaceRandomizer),SpecialAbilityIdentifier.GetID(OcCardPackPlugin.PluginGuid,"JnfrFaceRandomizer"),info);
            JnfrFaceRandomizer.specialTriggeredAbility = ability.specialTriggeredAbility;

            // Load textures and convert to sprites
            jnfrFaceSprites = OcCardPackPlugin.AllCardArt.Where(tex => tex.name.StartsWith("jnfr") && !tex.name.Contains("emission")).
                Select(tex => Sprite.Create(tex,CardUtils.DefaultCardArtRect,CardUtils.DefaultVector2)).
                ToArray();
        }

    }
}

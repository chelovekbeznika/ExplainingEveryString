using ExplainingEveryString.Data.Blueprints.AssetsExtraction.cs;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExplainingEveryString.Data.Blueprints
{
    public static class AssetsExtractor
    {
        private static readonly Dictionary<Type, ActorAssetsExtractor> assetsExtractors =
            new Dictionary<Type, ActorAssetsExtractor>()
        {
            { typeof(Blueprint), new ActorAssetsExtractor() },
            { typeof(ObstacleBlueprint), new ActorAssetsExtractor() },
            { typeof(DoorBlueprint), new DoorAssetsExtractor() },
            { typeof(PlayerBlueprint), new PlayerAssetsExtractor() },
            { typeof(EnemyBlueprint), new EnemyAssetsExtractor() },
            { typeof(ShadowEnemyBlueprint), new ShadowEnemyAssetsExtractor() },
            { typeof(FirstBossBlueprint), new FirstBossAssetsExtractor() },
            { typeof(SecondBossBlueprint), new SecondBossAssetsExtractor() }
        };

        public static List<String> GetNeccessarySprites(IBlueprintsLoader loader)
        {
            var blueprints = loader.GetBlueprints().Values;
            return blueprints.SelectMany(blueprint => GetSprites(blueprint)).Where(ss => ss != null)
                .Select(ss => ss.Name).Distinct().ToList();
        }

        public static List<String> GetNecessarySounds(IBlueprintsLoader loader)
        {
            var blueprints = loader.GetBlueprints().Values;
            return blueprints.SelectMany(blueprint => GetSpecEffects(blueprint)).Where(se => se != null && se.Sound != null)
                .Select(se => se.Sound.Name).Distinct().ToList();
        }

        private static IEnumerable<SpriteSpecification> GetSprites(Blueprint blueprint)
        {
            var blueprintType = blueprint.GetType();
            var getSpritesMethod = typeof(AssetsExtractor)
                .GetMethod(nameof(GetFromCertainTypeSprites), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(blueprintType);
            return getSpritesMethod.Invoke(null, new Object[] { blueprint }) as IEnumerable<SpriteSpecification>;
        }

        private static IEnumerable<SpecEffectSpecification> GetSpecEffects(Blueprint blueprint)
        {
            var blueprintType = blueprint.GetType();
            var getSpecEffectsMethod = typeof(AssetsExtractor)
                .GetMethod(nameof(GetFromCertainTypeSpecEffects), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(blueprintType);
            return getSpecEffectsMethod.Invoke(null, new Object[] { blueprint }) as IEnumerable<SpecEffectSpecification>;
        }

        private static IEnumerable<SpriteSpecification> GetFromCertainTypeSprites<T>(T blueprint) 
            where T : Blueprint
        {
            var extractor = GetAssetsExtractor<T>();
            var specEffectSprites = extractor.GetSpecEffects(blueprint)
                .Where(ses => ses != null && ses.Sprite != null).Select(ses => ses.Sprite);
            return extractor.GetSprites(blueprint).Concat(specEffectSprites);
        }

        private static IEnumerable<SpecEffectSpecification> GetFromCertainTypeSpecEffects<T>(T blueprint) 
            where T : Blueprint
        {
            var extractor = GetAssetsExtractor<T>();
            return extractor.GetSpecEffects(blueprint);
        }

        private static IAssetsExtractor<T> GetAssetsExtractor<T>() where T : Blueprint
        {
            return (IAssetsExtractor<T>)assetsExtractors[typeof(T)];
        }
    }
}

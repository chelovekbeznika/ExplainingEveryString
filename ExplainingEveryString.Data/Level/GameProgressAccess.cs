using System;
using System.Collections.Generic;
using System.IO;

namespace ExplainingEveryString.Data.Level
{
    public static class GameProgressAccess
    {
        private static readonly Dictionary<Int32, GameProgress> cache = new Dictionary<Int32, GameProgress>();

        public static GameProgress Load(Int32 profileNumber)
        {
            if (cache.ContainsKey(profileNumber)) 
            { 
                return cache[profileNumber]; 
            }
            else
            {
                if (File.Exists(FileNames.GameProgress(profileNumber)))
                {
                    var profile = JsonDataAccessor.Instance.Load<GameProgress>(FileNames.GameProgress(profileNumber));
                    cache.Add(profileNumber, profile);
                    return profile;
                }
                else
                    return null;
            }
        }

        public static void Save(GameProgress gameProgress, Int32 profileNumber)
        {
            JsonDataAccessor.Instance.Save(FileNames.GameProgress(profileNumber), gameProgress);
            if (cache.ContainsKey(profileNumber))
            {
                cache[profileNumber] = gameProgress;
            }
            else
            {
                cache.Add(profileNumber, gameProgress);
            }
        }
    }
}

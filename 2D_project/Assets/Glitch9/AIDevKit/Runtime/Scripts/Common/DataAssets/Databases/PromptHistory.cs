using Glitch9.Collections;
using Glitch9.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    [CreateAssetMenu(fileName = nameof(PromptHistory), menuName = AIDevKitConfig.kLogDatabase, order = AIDevKitConfig.kLogDatabaseOrder)]
    public class PromptHistory : ScriptableDatabase<PromptHistory.Repo, GENTaskRecord, PromptHistory>
    {
        public class Repo : Database<GENTaskRecord> { }
        public static List<GENTaskRecord> GetBySender(string sender = null)
        {
            return DB.FindAll(log => log.Sender == sender).ConvertAll(log => log);
        }
    }
}
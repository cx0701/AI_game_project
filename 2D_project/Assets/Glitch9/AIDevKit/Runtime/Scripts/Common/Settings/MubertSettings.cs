using UnityEngine;

namespace Glitch9.AIDevKit.Mubert
{
    [CreateAssetMenu(fileName = nameof(MubertSettings), menuName = AIDevKitConfig.kMubertSettings, order = AIDevKitConfig.kMubertSettingsOrder)]
    public partial class MubertSettings : AIClientSettings<MubertSettings>
    {
    }
}
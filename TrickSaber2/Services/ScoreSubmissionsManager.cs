using System.Linq;
using SiraUtil.Submissions;
using TrickSaber2.Configuration;
using TrickSaber2.Extensions;
using Zenject;

namespace TrickSaber2.Services; 

internal class ScoreSubmissionsManager : IInitializable
{
    private readonly PluginConfig pluginConfig;
    private readonly Submission submission;

    public ScoreSubmissionsManager(PluginConfig pluginConfig, Submission submission)
    {
        this.pluginConfig = pluginConfig;
        this.submission = submission;
    }

    public void Initialize()
    {
        if (pluginConfig.TricksAffectHitbox)
        {
            submission.DisableScoreSubmission("TrickSaber2", "Hitboxes affected by tricks");
        }
        
        if (pluginConfig.Tricks.HaveTimeWarp())
        {
            submission.DisableScoreSubmission("TrickSaber2", "Throw trick time warp in use");
        }
    }
}
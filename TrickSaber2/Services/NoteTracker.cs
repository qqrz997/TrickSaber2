using System;
using TrickSaber2.Configuration;
using Zenject;

namespace TrickSaber2.Services;

internal class NoteTracker : IInitializable, IDisposable
{
    private readonly PluginConfig pluginConfig;
    private readonly BeatmapObjectManager beatmapObjectManager;
    private readonly float noteJumpSpeed;

    public NoteTracker(
        PluginConfig pluginConfig,
        BeatmapObjectManager beatmapObjectManager,
        GameplayCoreSceneSetupData gameplayCoreSceneSetupData)
    {
        this.pluginConfig = pluginConfig;
        this.beatmapObjectManager = beatmapObjectManager;
        noteJumpSpeed = gameplayCoreSceneSetupData.beatmapBasicData.noteJumpMovementSpeed;
    }

    private int spawnedNotes;

    public bool DisableIfNotesOnScreen() => spawnedNotes > 0;

    public void Initialize()
    {
        if (pluginConfig.DisableDuringNotes)
        {
            beatmapObjectManager.noteWasSpawnedEvent += HandleBeatmapObjectManagerNoteWasSpawned;
            beatmapObjectManager.noteWasDespawnedEvent += HandleBeatmapObjectManagerNoteWasDespawned;
        }
    }

    public void Dispose()
    {
        beatmapObjectManager.noteWasSpawnedEvent -= HandleBeatmapObjectManagerNoteWasSpawned;
        beatmapObjectManager.noteWasDespawnedEvent -= HandleBeatmapObjectManagerNoteWasDespawned;
    }

    private void HandleBeatmapObjectManagerNoteWasSpawned(NoteController noteController)
    {
        spawnedNotes++;
    }

    private void HandleBeatmapObjectManagerNoteWasDespawned(NoteController obj)
    {
        spawnedNotes--;
    }
}
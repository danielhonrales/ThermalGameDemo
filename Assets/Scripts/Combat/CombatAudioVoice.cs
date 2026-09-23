using UnityEngine;

/// <summary>One bounded sound per emitter, with a short release instead of overlapping long clips.</summary>
public sealed class CombatAudioVoice : MonoBehaviour
{
    private AudioSource source;
    private float nominalVolume;
    private float startedAt;
    private float duration;
    private float level;

    public static AudioSource Create(Transform parent, string name, AudioClip clip, bool loop, float volume = 1f)
    {
        var emitter = new GameObject(name);
        emitter.transform.SetParent(parent, false);
        var audio = emitter.AddComponent<AudioSource>();
        audio.clip = clip;
        audio.loop = loop;
        audio.playOnAwake = false;
        audio.spatialBlend = 1f;
        audio.minDistance = 0.2f;
        audio.maxDistance = 7f;
        audio.volume = volume;
        return audio;
    }

    public static void Play(AudioSource audio, AudioClip clip, float gain, float maximumSeconds, float startSeconds = 0f)
    {
        if (audio == null || clip == null) return;
        var voice = audio.GetComponent<CombatAudioVoice>();
        if (voice == null)
        {
            voice = audio.gameObject.AddComponent<CombatAudioVoice>();
            voice.source = audio;
            voice.nominalVolume = audio.volume;
        }
        audio.Stop();
        audio.clip = clip;
        audio.loop = false;
        float start = Mathf.Clamp(startSeconds, 0f, Mathf.Max(0f, clip.length - 0.01f));
        audio.time = start;
        voice.startedAt = Time.unscaledTime;
        voice.duration = Mathf.Min(maximumSeconds, (clip.length - start) / Mathf.Max(0.1f, Mathf.Abs(audio.pitch)));
        voice.level = voice.nominalVolume * gain;
        audio.volume = voice.level;
        voice.enabled = true;
        audio.Play();
    }

    private void Update()
    {
        if (source == null || !source.isPlaying) { enabled = false; return; }
        float remaining = duration - (Time.unscaledTime - startedAt);
        source.volume = level * Mathf.Clamp01(remaining / 0.1f);
        if (remaining <= 0f) { source.Stop(); enabled = false; }
    }
}

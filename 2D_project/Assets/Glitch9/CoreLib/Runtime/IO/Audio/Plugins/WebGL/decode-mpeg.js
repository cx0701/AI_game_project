/// <reference lib="dom" />

window.decodeMpegAsync = async function (base64Data, target, method) {
    try {
        const byteArray = Uint8Array.from(atob(base64Data), c => c.charCodeAt(0));
        const audioCtx = new (window.AudioContext || window.webkitAudioContext)();

        const audioBuffer = await audioCtx.decodeAudioData(byteArray.buffer);
        const channelData = audioBuffer.getChannelData(0); // Mono

        const floatBytes = new Uint8Array(channelData.buffer);
        const base64PCM = btoa(String.fromCharCode(...floatBytes));

        const result = {
            pcm: base64PCM,
            sampleRate: audioBuffer.sampleRate
        };

        SendMessage(target, method, JSON.stringify(result));
    } catch (e) {
        console.error("MP3 decoding failed:", e);
    }
};

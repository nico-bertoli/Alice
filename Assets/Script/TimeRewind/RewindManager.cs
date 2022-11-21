using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindManager {
    private List<TimeFrame> frames = new List<TimeFrame>();
    private float rewindTime;

    public RewindManager(float _rewindSize) {
        rewindTime = _rewindSize;
    }

    public void RegisterFrame(WorldCell _cell) {
        TimeFrame newFrame = new TimeFrame(_cell,Time.time);
        frames[frames.Count - 1].EndTime = Time.time;
        frames.Add(newFrame);
        RemoveTooOldFrames();
    }

    public WorldCell Rewind() {
        RemoveTooOldFrames();
        return frames[frames.Count - 1].Cell;
    }
    
    /// <summary>
    /// Removes all the frames with times too old to be rewinded
    /// </summary>
    private void RemoveTooOldFrames() {
        int? startCleaningIndex = FindFirstTooOldIndex();
        removeAllFramesSinceIndex(startCleaningIndex);
    }

    /// <summary>
    /// Finds the first index with time too old to be rewinded
    /// </summary>
    /// <returns></returns>
    private int? FindFirstTooOldIndex() {
        int? startCleaningIndex = null;
        for (int i = 0; i < frames.Count; i++) {
            if (frames[i].EndTime != null && Time.time - frames[i].EndTime > rewindTime) {
                startCleaningIndex = i;
                break;
            }
        }
        return startCleaningIndex;
    }

    /// <summary>
    /// Removes all frames starting from given index, if the index isn't null
    /// </summary>
    /// <param name="_index"></param>
    private void removeAllFramesSinceIndex(int? _index) {
        if (_index != null) {
            for (int i = frames.Count; i >= _index; i--) {
                frames.RemoveAt(i);
            }
        }
    }

    private class TimeFrame {
        public WorldCell Cell { get; set; }
        public float StratTIme { get; set; }
        public float? EndTime { get; set; }

        public TimeFrame(WorldCell _cell, float _startTime) {
            Cell = _cell;
            StratTIme = _startTime;
        }
    }
}


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
        if(frames.Count -1 >=0) frames[frames.Count - 1].EndTime = Time.time;
        frames.Add(newFrame);
        RemoveTooOldFrames();
    }

    public List<TimeFrame> Rewind() {
        RemoveTooOldFrames();
        //return frames[0].Cell;
        //if (frames.Count - 1 >= 0) frames[frames.Count - 1].EndTime = Time.time;
        foreach (TimeFrame frame in frames) if (frame.EndTime == null) frame.EndTime = Time.time;
        return frames;
    }
    
    /// <summary>
    /// Removes all the frames with times too old to be rewinded
    /// </summary>
    private void RemoveTooOldFrames() {
        int tooOldFramesNum = CountTooOldFrames();
        removeFramesFromOlder(tooOldFramesNum);
    }

    /// <summary>
    /// Finds the first index with time too old to be rewinded
    /// </summary>
    /// <returns></returns>
    private int CountTooOldFrames() {
        int tooOldFrames = 0;
        for (int i = 0; i < frames.Count; i++) {
            if (frames[i].EndTime != null && Time.time - frames[i].EndTime > rewindTime) {
                tooOldFrames++;
            }
            else break;
        }
        return tooOldFrames;
    }

    /// <summary>
    /// Removes all frames starting from given index, if the index isn't null
    /// </summary>
    /// <param name="_index"></param>
    private void removeFramesFromOlder(int _num) {
            for(int i = 0; i < _num; i++)
                frames.RemoveAt(0);
    }
    public void DebugList() {
        string ris = "";
        for(int i = 0;i<frames.Count;i++)
            ris += frames[i].Cell + " start: " + frames[i].StratTime + "end: " + frames[i].EndTime+'\n';
        Debug.Log(ris);
    }
}

public class TimeFrame {
    public WorldCell Cell { get; set; }
    public float StratTime { get; set; }
    public float? EndTime { get; set; }

    public TimeFrame(WorldCell _cell, float _startTime) {
        Cell = _cell;
        StratTime = _startTime;
    }
}
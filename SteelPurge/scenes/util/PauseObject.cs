using Godot;
using System;

public class PauseObject : Godot.Object
{
    private bool _isPaused = false;
    private bool _isJustUnPaused = false;

    public bool IsPaused => _isPaused;
    
    public PauseObject()
    {
        
    }

    public void PauseOrUnpause(SceneTree tree)
    {
        if (_isPaused)
        {
            tree.Paused = false;
            _isPaused = false;
            _isJustUnPaused = true;
        }
			
        if (!tree.Paused && !_isJustUnPaused)
        {
            tree.Paused = true;
            _isPaused = true;
        }

        if (_isJustUnPaused)
            _isJustUnPaused = false;
    }

    public void TryToPause(SceneTree tree)
    {
        if (!tree.Paused && !_isJustUnPaused)
        {
            tree.Paused = true;
            _isPaused = true;
        }
    }

    public void TryToUnpause(SceneTree tree)
    {
        // TODO: Might be buggy
        if (_isPaused)
        {
            tree.Paused = false;
            _isPaused = false;
        }
    }
}

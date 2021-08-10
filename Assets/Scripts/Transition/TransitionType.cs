using System.Collections.Generic;
using UnityEngine;

// Placeholder anims, waiting for confirmation
public enum TransitionType
{
    None,
    CrossFadeLong,
    CrossFadeShort,
    BlackBars,
    CircleSwipe,
    CircleExpand,
    PanelSwipeHorizontal,
    PanelSwipeVertical, 
    FadeToBlack
}


[System.Serializable]
public class TransitionAnimationControllers
{
    public RuntimeAnimatorController None;
    public RuntimeAnimatorController CrossFadeLong;
    public RuntimeAnimatorController CrossFadeShort;
    public RuntimeAnimatorController CinematicBlackBars;
    public RuntimeAnimatorController CircleSwipe;
    public RuntimeAnimatorController CircleExpand;
    public RuntimeAnimatorController PanelSwipeHorizontal;
    public RuntimeAnimatorController PanelSwipeVertical;
    public RuntimeAnimatorController FadeToBlack;

    public TransitionTypes transitionTypes;

    public RuntimeAnimatorController GetAnimator(GameState currGameState, GameState previousGameState)
    {
        transitionTypes = new TransitionTypes();
        TransitionType transitionType = (TransitionType)transitionTypes.transitionTypes2DList[(int)previousGameState][(int)currGameState];
        switch (transitionType)
        {
            case TransitionType.None:
                return None;
            case TransitionType.CrossFadeLong:
                return CrossFadeLong;
            case TransitionType.CrossFadeShort:
                return CrossFadeShort;
            case TransitionType.BlackBars:
                return CinematicBlackBars;
            case TransitionType.CircleSwipe:
                return CircleSwipe;
            case TransitionType.CircleExpand:
                return CircleExpand;
            case TransitionType.PanelSwipeHorizontal:
                return PanelSwipeHorizontal;
            case TransitionType.PanelSwipeVertical:
                return PanelSwipeVertical;
            case TransitionType.FadeToBlack:
                return FadeToBlack;
            default:
                return None; 
        }
    }

    public RuntimeAnimatorController GetAnimator(TransitionType transitionType)
    {
        switch (transitionType)
        {
            case TransitionType.None:
                return None;
            case TransitionType.CrossFadeLong:
                return CrossFadeLong;
            case TransitionType.CrossFadeShort:
                return CrossFadeShort;
            case TransitionType.BlackBars:
                return CinematicBlackBars;
            case TransitionType.CircleSwipe:
                return CircleSwipe;
            case TransitionType.CircleExpand:
                return CircleExpand;
            case TransitionType.PanelSwipeHorizontal:
                return PanelSwipeHorizontal;
            case TransitionType.PanelSwipeVertical:
                return PanelSwipeVertical;
            default:
                return None;
        }
    }

}

public class TransitionTypes
{

    public List<List<int>> transitionTypes2DList;


    public TransitionTypes()
    {
        transitionTypes2DList = new List<List<int>>
            {
            new List<int>  { 0, 0, 1, 0, 0, 0, 0  },
            new List<int>  { 0, 0, 0, 0, 3, 0, 0  },
            new List<int>  { 1, 0, 2, 0, 0, 3, 4  },
            new List<int>  { 0, 0, 0, 0, 0, 0, 0  },
            new List<int>  { 0, 0, 0, 0, 0, 0, 4  },
            new List<int>  { 0, 5, 0, 0, 0, 0, 0  },
            new List<int>  { 0, 4, 0, 4, 0, 0, 0 }
        };
    }
}

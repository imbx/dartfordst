using System.Collections;
using System.Collections.Generic;
using BoxScripts;
using UnityEngine;

public class ItemNote : Item
{
    public override void Execute(bool isLeftAction = true) {
        if(hasRequirement && !GameController.current.database.GetProgressionState(reqID)) return;
        // GameController.current.music.playMusic(itemSound);
        this.OnEnd();
        return;
    }

    protected override void OnEnd(bool destroyGameObject = false)
    {
        isInteractingThis = false;
        isLeftAction = false;
        GameController.current.database.EditProgression(_id, true);
        gameControllerObject.ChangeState(GameState.OPENNOTEBOOK);
        GameController.current.ui.ForceDiaryPage(_id);
        Destroy(gameObject);
    }
}

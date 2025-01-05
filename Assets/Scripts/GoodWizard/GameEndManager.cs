using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameEndManager : MonoBehaviour
{
    public XPManager xpManager;
    public WinningScript winningScript;
    private InputAction endGameAction;

    private void Awake()
    {
        // activate and initialize the input action
        endGameAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        endGameAction.Enable();
    }

    private void OnDestroy()
    {
        //  dispose of the input action when the script is destroyed
        endGameAction.Disable();
        endGameAction.Dispose();
    }

    private void Update()
    {
        // Check if the player has at least 4 skill points before allowing the final skill
        if (xpManager.playerXP >= 60 && endGameAction.WasPerformedThisFrame())
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Debug.Log("End Game action performed. Ending the game...");

        // Call the method from WinningScript
        if (winningScript != null)
        {
            winningScript.WinGame();
        }
        else
        {
            Debug.LogError("WinningScript reference is missing!");
        }

    }
}

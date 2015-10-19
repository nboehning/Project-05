using UnityEngine;
using System.Collections;

public class ScriptPhaseTransition {

    GameState currentPhase;

    StateCommands command;

    public ScriptPhaseTransition(GameState thisPhase, StateCommands thisCommand)
    {
        currentPhase = thisPhase;
        command = thisCommand;
    }

    public override int GetHashCode()
    {
        return 17 + 31 * currentPhase.GetHashCode() + 31 * command.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        ScriptPhaseTransition other = obj as ScriptPhaseTransition;
        return other != null && this.currentPhase == other.currentPhase && this.command == other.command;
    }
    
}

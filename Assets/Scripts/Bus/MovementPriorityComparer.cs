public class MovementPriorityComparer
{
    public bool HasHigherPriority(IBusMovementState state, IBusMovementState other)
    {
        if (state.EffectivePriority != other.EffectivePriority)
            return state.EffectivePriority > other.EffectivePriority;

        return state.PriorityOrder < other.PriorityOrder;
    }
}
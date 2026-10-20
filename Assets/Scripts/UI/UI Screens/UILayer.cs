// Higher values sit above lower ones. Only the top screen of the highest non-empty layer
// is focused and receives input.
public enum UILayer
{
    Menu = 0,   // Navigation history: pause, inventory, submenus. Cancel goes back.
    Modal = 1,  // Interruptions: dialog, confirmations, pickers. Don't affect menu history or game state.
}

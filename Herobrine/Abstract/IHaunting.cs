namespace Herobrine.Abstract
{
    public interface IHaunting
    {
        /// <summary>
        ///     Ticked to keep the haunt going.
        /// </summary>
        void Update();

        /// <summary>
        ///     Used to undo any changes made to the world after the victim is done being haunted.
        /// </summary>
        void CleanUp();

        /// <summary>
        ///     The player this haunting targets.
        /// </summary>
        Victim Victim { get; }

        /// <summary>
        ///     The end condition of this haunting. Should be a public get and set.
        /// </summary>
        IHauntingEndCondition EndCondition { get; set; }
    }
}
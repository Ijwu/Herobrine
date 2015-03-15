using System.Collections.Generic;

namespace Herobrine.Abstract
{
    public interface IHauntingEndCondition
    {
        /// <summary>
        /// Called to get the status of the end condition.
        /// </summary>
        /// <returns>If the condition has reached its end.</returns>
        bool Update();

        /// <summary>
        /// Because I'm not a very good programmer, this is essentially a forced constructor in the interface.
        /// </summary>
        /// <param name="parameters">Parameters to the end condition.</param>
        /// <returns>True if there was no error in parsing.</returns>
        bool ParseParameters(List<string> parameters);

        /// <summary>
        /// Saves the current EndCondition state to a dictionary.
        /// </summary>
        Dictionary<string, string> Save();

        /// <summary>
        /// Restores the EndCondition state from a dictionary.
        /// </summary>
        void Load(Dictionary<string, string> state);

        /// <summary>
        /// The haunting which this condition decides the status of.
        /// </summary>
        IHaunting Haunting { get; }
    }
}
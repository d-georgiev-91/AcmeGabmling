namespace AcmeGambling
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISlotMachine
    {
        /// <summary>
        /// Spins the sloth machine reels
        /// </summary>
        /// <param name="steak">The steak</param>
        /// <returns>The won amount of won reward</returns>
        decimal Spin(decimal steak);
    }
}

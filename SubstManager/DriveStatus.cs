namespace SubstManager
{
    /// <summary>
    /// ドライブの状態
    /// </summary>
    public enum DriveStatus
    {
        /// <summary>
        /// 利用可能
        /// </summary>
        Enable,

        /// <summary>
        /// 利用不可
        /// </summary>
        Disable,

        /// <summary>
        /// 利用中
        /// </summary>
        Busy,

        /// <summary>
        /// 不明
        /// </summary>
        Unknown
    }
}
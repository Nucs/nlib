namespace nucs.Automation {
    /// <summary>
    ///     Classifications to what type of window is it.
    /// </summary>
    public enum WindowClassification {
        /// <summary>
        ///     A regular window to a regular exe.
        /// </summary>
        Generic,
        /// <summary>
        ///     Belongs to the operation system.
        /// </summary>
        Windows,
        /// <summary>
        ///     An explorer window - the directory browser of the operation system.
        /// </summary>
        Explorer,
        /// <summary>
        ///     A chrome/firefox/internet explorer window.
        /// </summary>
        InternetBrowser,
    }
}
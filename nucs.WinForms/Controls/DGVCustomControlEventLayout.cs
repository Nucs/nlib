namespace nucs.WinForms.Controls {
    /// <summary>
    ///     Handler for cell changing value event.
    /// </summary>
    /// <param name="sender">The cell object</param>
    /// <param name="value">The value that the cell returns</param>
    public delegate void CellChangedHandler(object sender, object value);

    public interface ICustomControlEventLayout {
        /// <summary>
        ///     Event that is fired during value change.
        /// </summary>
        event CellChangedHandler CellValueChanged;

        /// <summary>
        ///     Needs to fire the event <see cref="CellValueChanged"/> on cell value change.
        /// </summary>
        void OnCellValueChanged(object sender, object value);
    }
}
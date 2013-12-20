using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nucs.ADO.NET;
using nucs.Annotations;
using nucs.Collections;
using nucs.Collections.Extensions;
using nucs.Forms;
using nucs.SystemCore;
using nucs.SystemCore.Dynamic;
using DataGrid = nucs.Controls.DataGridPro;
using Column = System.Windows.Forms.DataGridViewColumn;
using Row = System.Windows.Forms.DataGridViewRow;
using Cell = System.Windows.Forms.DataGridViewCell;

namespace nucs.Controls {
    public delegate void ReadyForModificationsHandler(object sender, int TimesInvoked);

    /// <summary>
    /// Improved version of the DataGridView, contains bugfixes, many more options, issue solving and a layer architecture
    /// </summary>
    public partial class DataGridPro : DataGridView {

        #region Constructor
        
        /// <summary>
        /// DataGridPro is a prototype of DataGridView. It has many _bug fixes of it's original version and provides plenty of methods to help displaying the data (Adapter)
        /// </summary>
        public DataGridPro() {
            ColorEverySecondLine = false;
            base.DoubleBuffered = true;
            base.RowHeadersVisible = _rowHeadersVisible;
            AllowUserToResizeRows = false;
            AutoGenerateColumns = true;
            DataBindingComplete += _OnBindingCompleted;
            Paint += _OnPaint;
            RowsAdded += _OnRowsAdded;
            KeyDown += _OnKeyDown_LastItemEnterFix;
            CellDoubleClick += (sender, args) => {
                                   if (args.RowIndex == -1 || args.ColumnIndex == -1) return;
                                   if (EditMode == DataGridViewEditMode.EditOnEnter) return;
                                   CurrentCell = this[args.ColumnIndex, args.RowIndex];
                                   BeginEdit(false);
                               }; //used to force edit mode on double click.
            MouseClick += _OnRowHeadGridMouseClick;
            Font = new Font("Segoe UI", 8.125f);
            //CurrentCell = null; //The grid, when first initializing, incorrectly attempts to set the current cell to the first cell (0,0) which when done makes the first column visible. You can work around this by manually setting the CurrentCell property.
            #if DEBUG
            DataError += OnDataError;
            #endif
        }

        #endregion

        #region Properties
        private bool _fillColumnsToFitControl = true;
        private DataGridViewAutoSizeColumnsMode? _m_mode;
        private bool _rowHeadersVisible; //false by default
        private bool _colorEverySecondLine = true;
        private bool? _rtl;
        private bool _bound_mouseclick;
        private bool _onready_binded;
        private bool _ignoreErrors = true;
        internal readonly object lock_rdyformods = new object();
        internal int _onready_invokeCounts = 0;
        /// <summary>
        ///     Represents the name-changes of HeaderText of the columns inside of the dictionary
        /// </summary>
        internal readonly DictionaySelfModifier<string, string> _customNames = new DictionaySelfModifier<string, string>();
        /// <summary>
        ///     List of custom Widths that are static and unchangable
        /// </summary>
        internal readonly DictionaySelfModifier<string, int> _customWidths = new DictionaySelfModifier<string, int>();
        /// <summary>
        ///     Used to modify public properties of a specified column name, Key is the Name.
        ///     Value are properties of the column that are passed as anonymous typle to AnonymousProperties class
        /// </summary>
        internal readonly DictionaySelfModifierAggregatable<string, AnonymousProperties<Column>> _customizeColumns = new DictionaySelfModifierAggregatable<string, AnonymousProperties<Column>>();

        /// <summary>
        /// Provides the instance's grid. Used to display data from database and has many techniques to modify and display data. 
        /// <remarks>To create, start a new instance or use InitAdapter() method</remarks>
        /// </summary>
        public DataGridAdapter Adapter { get; set; }

        /// <summary>
        /// Ignore errors that are being thrown - default is true.
        /// </summary>
        public bool IgnoreErrors {
            get { return _ignoreErrors; }
            set { _ignoreErrors = value; }
        }

        /// <summary>
        ///     Streches the columns inside the grid to fit just the width of the control, while considering the content of the
        ///     column. default is true.
        ///     <remarks>If this is true, then ResizeMode of Column is set automatically to Fill</remarks>
        /// </summary>
        public bool FillColumnsToFitControl {
            get { return _fillColumnsToFitControl; }
            set {
                if (value) {
                    _m_mode = AutoSizeColumnsMode;
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    _fillColumnsToFitControl = true;
                } else {
                    if (_m_mode != null)
                        AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode) _m_mode;
                    _fillColumnsToFitControl = false;
                }
            }
        }
        public new bool RowHeadersVisible {
            get { return _rowHeadersVisible; }
            set { _rowHeadersVisible = base.RowHeadersVisible = value; }
        }
        /// <summary>
        /// Colors every second line with a bit darker white to make browsing easier.
        /// </summary>
        public bool ColorEverySecondLine {
            get { return _colorEverySecondLine; }
            set { _colorEverySecondLine = value; }
        }
        public bool IsRightToLeft {
            get {
                if (_rtl == null) return (_rtl = this.IsRightToLeft()) == true;
                return _rtl == true;
            }
        }
        /// <summary>
        ///     Represents the name-changes of HeaderText of the columns inside of the dictionary. To add new title change use
        ///     method 'ChangeTitle(string, string)';
        /// </summary>
        public ReadOnlyDictionary<string, string> CustomNames { get { return new ReadOnlyDictionary<string, string>(_customNames); } }
        [DefaultValue(false)]
        public bool IgnoreOnReadyForModifications { get; set; }


        #region Events

        /// <summary>
        ///     Invoked after DataBinding is loaded, look after the TimesInvoked parameter, because it may invoke multiple times on
        ///     unwanted occasions
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public event ReadyForModificationsHandler ReadyForModifications;

        private void _OnRowsAdded(object sender, DataGridViewRowsAddedEventArgs args) {
            if (ColorEverySecondLine)
                for (int i = args.RowIndex; i < args.RowIndex + args.RowCount; i++) {
                    if (i%2 != 1) continue;
                    foreach (Cell cell in Rows[i].Cells)
                        cell.Style.BackColor = Color.FromArgb(240, 240, 240);
                }
        }

        #endregion

        #endregion

        #region Methods

        #region Designing

        /// <summary>
        ///     Add a custom property that will be set on the Column. Properties like ReadOnly, Visible and so on.
        /// </summary>
        /// <param name="columnName">The Name of the column</param>
        /// <param name="AnonymousTypeProperties"></param>
        public void AddProperties(string columnName, dynamic AnonymousTypeProperties) {
            _customizeColumns.Add(columnName, new AnonymousProperties<Column>(AnonymousTypeProperties));
        }

        public void SetStaticWidth(string columnName, int width) {
            _customWidths.Add(columnName, width);
        }

        public void ChangeTitle(string columnName, string HeaderName) {
            _customNames.Add(columnName, HeaderName);
            if (_onready_invokeCounts > 1) {
                var column = Columns[columnName];
                if (column != null) column.HeaderText = HeaderName;
            }
        }

        public void AutoSizeColumns() {
            SuspendLayout();
            List<DataGridViewColumn> columns = GetDisplayedColumns().ToList();
            AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells); //resizes all to fit
            //set all static ones
            //AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            List<KeyValuePair<DataGridViewColumn, KeyValuePair<string, int>>> customized =
                columns.Combine(_customWidths, (key, val) => key.Name == val.Key).ToList(); //filter

            Rectangle _rect_last = GetCellDisplayRectangle(columns.Last().Index, -1, false);
            int _width_extraSpace = (IsRightToLeft) ? _rect_last.Left : Width - _rect_last.Right;
            if (_width_extraSpace > 0) {
                //if there is extra space
                int _width_foreveryone = _width_extraSpace/(columns.Count - customized.Count);
                IEnumerable<DataGridViewColumn> cc = columns.Except(customized.Select(kv => kv.Key));
                foreach (DataGridViewColumn column in cc)
                    column.Width += _width_foreveryone;
            }
            foreach (var kv in customized) //set the values
                kv.Key.Width = kv.Value.Value;
            ResumeLayout();
        }

        /// <summary>
        ///     Hides first column, if the first is the only column, sets Enabled = false!
        /// </summary>
        public void HideFirstColumn() {
            List<DataGridViewColumn> displayed = GetDisplayedColumnsOrdered().ToList();
            if (displayed.Count == 0)
                return;
            if (displayed.Count == 1) {
                displayed[0].Visible = false;
                    //todo consider adding dull column cuz visible=false doesnt hide first, not when databound... see: http://msdn.microsoft.com/en-us/library/system.windows.forms.datagridviewcolumn.minimumwidth.aspx
                return;
            }
            displayed[0].DisplayIndex = displayed.Count - 1;
        }

        #endregion

        /// <summary>
        /// Initializes a new adapter with given properties.
        /// </summary>
        public void InitAdapter(IDbConnection conn, IDbDataAdapter da, DbCommandBuilder builder) {
            Adapter = new DataGridAdapter(this, conn, da, builder);
        }

        public void SelectRow(int index, bool DeselectAll = false) {
            if (Rows.Count > index == false) return;
            if (MultiSelect == false || DeselectAll) //todo consider layout lock
                ClearSelection(); //todo fix, once it gets to the top it stops.
            Rows[index].Selected = true;
        }

        public void SelectColumn(int row, int columnIndex, bool DeselectAll = false) {
            if (Rows.Count > row == false || Columns.Cast<Column>().Any(i => i.Index == columnIndex) == false) return;
            if (MultiSelect == false || DeselectAll) //todo consider layout lock
                ClearSelection();
            Rows[row].Cells[columnIndex].Selected = true;
        }

        /*/// <summary> NOT WORKING WELL
        /// Returns the minimal height that all of the rows can fit into
        /// </summary>
        /// <returns></returns>
        public int GetMinimalGridHeight(int maximumHeight) {
            var divider = Rows[0].DividerHeight;
            //var a = RowHeadersWidth + Rows.Cast<Row>().Sum(r => r.Height + divider) + divider;
            var a = GetRowDisplayRectangle(Rows.GetLastRow(DataGridViewElementStates.Visible), false).Bottom;
            if (a > maximumHeight)
                return maximumHeight;
            return a;
        }*/

        // ReSharper disable LoopCanBeConvertedToQuery

        public IEnumerable<DataGridViewCell> GetCells(string columnName) {
            foreach (DataGridViewRow row in Rows)
                yield return row.Cells[columnName];
        }

        public IEnumerable<Column> GetDisplayedColumns() {
            foreach (Column column in Columns) {
                if (column.Visible)
                    yield return column;
            }
        }

        // ReSharper restore LoopCanBeConvertedToQuery
        public IEnumerable<Column> GetDisplayedColumnsOrdered()
        {
            return GetDisplayedColumns().OrderBy(c => c.DisplayIndex);
        }
        
        /// <summary>
        /// Finds the duplicates inside of a DataGridView, returning the index of the compares (key) and the list of equal indexes (value).
        /// </summary>
        /// <param name="columnNamesToCompare">Collection of the Column.Name to compare</param>
        public IEnumerable<KeyValuePair<int, List<int>>> FindDuplicates(ICollection<string> columnNamesToCompare) {
            var dupesLog = new List<int>(); //represents rows that were marked as duplicates
            var locDupes = new List<int>(); //collector for the yield return
            for (int i = 0; i < Rows.Count; i++) {
                if (dupesLog.Contains(i)) continue;
                locDupes.Clear();
                for (int j = 0; j < Rows.Count; j++) {
                    if (j == i) continue;
                    foreach (string column in columnNamesToCompare) {
                        if (Rows[i].Cells[column].Value.Equals(Rows[j].Cells[column].Value) == false)
                            goto _next;
                    }
                    //if it got to here, means it is true
                    locDupes.Add(j);
                    dupesLog.Add(j);
                    _next:
                    ;
                }
                if (locDupes.Count > 0)
                    yield return new KeyValuePair<int, List<int>>(i, locDupes);
            }
        }

        #region EndEdit

        /// <summary>
        ///     Finalizes an cell edit, calling both EndEdit() and EndEdit of data-source.
        /// </summary>
        public void ApplyEditAndEnd() {
            DataGridViewEditMode _editMode = EditMode;
            EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            if (Adapter != null && Adapter.BindingSource.AllowEdit)
                Adapter.BindingSource.EndEdit();
            EndEdit();
            EditMode = _editMode;
        }

        /// <summary>
        ///     Finalizes an cell edit, calling both EndEdit() and EndEdit of data-source.
        /// </summary>
        public void ApplyEditAndEnd(DataGridViewDataErrorContexts context) {
            DataGridViewEditMode _editMode = EditMode;
            EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            if (Adapter != null && Adapter.BindingSource.AllowEdit)
                Adapter.BindingSource.EndEdit();
            EndEdit(context);
            EditMode = _editMode;
        }


        [Obsolete("Use ApplyEditAndEnd() instead. this does not end edit of data-binding source!")]
        public new void EndEdit() {
            base.EndEdit();
        }

        [Obsolete("Use ApplyEditAndEnd(context) instead. this does not end edit of data-binding source!")]
        public new void EndEdit(DataGridViewDataErrorContexts context) {
            base.EndEdit();
        }

        #endregion

        #endregion

        #region Suspension of Layout

        public override void Sort(Column dataGridViewColumn, ListSortDirection direction) {
            SuspendLayout();
            base.Sort(dataGridViewColumn, direction);
            ResumeLayout();
        }

        public override void Sort(IComparer comparer) {
            SuspendLayout();
            base.Sort(comparer);
            ResumeLayout();
        }

        #endregion

        #region Reduant And DGV Bug Fixes

        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e) {
            base.OnDataBindingComplete(e);
            Console.WriteLine("DataBinding Completed");
        }


        /// <summary>
        /// Called when an error is thrown
        /// </summary>
        public virtual void OnDataError(object sender, DataGridViewDataErrorEventArgs args) {
            if (!IgnoreErrors)
                throw args.Exception;
        }

        /// <summary>
        /// Used to leave focus from the grid when a click is called outside of it.
        /// </summary>
        /// <param name="parent">the parent form containing all of the controls.</param>
        public void BindEndEditOnClickElsewhere([NotNull] Control parent) {
            if (_bound_mouseclick) return;
            Parent = parent;
            Parent.MouseClick += _OnMouseStopEdit;
            Parent.Controls.Cast<Control>().ForEach(c => c.MouseClick += _OnMouseStopEdit);
            _bound_mouseclick = true;
        }

        private void _OnRowHeadGridMouseClick(object sender, MouseEventArgs e) {
            var dgv = (DataGridPro) sender;
            HitTestInfo ht = dgv.HitTest(e.X, e.Y);
            if (ht.Type == DataGridViewHitTestType.RowHeader) //row header click fix
                dgv.ApplyEditAndEnd();
        }

        private void _OnMouseStopEdit(object sender, MouseEventArgs e) {
            if (sender.GetType() == typeof (DataGridPro)) {
                HitTestInfo ht = HitTest(e.X, e.Y);
                if (ht.Type == DataGridViewHitTestType.None && IsCurrentCellInEditMode)
                    ApplyEditAndEnd();
            } else {
                ApplyEditAndEnd();
            }
        }

        private void _OnKeyDown_LastItemEnterFix(object sender, KeyEventArgs e) {
            if ((Keys) e.KeyValue == Keys.Enter && SelectedRows.Count > 0 && SelectedRows[0].Index == Rows.Count - 1) {
                e.Handled = true;
                SelectRow(0, true);
            }
        }

        #endregion

        #region OnReadyForModifications
        //private readonly Semaphore _pool = new Semaphore(2, 2);
        private readonly object lock_attemptTolock = new object(); //this will make sure that _counter won't be accessed till _catchOnHandle() finishes

        protected virtual void OnReadyForModifications() {
            if (IgnoreOnReadyForModifications)
                return;
            /*_pool.WaitOne();*/
            lock (lock_attemptTolock) {
                //Set custom properties first, before design!
                var _i = Interlocked.Increment(ref _onready_invokeCounts);

                if (_i == 1) {
                    SuspendLayout();
                    var dic = Columns.Cast<Column>().Combine(_customizeColumns, (key, val) => key.Name == val.Key);
                    foreach (var kvp in dic)
                        kvp.Value.Value.ApplyProperties(kvp.Key);
                    ResumeLayout();
                }

                //Call event, also invoked PreDisplay() (design core).
                if (ReadyForModifications != null)
                    ReadyForModifications.Invoke(this, _i); //add then return

                #region AutoDesigners

                if (_i == 1) {
                    SuspendLayout();
                    foreach (var _cName in _customNames)
                        foreach (Column column in Columns) {
                            if (_cName.Key != column.Name)
                                continue;
                            column.HeaderText = _cName.Value;
                        }
                    ResumeLayout();
                }

                if (FillColumnsToFitControl && Adapter == null &&
                    MathTools.PrecentageDifferences(Width,
                        GetDisplayedColumns().Sum(dis => dis.Width + dis.DividerWidth/2d), Width/80d) == false)
                    AutoSizeColumns();
                //_pool.Release(1);
            }
        }

            #endregion


        private void _OnBindingCompleted(object sender,
            DataGridViewBindingCompleteEventArgs dataGridViewBindingCompleteEventArgs) {
            _onready_binded = true;
        }

        private void _OnPaint(object sender, PaintEventArgs paintEventArgs) {
            if (_onready_binded) {
                OnReadyForModifications();
                _onready_binded = false;
            }
        }

        #endregion
    }
}
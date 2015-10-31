
/*
 * nucs.Controls.DataGridPro - by nucs, (C) EB Programming 2013
 Todo Add option for static width, that wont change and will be included in calculations 
 Todo custom sort for custom layers and regular layers
 */


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using nucs.Collections;
using nucs.Collections.Extensions;
using nucs.Controls;
using nucs.SystemCore;
using Column = System.Windows.Forms.DataGridViewColumn;
using Row = System.Windows.Forms.DataGridViewRow;
using Cell = System.Windows.Forms.DataGridViewCell;

namespace nucs.ADO.NET {
    public delegate void BindingAndLoadingCompletedHandler(object sender, int rows);
    /* ORDER
        * 1-> Constructor
        * --wait-- //assigning properties now by user
        * 2->Load(); called (DataSource binding and db stuff)
        * 3->Process(); (adding columns from layerers)
        * {
        *      Finds Match->Creates a top layer -> ApplyingDesign() on it -> Adding it to db
        *      <Repeats for Layerers and ManualLayerers>
        * }
        * --waits for ReadyForModifications--
        * 4->Grid has been displayed and ready for modifications
        * 5->PreDisplay() called for modifying data 
        * {
        *      Processes Values -> Assigning them (Hides if wasn't able to figure out)
        *      Design Begins (Suspend Layout) -> Do custom design (width and readonly ..etc..) -> Resume Layout
        * }
        * 6-> DONE, Ready to display.
    */
    public class DataGridAdapter : IDisposable {

        #region Properties and Constructor
        private readonly DbCommandBuilder builder;
        private DataSet ds;
        private readonly object lock_design = new object();
        private bool _do_reorder = true;
        private bool _refresh_doPreDisplay;
        public IDbConnection Connection { get; set; }
        public IDbDataAdapter Adapter { get; set; }
        public DataGridPro DataGrid { get; set; }
        public BindingSource BindingSource { get; private set; }
        /// <summary>
        ///     Set to true after Adapter.Load(); (After binding, designing and display went successful).
        /// </summary>
        public bool IsBoundAndLoaded { get; set; }

        /// <summary>
        /// List of active layers that will be applied
        /// </summary>
        public readonly List<IColumnLayer<object, object>> Layerers = new List<IColumnLayer<object, object>>();
        /// <summary>
        /// List of active custom layers that will be applied
        /// </summary>
        public readonly ImprovedList<IManualColumnLayer<object>> ManualLayerers = new ImprovedList<IManualColumnLayer<object>>();

        public event BindingAndLoadingCompletedHandler BindingAndLoadingCompleted;
        public event BindingAndLoadingCompletedHandler LoadingCompleted;
        
        public DataGridAdapter(DataGridPro grid, IDbConnection connection, IDbDataAdapter da, DbCommandBuilder builder) {
            DataGrid = grid;
            Connection = connection;
            Adapter = da;
            this.builder = builder;
            grid.Sorted += (sender, args) => {
#if DEBUG
                               Console.WriteLine("SORTING");
#endif
                                   PreDisplay();
                           };
            grid.ReadyForModifications += _OnReadyForModifications;
        }
        
        private DataGridPro grid {
            get { return DataGrid; }
        }

        #endregion

        #region Design and Core

        /*///
        Hierchy before showing grid:
         * Insert Data (Once) to dataset - Collecting
         * Create Representive Layer -> Fill it corresponding to the data below it - Designing
         * Display Layers - (Least work) - Showing
        */
        
        private void Process() {
            //POST Binding
            DataGridViewColumn[] columns = grid.Columns.Cast<Column>().ToArray();
            foreach (DataGridViewColumn low in columns) {
                //todo PARALLELISM!! 
                IColumnLayer<object, object> _layer =
                    Layerers.FirstOrDefault(lay => String.Equals(lay.LowerLayerName, low.Name, StringComparison.InvariantCultureIgnoreCase));
                if (_layer == null) continue;
                _layer.LowerLayer = low;
                grid.Columns.Add(ApplyLayerStyling(_layer.CreateLayeredColumn(), _layer));
            }

            foreach (var layer in ManualLayerers) {
                //todo PARALLELISM!! 
                layer.LowerLayers =
                    layer.LowerLayersNames.Select(n => columns.FirstOrDefault(c => c.Name.Equals(n)))
                        .Where(c => c != null);
                grid.Columns.Add(ApplyLayerStyling(layer.CreateLayeredColumn(), layer));
            }
        }

        private Column ApplyLayerStyling(Column top, IColumnLayer<object, object> layer) {
            if (top.Name.Contains("_Layer") == false)
                throw new ArgumentException("layer '" + top + "' must contain '_Layer' in it's name");
            if (layer.LowerLayer.DisplayIndex == 0) {
                grid.HideFirstColumn();
                top.DisplayIndex = 0;
            }

            if (grid._customNames.ContainsKey(top.Name)) //Apply Name
                top.HeaderText = grid._customNames[top.Name];

            if (grid._customizeColumns.ContainsKey(top.Name)) //Apply custom property
                grid._customizeColumns[top.Name].ApplyProperties(top);

            layer.LowerLayer.Visible = layer.AreLowerVisible;
            top.ReadOnly = layer.IsReadOnly;
            return top;
        }

        private Column ApplyLayerStyling(Column top, IManualColumnLayer<object> layer) {
            if (top.Name.Contains("_CLayer") == false)
                throw new ArgumentException("layer '" + top.Name + "' must contain '_CLayer' in it's name");
            if (layer.LowerLayers.FirstOrDefault(c => c.DisplayIndex == 0) != null) {
                grid.HideFirstColumn();
                top.DisplayIndex = 0;
            }

            if (grid._customNames.ContainsKey(top.Name)) //Apply Name
                top.HeaderText = grid._customNames[top.Name];

            if (grid._customizeColumns.ContainsKey(top.Name)) //Apply custom property
                grid._customizeColumns[top.Name].ApplyProperties(top);

            layer.LowerLayers.ForEach(l => l.Visible = layer.AreLowerVisible);
            layer.LowerLayers.ForEach(l => l.ReadOnly = layer.IsReadOnly);
            top.ReadOnly = layer.IsReadOnly;
            return top;
        }
        
        private void _OnReadyForModifications(object sender, int timesInvoked) {
            if (timesInvoked == 1 || _refresh_doPreDisplay) {
                PreDisplay();
            }
        }

        protected virtual void PreDisplay() { //todo PARALLELISM!!!!!!!!
            lock (lock_design) {
                _refresh_doPreDisplay = false;
                Console.WriteLine("PRE DISPLAY");
                grid.IgnoreOnReadyForModifications = true;
                grid.SuspendLayout();

                #region Value Assigning

                foreach (var _l in Layerers) {
                    foreach (Row row in grid.Rows) {
                        if (row.IsNewRow) continue;
                        object mod = _l.ModifyItem(row.Cells[_l.LowerLayer.Name].Value);
                        if (mod == null || string.IsNullOrEmpty(mod.ToString())) {
                            grid.CurrentCell = null;
                            row.Visible = false;
                            continue;
                        }
                        row.Cells[_l.TopLayer.Name].Value = mod;
                    }
                }

                foreach (var _l in ManualLayerers) {
                    foreach (Row row in grid.Rows) {
                        if (row.IsNewRow) continue;
                        object mod = _l.ModifyItem(_l.LowerLayersNames.ToDictionary(n => n, n => row.Cells[n].Value));
                        if (mod == null || string.IsNullOrEmpty(mod.ToString())) {
                            grid.CurrentCell = null;
                            row.Visible = false;
                            continue;
                        }
                        row.Cells[_l.TopLayer.Name].Value = mod;
                    }
                }

                #endregion

                #region Desinging

                foreach (var _l in Layerers) {
                    if (_l.IsReadOnly == false) continue; //false is default to all
                    foreach (Row row in grid.Rows) {
                        row.Cells[_l.TopLayer.Name].ReadOnly = true;
                    }
                }

                foreach (var _l in ManualLayerers) {
                    if (_l.IsReadOnly == false) continue; //false is default to all
                    foreach (Row row in grid.Rows) {
                        row.Cells[_l.TopLayer.Name].ReadOnly = true;
                    }
                }

                if (grid.FillColumnsToFitControl && (_do_reorder) /* && SingleRun.HasAlreadyRan(GetHashCode())*/) {
                    grid.AutoSizeColumns();
                    _do_reorder = false;
                }

                if (LoadingCompleted != null)
                    LoadingCompleted(grid, grid.DisplayedRowCount(true));

                #endregion

                grid.ResumeLayout();
                grid.IgnoreOnReadyForModifications = false;
            }
        }

        public virtual void PreCommit() {
            //todo PARALLELISM!!
            grid.SuspendLayout();

            foreach (var _l in Layerers) {
                foreach (Row row in grid.Rows) {
                    if (row.IsNewRow) {
                        try {
                            object _dem = _l.DemodifyItem(row.Cells[_l.TopLayer.Name].Value);
                            if (_dem == null) continue;
                            row.Cells[_l.LowerLayer.Name].Value = _dem;
                        } catch {}
                        continue;
                    }
                    object dem = _l.DemodifyItem(row.Cells[_l.TopLayer.Name].Value);
                    if (dem == null) continue;
                    row.Cells[_l.LowerLayer.Name].Value = dem;
                }
            }


            foreach (var _l in ManualLayerers) {
                foreach (Row row in grid.Rows) {
                    if (row.IsNewRow) {
                        try {
                            IDictionary<string, object> _demodifed = _l.DemodifyItem(row.Cells[_l.TopLayer.Name]);
                            if (_demodifed == null) continue;
                            foreach (DataGridViewColumn layer in _l.LowerLayers) {
                                row.Cells[layer.Name].Value = _demodifed[layer.Name];
                            }
                        } catch {}
                        continue;
                    }
                    IDictionary<string, object> demodifed = _l.DemodifyItem(row.Cells[_l.TopLayer.Name]);
                    if (demodifed == null) continue;
                    foreach (DataGridViewColumn layer in _l.LowerLayers) {
                        row.Cells[layer.Name].Value = demodifed[layer.Name];
                    }
                }
            }

            grid.ResumeLayout();
        }

        #endregion

        #region Database Operations
#if !NET_4_5
        private bool singrun = false;
#endif
        public int Load() {
#if !NET_4_5
            if (singrun) return -1;
            singrun=true;
#else
            if (SingleRun.HasAlreadyRan(GetHashCode()))
                return 0;
#endif
            ds = new DataSet();
            Open();
            int res = Adapter.Fill(ds);
            Close();
            BindingSource = new BindingSource(ds, ds.Tables[0].TableName);
            grid.DataSource = BindingSource;
            Process();
            if (BindingAndLoadingCompleted != null)
                BindingAndLoadingCompleted(this, res);
            IsBoundAndLoaded = true;
            return res;
        }

        public int Refresh() {
            ds.Clear();
            Open();
            int res = Adapter.Fill(ds);
            Close();
            #region Design
            _do_reorder = true;
            _refresh_doPreDisplay = true;
            #endregion

            return res;
        }

        public void Save() {
            if (Layerers.Count == 0 && ManualLayerers.Count == 0 && ds.HasChanges() == false)
                return;
            grid.ApplyEditAndEnd();
            PreCommit();
            if (ds.HasChanges() == false) {
                return;
            }

            Adapter.UpdateCommand = builder.GetUpdateCommand(); //might be reduant
            Adapter.Update(ds);
            _refresh_doPreDisplay = true;
            //_do_reorder = true;
            Open();
            ds.AcceptChanges();
            Close();
        }

        #region Connection Methods

        private void Open() {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
        }

        private void Close() {
            if (Connection.State != ConnectionState.Broken && Connection.State != ConnectionState.Closed)
                Connection.Close();
        }

        #endregion

        #endregion

        #region Implementations

        public void Dispose() {
            //todo remove layers, show low-layers
            Connection.Close();
            Connection.Dispose();
            Layerers.Clear();
            ManualLayerers.Clear();
        }

        public override int GetHashCode() {
            return Adapter.GetHashCode() ^ Connection.GetHashCode() ^ Adapter.GetHashCode() ^ DataGrid.GetHashCode();
        }

        #endregion

        /// <summary>
        ///     Invoked once after DataSource is binded. This can only happen before Adapter.Load() is called.
        /// </summary>
    }

    #region Helping and Representive classes

    #region Interfaces

    /// <summary>
    ///     Layerer for single top layer, that uses single lower layer
    /// </summary>
    public interface IColumnLayer<TBot, TTop> {
        string RepresentiveName { get; }
        string CodeName { get; }
        string LowerLayerName { get; }
        Regex Format { get; }
        string FormatError { get; }
        Column LowerLayer { get; set; }
        Column TopLayer { get; set; }

        #region Design Properties

        bool AreLowerVisible { get; }
        bool IsReadOnly { get; }

        #endregion

        Column CreateLayeredColumn();
        TTop ModifyItem(TBot tbot);
        TBot DemodifyItem(TTop ttop);
    }

    /// <summary>
    ///     Layerer for single top layer, that uses value of multiple lower layers
    /// </summary>
    public interface IManualColumnLayer<TTop> {
        string RepresentiveName { get; }
        string CodeName { get; }
        IList<string> LowerLayersNames { get; }
        Regex Format { get; }
        string FormatError { get; }
        IEnumerable<Column> LowerLayers { get; set; }
        Column TopLayer { get; set; }

        #region Design Properties

        bool AreLowerVisible { get; }
        bool IsReadOnly { get; }

        #endregion

        Column CreateLayeredColumn();
        TTop ModifyItem(IDictionary<string, object> tbot);

        /// <summary>
        ///     Processes the top value, and by it, returns a dictionary of: -LowerLayerName,It'sValue-
        /// </summary>
        IDictionary<string, object> DemodifyItem(TTop ttop);
    }

    #endregion

    #region Bases

    /// <summary>
    ///     Creates a layer teoretically over a column that was bound, so you can customize the way you display easly.
    /// </summary>
    /// <typeparam name="TBot">The type of the lower column (original column)</typeparam>
    /// <typeparam name="TTop">The type of the upper column which is the representor</typeparam>
    /// <typeparam name="TColumn">The type of the column; e.g. DataGridViewTextBoxColumn/CheckBoxColumn</typeparam>
    public class ColumnLayer<TBot, TTop, TColumn> : IColumnLayer<object, object> where TColumn : Column, new() {
        public delegate bool ModifyItemForGrid(TBot input, out TTop output);

        public delegate bool ModifyItemFromGrid(out TBot output, TTop input);

        public ModifyItemFromGrid Demodifier;
        public ModifyItemForGrid Modifier;
        private string _codeName;
        private ColumnLayer() {}

        /// <summary>
        ///     Simple one->one layer, make sure to add it to DataGridPro1.Adapter.Layerers.
        /// </summary>
        /// <param name="lowerLayerName">The name of the lower column, which holds the original data</param>
        /// <param name="representiveName">The header text of the TopLayer</param>
        /// <param name="codeName">The name of the column (NOT Header Text)</param>
        /// <param name="modifier">
        ///     An action to modify the data to suit the top layer display. Can be null only if 'demodifier' is
        ///     null either, inorder to leave the data as it is
        /// </param>
        /// <param name="demodifier">
        ///     An action to reverse modification from the top layer to the original layer (lowerLayer).  Can
        ///     be null only if 'modifier' is null either, inorder to leave the data as it is
        /// </param>
        public ColumnLayer(string lowerLayerName, string representiveName, string codeName, ModifyItemForGrid modifier,
            ModifyItemFromGrid demodifier) : this() {
            RepresentiveName = representiveName;
            Modifier = modifier;
            Demodifier = demodifier;
            LowerLayerName = lowerLayerName;
            _codeName = codeName;
        }

        #region Design Properties

        private bool _areLowerVisible = true;

        public bool AreLowerVisible {
            get { return _areLowerVisible; }
            set { _areLowerVisible = value; }
        }

        public bool IsReadOnly { get; set; }

        #endregion

        public string TopLayerName {
            get { return TopLayer == null ? CodeName : TopLayer.Name; }
        }

        public string RepresentiveName { get; private set; }
        public string LowerLayerName { get; private set; }
        public Regex Format { get; set; }
        public string FormatError { get; set; }
        public Column LowerLayer { get; set; }
        public Column TopLayer { get; set; }

        public string CodeName {
            get {
                if (TopLayer == null)
                    return _codeName.Contains("_Layer") ? _codeName : _codeName = (_codeName + "_Layer");
                return TopLayerName;
            }
            set { if (TopLayer == null) _codeName = value.Contains("_Layer") ? value : value + "_Layer"; }
        }

        public virtual Column CreateLayeredColumn() {
            if (LowerLayer == null)
                throw new InvalidOperationException(
                    "You cannot create LayeredColumn unless you set LowerLayer to a column");
            LowerLayer.ValueType = typeof (TBot);
            var t = new TColumn();
            t.ValueType = typeof (TTop);
            t.HeaderText = RepresentiveName;
            t.Name = CodeName;
            t.DataPropertyName = null;
            return TopLayer = t;
        }

        object IColumnLayer<object, object>.ModifyItem(object tbot) {
            if (Modifier == null)
                return tbot;
            TTop output;
            if (_ModifyItem(((TBot) tbot), out output) == false)
                return null;
            return output;
        }

        object IColumnLayer<object, object>.DemodifyItem(object ttop) {
            if (Demodifier == null)
                return ttop;
            TBot output;
            if (_DemodifyItem((TTop) ttop, out output) == false)
                return null;
            return output;
        }

        public virtual void ErrorFormatSetup(DataGridView grid) {
            grid.CellValidating += (sender, args) => {
                                       if (Format != null && TopLayer != null && args.ColumnIndex == TopLayer.Index &&
                                           !Format.IsMatch(args.FormattedValue.ToString())) {
                                           grid[args.ColumnIndex, args.RowIndex].ErrorText =
                                               string.IsNullOrEmpty(FormatError)
                                                   ? "Error parsing to the following regex: " + Format.ToString()
                                                   : FormatError;
                                       }
                                   };
        }

        public virtual bool _ModifyItem(TBot value, out TTop ttop) {
            if (Modifier == null) {
                ttop = (TTop) ((Object) value);
                return true;
            }
            return Modifier(value, out ttop);
        }

        public virtual bool _DemodifyItem(TTop value, out TBot output) {
            if (Demodifier == null) {
                output = (TBot) ((Object) value);
                return true;
            }
            return Demodifier(out output, value);
        }
    }

    /// <summary>
    ///     Layerer for single top layer, that uses value of multiple lower layers
    /// </summary>
    public abstract class ManualColumnLayeringBase<TTop> : IManualColumnLayer<object> {
        private string _codeName;

        protected ManualColumnLayeringBase(string representiveName, string codeName, params string[] lowerLayerNames) {
            AreLowerVisible = false;
            LowerLayersNames = lowerLayerNames.ToList();
            RepresentiveName = representiveName;
            CodeName = codeName;
        }

        public string TopLayerName {
            get { return TopLayer == null ? CodeName : TopLayer.Name; }
        }

        public string TopLayerHeaderText {
            get { return TopLayer == null ? "" : TopLayer.HeaderText; }
            set { if (TopLayer != null) TopLayer.HeaderText = value; }
        }

        public string RepresentiveName { get; protected set; }
        public IList<string> LowerLayersNames { get; protected set; }
        public Regex Format { get; set; }
        public string FormatError { get; set; }
        public IEnumerable<Column> LowerLayers { get; set; }
        public Column TopLayer { get; set; }

        public string CodeName {
            get {
                if (TopLayer == null)
                    return _codeName.Contains("_CLayer") ? _codeName : _codeName = (_codeName + "_CLayer");
                return TopLayerName;
            }
            set { if (TopLayer == null) _codeName = value.Contains("_CLayer") ? value : value + "_CLayer"; }
        }

        public abstract Column CreateLayeredColumn();

        /// <summary>
        ///     Returns TTop
        /// </summary>
        object IManualColumnLayer<object>.ModifyItem(IDictionary<string, object> tbot) {
            TTop output;
            if (ModifyItem(tbot, out output))
                return output;
            return null;
        }

        /// <summary>
        ///     Accepts TTop
        /// </summary>
        IDictionary<string, object> IManualColumnLayer<object>.DemodifyItem(object ttop) {
            IDictionary<string, object> output;
            if (ttop is Cell) {
                return DemodifyItem((TTop) (((Cell) ttop).Value), out output) ? output : null;
            }
            return DemodifyItem((TTop) ttop, out output) == false ? null : output;
        }

        #region Design Properties

        public bool AreLowerVisible { get; protected set; }
        public bool IsReadOnly { get; protected set; }

        #endregion

        public virtual Column DefaultLayeredColumn<TColumnType>() where TColumnType : Column, new() {
            var t = new TColumnType();
            t.ValueType = typeof (TTop);
            t.HeaderText = RepresentiveName;
            t.Name = CodeName;
            t.DataPropertyName = null;
            return t;
        }

        public virtual void ErrorFormatSetup(DataGridView grid) {
            grid.CellValidating += (sender, args) => {
                                       if (Format != null && TopLayer != null && args.ColumnIndex == TopLayer.Index &&
                                           !Format.IsMatch(args.FormattedValue.ToString())) {
                                           grid[args.ColumnIndex, args.RowIndex].ErrorText =
                                               string.IsNullOrEmpty(FormatError)
                                                   ? "Error to the following regex: " + Format.ToString()
                                                   : FormatError;
                                       }
                                   };
        }

        public abstract bool ModifyItem(IDictionary<string, object> tbot, out TTop top);
        public abstract bool DemodifyItem(TTop ttop, out IDictionary<string, object> output);
    }

    #endregion

    #endregion
}


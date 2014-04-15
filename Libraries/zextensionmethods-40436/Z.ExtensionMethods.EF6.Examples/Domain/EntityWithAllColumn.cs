using System;
using System.Data.Entity.Spatial;

namespace Z.ExtensionMethods.EF6.Examples
{
    public class EntityWithAllColumn
    {
        public int ID { get; set; }
        public long? BitIntColumn { get; set; }
        public byte[] BinaryColumn { get; set; }
        public bool? BitColumn { get; set; }
        public string CharColumn { get; set; }
        public DateTime? DateColumn { get; set; }
        public DateTime? DateTimeColumn { get; set; }
        public DateTime? DateTime2Column { get; set; }
        public DateTimeOffset? DateTimeOffsetColumn { get; set; }
        public decimal? DecimalColumn { get; set; }
        public double? FloatColumn { get; set; }
        public byte[] ImageColumn { get; set; }
        public int? IntColumn { get; set; }
        public decimal? MoneyColumn { get; set; }
        public string NCharColumn { get; set; }
        public string NTextColumn { get; set; }
        public decimal? NumericColumn { get; set; }
        public string NVarcharColumn { get; set; }
        public string NVarcharMaxColumn { get; set; }
        public float? RealColumn { get; set; }
        public DateTime? SmallDateTimeColumn { get; set; }
        public short? SmallIntColumn { get; set; }
        public decimal? SmallMoneyColumn { get; set; }
        public string TextColumn { get; set; }
        public TimeSpan? TimeColumn { get; set; }
        public byte[] TimestampColumn { get; set; }
        public byte? TinyIntColumn { get; set; }
        public Guid? UniqueIdentifierColumn { get; set; }
        public byte[] VarBinaryColumn { get; set; }
        public byte[] VarBinaryMaxColumn { get; set; }
        public string VarcharColumn { get; set; }
        public string VarcharMaxColumn { get; set; }
        public string XmlColumn { get; set; }
    }
}
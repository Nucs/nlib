namespace Z.ExtensionMethods.EF6.Examples
{
    public class ComplexType
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public ComplexTypeInfo1 Info1 { get; set; }
    }

    public class ComplexTypeInfo1
    {
        public string DescriptionInfo1 { get; set; }
        public ComplexTypeInfo2 Info2 { get; set; }
    }

    public class ComplexTypeInfo2
    {
        public string DescriptionInfo2 { get; set; }
    }
}
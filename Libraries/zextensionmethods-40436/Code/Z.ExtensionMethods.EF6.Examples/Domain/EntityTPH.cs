namespace Z.ExtensionMethods.EF6.Examples
{
    public class EntityTPH
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class EntityTPH1 : EntityTPH
    {
        public string DescriptionTPH1 { get; set; }
    }

    public class EntityTPH2 : EntityTPH
    {
        public string DescriptionTPH2 { get; set; }
    }
}
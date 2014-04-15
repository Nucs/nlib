using System;

namespace Z.ExtensionMethods.EF6.Examples
{
    public abstract class Entity : EntityWithTypeId<int>
    {
    }

    public abstract class EntityWithTypeId<T>
    {
        public T ID { get; set; }
    }

    public class EntityWithTypeIdGuid : EntityWithTypeId<Guid>
    {
        public string Description { get; set; }
    }

    public class EntityWithTypeIdInt : Entity
    {
        public string Description { get; set; }
    }
}
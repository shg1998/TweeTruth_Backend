namespace Entities.Common
{
    public interface IEntity
    {

    }

    public abstract class BaseEntity<T>:IEntity
    {
        public T Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int>
    {
    }
}

namespace MasterOfCentauri.Managers
{
    public interface IEventListener<in T>
    {
        void EventRaised(T eventObject);
    }
}
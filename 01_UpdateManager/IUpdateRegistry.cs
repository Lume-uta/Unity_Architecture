namespace Core.UpdateService
{
    public interface IUpdateRegistry { void Tick(); }
    public interface ILateUpdateRegistry { void LateTick(); }
    public interface IFixedUpdateRegistry { void FixedTick(); }
}
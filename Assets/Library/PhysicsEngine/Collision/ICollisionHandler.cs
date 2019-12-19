using Library.PhysicsEngine.Data;

namespace Library.PhysicsEngine.Collision
{
    public interface ICollisionHandler
    {
        bool CanHandle(Shape a, Shape b);
        void Handle(ref Manifold manifold, Rigidbody.Rigidbody a, Rigidbody.Rigidbody b);
    }
}
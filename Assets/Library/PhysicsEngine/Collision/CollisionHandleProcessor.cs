using Library.PhysicsEngine.Data;

namespace Library.PhysicsEngine.Collision
{
    public static class CollisionHandleProcessor
    {
	    private static readonly ICollisionHandler[] CollisionHandlers = 
	    {
		    new CircleVsCircleHandler(),
		    new CircleVsPolygonHandler()
	    };
	    
        public static void Handle(ref Manifold manifold, Rigidbody.Rigidbody a, Rigidbody.Rigidbody b)
        {
	        for (int i = 0; i < CollisionHandlers.Length; i++)
	        {
		        if (CollisionHandlers[i].CanHandle(a.Shape, b.Shape))
		        {
			        CollisionHandlers[i].Handle(ref manifold, a, b);
			        break;
		        }
	        }
        }
    }
}
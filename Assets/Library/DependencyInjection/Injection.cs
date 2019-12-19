using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Assertions;

namespace Library.DependencyInjection
{
	public class Injector : IInjectable
	{
		public Injector( Injector parent = null )
		{
			_parentInjector = parent;
			Bind<Injector>( this );
		}

		private readonly Injector _parentInjector;

		private readonly Dictionary<System.Type, object> _objects = new Dictionary<System.Type, object>();

		public void Bind<T>( T obj ) where T : IInjectable
		{
			var type = typeof(T);

			_objects[ type ] = obj;
		}

		public void PostBindings()
		{
			foreach (var objKvp in _objects)
			{
				Inject( objKvp.Value );
			}
		}

		public void Inject( object obj )
		{
			var fields = Reflector.Reflect( obj.GetType() );
			var fieldsLength = fields.Length;
			for (var i = 0; i < fieldsLength; i++)
			{
				var field = fields[ i ];
				var value = Get( field.FieldType );
				field.SetValue( obj, value );
			}
		}

		private object Get( System.Type type )
		{
			object obj = null;

			if (!_objects.TryGetValue( type, out obj ))
			{
				if (_parentInjector != null)
				{
					return _parentInjector.Get( type );
				}

				throw new InjectorException( "Could not get " + type.FullName + " from injector" );
			}

			return obj;
		}

		public T GetInstance<T>()
		{
			return (T)Get( typeof(T) );
		}

		private static class Reflector
		{
			private static readonly System.Type _injectAttributeType = typeof(Inject);
			private static readonly Dictionary<System.Type, FieldInfo[]> cachedFieldInfos = new Dictionary<System.Type, FieldInfo[]>();
			private static readonly List<FieldInfo> _reusableList = new List<FieldInfo>( 1024 );

			public static FieldInfo[] Reflect( System.Type type )
			{
				Assert.AreEqual( 0, _reusableList.Count, "Reusable list in Reflector was not empty!" );

				FieldInfo[] cachedResult;
				if (cachedFieldInfos.TryGetValue( type, out cachedResult ))
				{
					return cachedResult;
				}

				var fields = type.GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy );
				for (var fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
				{
					var field = fields[ fieldIndex ];
					var hasInjectAttribute = field.IsDefined( _injectAttributeType, inherit: false );
					if (hasInjectAttribute)
					{
						_reusableList.Add( field );
					}
				}
				var resultAsArray = _reusableList.ToArray();
				_reusableList.Clear();
				cachedFieldInfos[ type ] = resultAsArray;
				return resultAsArray;
			}
		}
	}
}
namespace Utterance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public delegate object ObjectFactory(params object[] args);

	public delegate T ObjectFactory<T>(params object[] args);

	/// <summary>
	/// Factory constructs an ObjectFactory delegate instance for a given type.
	/// There are a number of static methods on Factory that provide different input
	/// criteria for constructing ObjectFactory lambdas.
	/// Factory includes factory methods for both pre-compiled expressions and compiled delegates
	/// using both generic and non-generic approaches.
	/// </summary>
	public static class Factory
	{
		private static readonly ParameterExpression ArgsParameter = Expression.Parameter(typeof(object[]), "args");

		#region Anonymous
		/// <summary>
		/// Constructs a new Expression lambda of ObjectFactory for a type and given argument types.
		/// </summary>
		/// <param name="type">Type to be constructed by the ObjectFactory delegate</param>
		/// <param name="argumentTypes">Types to be used for parameter types</param>
		/// <returns>Anonymous ObjectFactory delegate</returns>
		public static Expression<ObjectFactory> NewExpression(Type type, params Type[] argumentTypes)
		{
			return (Expression<ObjectFactory>)FromTypes(type, argumentTypes ?? new Type[0]).Value;
		}

		/// <summary>
		/// Constructs a new ObjectFactory for a type with the given argumentTypes for parameters.
		/// </summary>
		/// <param name="type">Type to be constructed by the ObjectFactory delegate</param>
		/// <param name="argumentTypes">Type array of parameter types</param>
		/// <returns>Anonymous ObjectFactory delegate</returns>
		public static ObjectFactory New(Type type, params Type[] argumentTypes)
		{
			return (ObjectFactory)FromTypes(type, argumentTypes).Compiled;
		}

		/// <summary>
		/// Constructs a new Expression lambda of type ObjectFactory using a given ConstructorInfo for type initialization.
		/// </summary>
		/// <param name="ctor">ConstructorInfo to use for type instantiation</param>
		/// <returns>Anonymous ObjectFactory delegate</returns>
		public static Expression<ObjectFactory> NewExpression(ConstructorInfo ctor)
		{
			return (Expression<ObjectFactory>)FromConstructor(ctor).Value;
		}

		/// <summary>
		/// Constructs a new compiled anonymous ObjectFactory that instantiates types using the provided ConstructorInfo.
		/// </summary>
		/// <param name="ctor">ConstructorInfo to use in type instantiation</param>
		/// <returns>Anonymous ObjectFactory delegate</returns>
		public static ObjectFactory New(ConstructorInfo ctor)
		{
			return (ObjectFactory)FromConstructor(ctor).Compiled;
		}
		#endregion Anonymous
		#region Generic
		/// <summary>
		/// Constructs a new strong typed ObjectFactory[T] LambdaExpression with given argument types
		/// for construction parameters.
		/// </summary>
		/// <typeparam name="T">Type to be invoked by ObjectFactory delegate</typeparam>
		/// <param name="argumentTypes">Type array of arguments used for type initialization</param>
		/// <returns></returns>
		public static Expression<ObjectFactory<T>> NewExpression<T>(params Type[] argumentTypes)
		{
			return (Expression<ObjectFactory<T>>)FromTypes<T>(typeof(T), argumentTypes ?? new Type[0]).Value;
		}

		/// <summary>
		/// Constructs a new strong typed ObjectFactory[T] LambdaExpression with given argument types
		/// for construction parameters. Takes a seperate type as input for detailing which type to construct
		/// in the ObjectFactory delegate. This type must be assignable to <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">Type to be returned by the ObjectFactory delegate</typeparam>
		/// <param name="type">Type to be instantiated by the ObjectFactory delegate</param>
		/// <param name="argumentTypes">Type parameters for the delegate construction</param>
		/// <returns>Expression lambda of type ObjectFactory[T]</returns>
		public static Expression<ObjectFactory<T>> NewExpression<T>(Type type, Type[] argumentTypes)
		{
			return (Expression<ObjectFactory<T>>)FromTypes<T>(type, argumentTypes ?? new Type[0]).Value;
		}

		/// <summary>
		/// Constructs a new strong typed ObjectFactory[T] LambdaExpression with all type construction
		/// details defined by a given ConstructorInfo. ConstructorInfo must be for a type assignable to
		/// <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">Type to be returned by the ObjectFactory delegate</typeparam>
		/// <param name="ctor">ConstructorInfo instance for a type derived from <typeparamref name="T"/></param>
		/// <returns>Expression lambda of type ObjectFactory[T]</returns>
		public static Expression<ObjectFactory<T>> NewExpression<T>(ConstructorInfo ctor)
		{
			return (Expression<ObjectFactory<T>>)FromConstructor<T>(ctor).Value;
		}

		/// <summary>
		/// Constructs a new compiled ObjectFactory[T] with the given argument types as parameter types
		/// </summary>
		/// <typeparam name="T">Type to be created by the ObjectFactory delegate</typeparam>
		/// <param name="parameterTypes">Types of the parameters for type construction</param>
		/// <returns>Compiled ObjectFactory[T] instance</returns>
		public static ObjectFactory<T> New<T>(params Type[] parameterTypes)
		{
			return (ObjectFactory<T>)FromTypes<T>(typeof(T), parameterTypes ?? new Type[0]).Compiled;
		}

		/// <summary>
		/// Constructs a new compiled ObjectFactory[T] with the given argument types as parameter types
		/// and an internal constructed type of <paramref name="type"/>
		/// </summary>
		/// <typeparam name="T">Type to be returned by the ObjectFactory delegate</typeparam>
		/// <param name="type">Type to be instantiated by the ObjectFactory delegate</param>
		/// <param name="parameterTypes">Types of the parameters used for constructing <paramref name="type"/></param>
		/// <returns>Compiled ObjectFactory[T] instance</returns>
		public static ObjectFactory<T> New<T>(Type type, Type[] parameterTypes)
		{
			return (ObjectFactory<T>)FromTypes<T>(type, parameterTypes ?? new Type[0]).Compiled;
		}

		/// <summary>
		/// Constructs a new compiled ObjectFactory[T] with the given ConstructorInfo as the sourced of
		/// details for type creation. The owning type of the ConstructorInfo must be assignable to <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">Type to be returned by the ObjectFactory delegate</typeparam>
		/// <param name="ctor">ConstructorInfo to be used in creating type instances</param>
		/// <returns>Compiled ObjectFactory[T] instance</returns>
		public static ObjectFactory<T> New<T>(ConstructorInfo ctor)
		{
			return (ObjectFactory<T>)FromConstructor<T>(ctor).Compiled;
		}

		#endregion Generic
		#region Helpers
		private static LambdaCacheItem<FactoryKey> FromTypes(Type type, Type[] parameterTypes)
		{
			var key = new FactoryKey(type, type, parameterTypes);
			return _anonymousFactoryCache.GetOrAdd(key, k =>
			{
				NewExpression newExp;
				if (parameterTypes.Length == 0)
				{
					newExp = Expression.New(type);
				}
				else
				{
					newExp =
						Expression.New(type.GetConstructor(parameterTypes), parameterTypes.Select((p, i) =>
							Expression.Convert(Expression.ArrayIndex(ArgsParameter, Expression.Constant(i)), p)));
				}
				return Expression.Lambda<ObjectFactory>(newExp, ArgsParameter);
			});
		}

		private static LambdaCacheItem<FactoryKey> FromTypes<T>(Type type, Type[] parameterTypes)
		{
			var key = new FactoryKey(typeof(T), type, parameterTypes);
			return _anonymousFactoryCache.GetOrAdd(key, k =>
			{
				NewExpression newExp;
				if (parameterTypes.Length == 0)
				{
					newExp = Expression.New(type);
				}
				else
				{
					newExp =
						Expression.New(type.GetConstructor(parameterTypes), parameterTypes.Select((p, i) =>
							Expression.Convert(Expression.ArrayIndex(ArgsParameter, Expression.Constant(i)), p)));
				}
				return Expression.Lambda<ObjectFactory<T>>(newExp, ArgsParameter);
			});
		}

		private static LambdaCacheItem<FactoryKey> FromConstructor(ConstructorInfo ctor)
		{
			var parameters = ctor.GetParameters();
			var parameterTypes = parameters.Select(p => p.ParameterType).ToArray();

			var key = new FactoryKey(ctor.DeclaringType, ctor.DeclaringType, parameterTypes);
			return _anonymousFactoryCache.GetOrAdd(key, k =>
			{
				NewExpression newExp =
					Expression.New(ctor, parameterTypes.Select((p, i) =>
						Expression.Convert(Expression.ArrayIndex(ArgsParameter, Expression.Constant(i)), p)));
				return Expression.Lambda<ObjectFactory>(newExp, ArgsParameter);
			});
		}

		private static LambdaCacheItem<FactoryKey> FromConstructor<T>(ConstructorInfo ctor)
		{
			var parameters = ctor.GetParameters();
			var parameterTypes = parameters.Select(p => p.ParameterType).ToArray();

			var key = new FactoryKey(typeof(T), ctor.DeclaringType, parameterTypes);
			return _anonymousFactoryCache.GetOrAdd(key, k =>
			{
				NewExpression newExp =
					Expression.New(ctor, parameterTypes.Select((p, i) =>
						Expression.Convert(Expression.ArrayIndex(ArgsParameter, Expression.Constant(i)), p)));
				return Expression.Lambda<ObjectFactory<T>>(newExp, ArgsParameter);
			});
		}
		#endregion Helpers

		private static readonly LambdaCache<FactoryKey> _anonymousFactoryCache = new LambdaCache<FactoryKey>();

		private class FactoryKey : IEquatable<FactoryKey>
		{
			private static readonly FNV1aHash _hashCodeGenerator = new FNV1aHash();

			private readonly int _hashCode;
			private readonly Type[] _types;

			public FactoryKey(Type castType, Type type, Type[] parameterTypes)
				: this(new Type[] { castType, type }, parameterTypes)
			{
			}

			public FactoryKey(Type[] types, Type[] parameterTypes)
			{
				_types = parameterTypes == null ? types : types.Concat(parameterTypes).ToArray();

				_hashCode = MakeHashCode(_types.Select(t => t.GetHashCode()));
			}

			public bool Equals(FactoryKey other)
			{
				return other != null && other._types.SequenceEqual(_types);
			}

			public override bool Equals(object obj)
			{
				return Equals(obj as FactoryKey);
			}

			public override int GetHashCode()
			{
				return _hashCode;
			}

			private static int MakeHashCode(IEnumerable<int> more)
			{
				lock (_hashCodeGenerator)
				{
					_hashCodeGenerator.Reset();
					return _hashCodeGenerator.Step(more);
				}
			}
		}
	}
}

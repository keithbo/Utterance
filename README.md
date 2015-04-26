# Utterance
Utterance is a simple library that primarily contains tools for interacting with, caching, and manipulating System.Linq.Expression trees.
There are five main areas:

1. ExpressionVisitor derived classes.
2. Expression Extension methods.
3. Caching.
4. Expression-based type Factory.
5. Everything else.

## 1. ExpressionVisitor derived classes
A few of the ExpressionVisitor implementations available through Utterance are also used through some of the Extension methods available but can be used in their own right, and extended further as needed.

* ArchiveExpressionVisitor
* HashCodeExpressionVisitor
* IdentityExpressionVisitor
* ReplaceExpressionVisitor
* ResettingExpressionVisitor

## 2. Expression Extension methods
Utterance includes a number of Extension methods that aid in short-handing some more complicated operations around manipulating Expression trees.

* PartialEval
  * Compiles part of an Expression tree if possible. Useful for reducing Closures to their evaluated form.
* GetEnumerator
  * Creates an IEnumerator instance for an Expression tree that will iterate the nodes in a tree.
* Replace
  * Substitutes one expression for another throughout an Expression tree.
* ReplaceAll
  * Substitutes all provided expressions for their counterparts throughout an Expression tree.
* Compose
  * Creates a new LambdaExpression, or generic variant, that merges two LambdaExpressions by replacing the input parameter of the second with the entire first expression.

## 3. Caching
Utterance provides a basic but extensible caching implementation as well as a number of derivative implementations that can be used to cache Expressions or LanbdaExpressions and their compiled form.

## 4. Expression-based type Factory
A simple type factory mechanism exists with the Factory class. It makes use of Utterance's caching classes to minimize re-compilation of LambdaExpressions which are used to create ObjectFactory delegates for requested types.
Factory includes two forms of value, first is the raw `Expression<ObjectFactory>` and `Expression<ObjectFactory<T>>` results. These can be used in further Expression manipulations. Or Factory can return the compiled form `ObjectFactory` and `ObjectFactory<T>` which when invoked with the necessary constructor values will generate a new type, very similar to how `System.Activator` functions but with cached Expressions to minimize reflection.

## 5. Everything else
Utterance has a few miscellaneous utility classes that it provides and uses which might be useful. These include:

* DelegatingByteConverter
* DelegatingEqualityComparer
* ExpressionEnumerator
* ExpressionEqualityComparer
* FNV1AHash
* IByteConverter
* IExpressionArchive

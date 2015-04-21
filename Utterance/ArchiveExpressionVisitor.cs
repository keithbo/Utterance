namespace Utterance
{
	using System;
	using System.Linq.Expressions;

	/// <summary>
	/// ExpressionVisitor implementation that extracts each Expression node in an Expression tree
	/// and passes it to an IExpressionArchive implementation based on acceptance from
	/// a provided accept function.
	/// </summary>
	public class ArchiveExpressionVisitor : ArchiveExpressionVisitor<IExpressionArchive>
	{
		public ArchiveExpressionVisitor(IExpressionArchive archive)
			: base(archive)
		{
		}

		public ArchiveExpressionVisitor(IExpressionArchive archive, Func<Expression, bool> accept)
			: base(archive, accept)
		{
		}
	}

	/// <summary>
	/// ExpressionVisitor implementation that extracts each Expression node in an Expression tree
	/// and passes it to an IExpressionArchive implementation based on acceptance from
	/// a provided accept function.
	/// </summary>
	/// <typeparam name="TArchive">Type derived from IExpressionArchive</typeparam>
	public class ArchiveExpressionVisitor<TArchive> : ExpressionVisitor
		where TArchive : IExpressionArchive
	{
		private TArchive _archive;
		private Func<Expression, bool> _accept;

		public TArchive Archive
		{
			get
			{
				return _archive;
			}
			set
			{
				_archive = value;
			}
		}

		public ArchiveExpressionVisitor(TArchive archive)
			: this(archive, null)
		{
		}

		public ArchiveExpressionVisitor(TArchive archive, Func<Expression, bool> accept)
		{
			Archive = archive;
			_accept = accept ?? DefaultAccept;
		}

		public override Expression Visit(Expression node)
		{
			if (_accept(node))
			{
				Archive.Add(node);
			}
			return base.Visit(node);
		}

		private static bool DefaultAccept(Expression expression)
		{
			return true;
		}
	}
}

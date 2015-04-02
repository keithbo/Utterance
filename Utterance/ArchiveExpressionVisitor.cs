namespace Utterance
{
	using System;
	using System.Linq.Expressions;

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

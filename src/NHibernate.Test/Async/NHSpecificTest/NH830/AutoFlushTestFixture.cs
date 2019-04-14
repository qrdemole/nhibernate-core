﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Criterion;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH830
{
	using System.Threading.Tasks;
	[TestFixture]
	public class AutoFlushTestFixtureAsync : BugTestCase
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return TestDialect.SupportsEmptyInsertsOrHasNonIdentityNativeGenerator;
		}

		[Test]
		public async Task AutoFlushTestAsync()
		{
			ISession sess = OpenSession();
			ITransaction t = sess.BeginTransaction();
			//Setup the test data
			Cat mum = new Cat();
			Cat son = new Cat();
			await (sess.SaveAsync(mum));
			await (sess.SaveAsync(son));
			await (sess.FlushAsync());

			//reload the data and then setup the many-to-many association
			mum = (Cat) await (sess.GetAsync(typeof (Cat), mum.Id));
			son = (Cat) await (sess.GetAsync(typeof (Cat), son.Id));
			mum.Children.Add(son);
			son.Parents.Add(mum);

			//Use criteria API to search first 
			var result = await (sess.CreateCriteria(typeof (Cat))
				.CreateAlias("Children", "child")
				.Add(Expression.Eq("child.Id", son.Id))
				.ListAsync<Cat>());
			//the criteria failed to find the mum cat with the child
			Assert.AreEqual(1, result.Count);

			await (sess.DeleteAsync(mum));
			await (sess.DeleteAsync(son));
			await (t.CommitAsync());
			sess.Close();
		}
	}
}

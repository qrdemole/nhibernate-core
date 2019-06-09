﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.GH1918
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		private const string _ingData = "data_ing";
		private readonly EmbId _ingId = new EmbId { X = 5, Y = 6 };

		protected override void OnSetUp()
		{
			var edId = new EmbId { X = 1, Y = 2 };
			var ed = new BiEmbIdRefEdEntity { Id = edId, Data = "data_ed" };
			var ing = new BiEmbIdRefIngEntity { Id = _ingId, Data = _ingData, Reference = ed };
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				s.Save(ed);
				s.Save(ing);
				tx.Commit();
			}
		}

		[Test]
		public async Task CanReadBidirectionalEntitiesWithEmbeddedIdAsync()
		{
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var ingEntity = await (s.GetAsync<BiEmbIdRefIngEntity>(_ingId));
				Assert.That(ingEntity.Data, Is.EqualTo(_ingData));
				await (tx.CommitAsync());
			}
		}

		protected override void OnTearDown()
		{
			using (var session = OpenSession())
			using (var tx = session.BeginTransaction())
			{
				session.CreateQuery("delete from BiEmbIdRefIngEntity").ExecuteUpdate();
				session.CreateQuery("delete from BiEmbIdRefEdEntity").ExecuteUpdate();
				tx.Commit();
			}
		}
	}
}
using System.Collections;
using BiographicalDetails.Domain;

namespace BiographicalDetailsTests.Helpers;

public class ImmigrationStatusRequiringSINTestData : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator()
	{
		yield return new object[] { ImmigrationStatus.CanadianCitizen };
		yield return new object[] { ImmigrationStatus.PermanentResident };
		yield return new object[] { ImmigrationStatus.TemporaryForeignWorker };
		yield return new object[] { ImmigrationStatus.ProtectedPerson };
		yield return new object[] { ImmigrationStatus.Indigenous };
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class ImmigrationStatusRequiringUCITestData : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator()
	{
		yield return new object[] { ImmigrationStatus.PermanentResident };
		yield return new object[] { ImmigrationStatus.TemporaryForeignWorker };
		yield return new object[] { ImmigrationStatus.InternationalStudent };
		yield return new object[] { ImmigrationStatus.ProtectedPerson };
		yield return new object[] { ImmigrationStatus.Visitor };
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
using System.ComponentModel.DataAnnotations;

namespace BiographicalDetails.Website.Models.Enums;


public enum LevelOfStudy
{
	[Display(Name = "High school diploma")]
	HighSchoolDiploma = 0,

	[Display(Name = "Some college")]
	SomeCollege = 1,

	[Display(Name = "Bachelor's degree")]
	BachelorDegree = 2,

	[Display(Name = "Master's degree")]
	MasterDegree = 3,

	[Display(Name = "Doctorate (PhD)")]
	Doctorate = 4
}


public enum ImmigrationStatus
{
	[Display(Name = "Canadian Citizen")]
	CanadianCitizen = 0,

	[Display(Name = "Permanent Resident")]
	PermanentResident = 1,

	[Display(Name = "Temporary Foreign Worker")]
	TemporaryForeignWorker = 2,

	[Display(Name = "International Student")]
	InternationalStudent = 3,

	[Display(Name = "Protected Person")]
	ProtectedPerson = 4,

	[Display(Name = "Indigenous")]
	Indigenous = 5,

	[Display(Name = "Visitor")]
	Visitor = 6
}

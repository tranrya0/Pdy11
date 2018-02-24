2:39 AM 2/24/2017	
--Tutorial notes 	
--chuck costarella

The focus of tutorial #11 is OO inheritance in the language and how the relational database models those relationships with the TPH (table per hierarchy) structure.

Specifically, the download website you are starting with has been modified from the original online tutorial with the following changes:

The Student model has an EmailAddress property (a C# string object). EmailAddress is propagated to each of the Student views and works correctly in the website prior to proceeding with the tutorial steps. I had added this property to the Student model some time ago when testing the code first approach. I changed the Student model to observe the EF picking up my change and modifying my schema correctly to adjust. After that point, I never saw a reason to remove email from the Student model, so I left it in there.

So for this tutorial, because of the presence of email address in the Student model, there are additional changes that must be made that are not part of the tutorial in order to preserve the Student EmailAddress property, as follows:

1 - When you are instructed to overwrite the code in the Student.cs file, after you do that, you must add the email back into the model class with the following code:

public string EmailAddress { get; set; }

I didn't have any data annotations over this prop.

2 - Where you give the EntityFramework PM Console command Add-Migration Inheritance, this creates a migration file with the timestamp prepended to its name. You are then given instructions to open this file and edit the entityframework code that merges the inheritance hierarchy of base class Person, derived classes Student and Instructor back into the Person table (remember TPH is 1 table per hierarchy, in this case Person).

You must add a line of code to the Up() method to tell the EntityFramework to create a column for EmailAddress, and then subsequently copy all the Student email data into the column in the Person table and allow Null addresses for those Person(s) who don't have them (Instructors do not have them).

Here are the required lines of code:

// put the following command in the same group as the AddColumn commands:

AddColumn("dbo.Person", "EmailAddress", c => c.String(nullable: true, maxLength: 128));

The next fragment copies all the student data in to the Person table (preparing to drop the Student table). The EMailAddress is added as a named parameter, but make sure it is in the same position on both sides of the expression's "SELECT" keyword.

// Copy existing Student data into new Person table.
Sql("INSERT INTO dbo.Person (LastName, FirstName, EmailAddress, HireDate, EnrollmentDate, Discriminator, OldId) SELECT LastName, FirstName, EmailAddress, null AS HireDate, EnrollmentDate, 'Student' AS Discriminator, ID AS OldId FROM dbo.Student");
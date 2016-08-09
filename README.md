FluentMigrator.Assertions
======================

Summary
-------

**FluentMigrator.Assertions** is a simple extension of the excellent migration framework **FluentMigrator** which you can [learn about here](https://github.com/schambers/fluentmigrator). This extension is designed to make your migrations safer.

Why You Need This Extension
---------------------------

When running a migration against a database, it is nice to assume that all changes to the database schema were applied by your previous migrations - your migrations are solely responsible for schema changes. But is this a reasonable guarantee?

In larger applications it is often the case that many different parts of a business, or even different companies, try to share the same database. Therefore, stating you know what the database looks like before you run your migrations is reckless, and if you're wrong, your migration could have disastrous consequences! You could manually check the database beforehand, but this would require much care and you could easily miss something.

Instead, you should **use assertions** within the transactions surrounding your migrations! You need not just hope that there are no rows with values x in column y - you can be certain of it. If your assertions fail, your migration simply won't run and you won't have corrupted your database. Your forward thinking when you wrote the migraton just saved the day!

Real Life Example
-----------------

This is actually the inspiration for this package, and demonstrates when assertions can be very useful.

### The Problem

We had a fairly large database with huge stored procedures. We updated these procedures by copy pasting the existing definition into our source control and then modifying it. Our migrations then looked roughly like this:
```c#
public class UpdateStoredProcedureExample : Migration
{
    private const string ProcedureName = "MyProcedure";
    private const string NewProcedureDefinition = "NewProcedure.sql";
    private const string OldProcedureDefinition = "OldProcedure.sql";

    public override void Up()
    {
        Execute.Sql($"DROP PROCEDURE {ProcedureName}");
        Execute.EmbeddedScript(NewProcedureDefinition);
    }

    public override void Down()
    {
        Execute.Sql($"DROP PROCEDURE {ProcedureName}");
        Execute.EmbeddedScript(OldProcedureDefinition);
    }
}
```
So we recorded the current definition for the stored procedure as well as our planned new definition. We simply removed the existing definition and replaced it with our own.

As time passed and more people worked on the project in parallel, there was an incident where two people worked on the same stored procedure, but they didn't realise it. Lets say Alice took the original procedure definition A, modified it and saved definition B. Bob also took the original definition A, modified it and saved the definition C. We then ran Alice's migration followed by Bob's migration. What went wrong?

When Bob modified A, he didn't include Alice's changes. Therefore, when Bob dropped the procedure and replaced it with his own in the migration, he erased all of Alice's work! Needless to say this was a big mistake and some rapid repairs had to be made... not a situation you should ever have to face.

### The Solution

Bob and Alice could have been saved if they had just used assertions. They should have checked that the procedure definition they were modifying really would be the one in the database when the migration ran. Expecting Bob and Alice to do this check manually is risky, as the task is complicated if there are many migrations to run in a release (and Alice or Bob could just forget!).

Instead, we should write our assumptions into the migrations. With this extension, we can do this as follows:
```c#
public class UpdateStoredProcedureExample : MigrationWithAssertions
{
    private const string ProcedureName = "MyProcedure";
    private const string NewProcedureDefinition = "NewProcedure.sql";
    private const string OldProcedureDefinition = "OldProcedure.sql";

    public override void Up()
    {
        Assert.StoredProcedureExists(ProcedureName).WithDefinition(OldProcedureDefinition);
        Execute.Sql($"DROP PROCEDURE {ProcedureName}");
        Execute.EmbeddedScript(NewProcedureDefinition);
    }

    public override void Down()
    {
        Assert.StoredProcedureExists(ProcedureName).WithDefinition(NewProcedureDefinition);
        Execute.Sql($"DROP PROCEDURE {ProcedureName}");
        Execute.EmbeddedScript(OldProcedureDefinition);
    }
}
```
That's all you have to do! Now Bob's migration (running second) would notice that the stored procedure was not what it believed it should be, and thus fail to run. This avoids erasing Alice's changes, leaving the database in a good state. Bob can then amend his migration appropriately, and the changes can be applied later safely.

Disclaimer
----------

This library has only been tested on SQL server 2008 and was developed as a hobby for a very specific purpose. Therefore, it is not guaranteed to work on basically anything.

However, the library does focus solely on assertions, so it will probably either:
* Do absolutely nothing.
* Prevent migrations which should be allowed to run from running.
* Work perfectly! :)

Note that it is unlikely to corrupt data and so is relatively safe to use. Nevertheless if you do plan to use this package, please test your migrations as early as you can.

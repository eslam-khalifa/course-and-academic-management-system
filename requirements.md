# Requirements (Expanded Sentence Style for ERD)

## Course (entity)

* A **Course** has an identifier `CourseId` (primary key, integer, identity).
* A Course has a **Code** (`Code`, `nvarchar(20)`), which is optional but recommended and must be unique when provided (example: `"CS101"`).
* A Course has a **Name** (`Name`, `nvarchar(50)`), which is required, 3–50 characters, unique, and must not contain numbers (enforced by `NoNumberAttribute`); example: `"Introduction to Algorithms"`.
* A Course has a **ShortDescription** (`ShortDescription`, `nvarchar(250)`) for list views; example: `"Basic sorting and searching algorithms"`.
* A Course has a **Full Description** (`Description`, `nvarchar(max)`) which may contain a long syllabus or HTML.
* A Course has a **Category**; this may be stored as a string `Category` (`nvarchar(50)`, required) or as a foreign key `CategoryId` (`int`) to a `CourseCategory` lookup table (see CourseCategory entity). Example category: `"Computer Science"`.
* A Course has numeric **Credits** (`Credits`, `int`, optional, typical range 0–10) and **DurationHours** (`DurationHours`, `int`, optional, total expected hours).
* A Course may have a **MaxTrainees** limit (`MaxTrainees`, `int`, optional) to cap enrollments.
* A Course has a boolean **IsActive** flag (`IsActive`, `bit`) to indicate whether the course is currently offered.
* A Course may have resource links such as `ThumbnailUrl` and `SyllabusUrl` (`nvarchar(250)`, optional).
* A Course has an **InstructorId** (`InstructorId`, `int`, nullable) referencing `User.UserId`; a Course is assigned to exactly one Instructor when `InstructorId` is set, and an Instructor can have many courses.
* A Course has audit fields: `CreatedAt` (datetime), `CreatedBy` (int, FK to User optionally), `UpdatedAt` (datetime), `UpdatedBy` (int), and a soft-delete flag `IsDeleted` (`bit`) for safe deletes.
* A Course should include a concurrency token `RowVersion` (rowversion/timestamp) for optimistic concurrency.
* Business constraints: Course `Name` is unique; `Code` if present must be unique; course cannot be marked `IsActive = false` while there are scheduled Sessions unless those Sessions are canceled or reassigned.
* Example Course record (sentence): *CourseId = 1, Code = "CS101", Name = "Intro to Algorithms", Category = "Computer Science", Credits = 3, InstructorId = 5, IsActive = true.*

---

## Session (entity)

* A **Session** has an identifier `SessionId` (primary key, integer, identity).
* A Session belongs to one Course via `CourseId` (foreign key, required). A Course can have many Sessions.
* A Session has a **Title** (`Title`, `nvarchar(100)`, optional) to distinguish offerings (example: `"Fall 2025 Cohort"`).
* A Session may have a short `Code` (`SessionCode`, `nvarchar(30)`, optional) to identify the cohort.
* A Session has `StartDate` (datetime, required) which must not be in the past at creation time.
* A Session has `EndDate` (datetime, required) which must be strictly after `StartDate`.
* A Session can have `EnrollmentStartDate` and `EnrollmentEndDate` (datetime, optional) that control when trainees can enroll.
* A Session has `Location` (`nvarchar(200)`, optional) which may contain a room name or an online link; an additional `Mode` field (`Mode`, enum/string) indicates `Online`, `Offline`, or `Hybrid`.
* A Session has `Capacity` (`int`, optional) to limit seats; `EnrolledCount` can be computed or stored for convenience.
* A Session can optionally reference an `InstructorId` (`int`, FK) to override or specify the instructor for that particular session; if null the Course’s Instructor is used.
* A Session has a `Status` (`nvarchar(20)` or enum) such as `Scheduled`, `OpenForEnrollment`, `Ongoing`, `Completed`, `Cancelled`.
* A Session has audit fields: `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy`, `IsDeleted`, and `RowVersion` for concurrency.
* Business rules: cannot create a Session with `StartDate` < today; cannot set `Capacity` less than current `EnrolledCount`; if session `Status` is `Cancelled`, grades cannot be recorded for it.
* Example Session record (sentence): *SessionId = 10, CourseId = 1, Title = "Fall 2025", StartDate = 2025-10-01, EndDate = 2025-12-15, Mode = "Offline", Capacity = 30, InstructorId = 5.*

---

## User (entity)

* A **User** has an identifier `UserId` (primary key, integer, identity).
* A User has `FirstName` (`nvarchar(50)`) and `LastName` (`nvarchar(50)`), and a computed or persisted `FullName` (`nvarchar(101)`); `Name` (single field) may be used instead if you prefer. Example: `FirstName = "Ali"`, `LastName = "Hassan"`, `FullName = "Ali Hassan"`.
* A User has an `Email` (`nvarchar(100)`), required, valid format, and unique across all users; example: `"ali@example.com"`.
* A User stores authentication info: `PasswordHash` and optional `PasswordSalt` (`nvarchar(max)` or varbinary), though authentication may be delegated to Identity; `IsEmailConfirmed` (`bit`) marks confirmed emails.
* A User has a `Role` (`Role`, enum or FK to Role table) with allowed values `Admin`, `Instructor`, `Trainee`. Roles may be stored as an enum column or as a separate `Role` lookup table if roles are dynamic.
* A User may have `PhoneNumber` (`nvarchar(20)`, optional), `DateOfBirth` (date, optional), `Gender` (`nvarchar(10)`, optional), and `ProfilePictureUrl` (`nvarchar(250)`, optional).
* A User may have address fields: `AddressLine1`, `AddressLine2`, `City`, `State`, `PostalCode`, `Country` (all optional).
* A User has status flags: `IsActive` (`bit`), `IsLocked` (`bit`), and `IsDeleted` (`bit` for soft delete).
* A User has activity fields: `CreatedAt`, `CreatedBy` (optional), `UpdatedAt`, `LastLoginAt`.
* A User has `RowVersion` for concurrency.
* Business rules & validation: `Name` must be 3–50 characters; `Email` must be unique (check via remote validation on create/edit); `Role` is required. Deleting a User who is an Instructor assigned to active Courses or Sessions should be prevented (or Instructor must be reassigned) unless a soft-delete policy is used.
* Example User (sentence): *UserId = 5, FirstName = "Sara", LastName = "Ezzat", Email = "[sara@uni.edu](mailto:sara@uni.edu)", Role = "Instructor", IsActive = true.*

---

## Grade (entity)

* A **Grade** has an identifier `GradeId` (primary key, integer, identity).
* A Grade belongs to one `Session` via `SessionId` (FK, required).
* A Grade belongs to one `Trainee` via `TraineeId` (FK to `User.UserId`, required); the referenced user must have `Role = Trainee` by business rule.
* A Grade has a numeric `Value` (`int`, required) constrained to `0..100`.
* A Grade may have a `Weight` (`decimal(5,2)`, optional) used when calculating weighted totals.
* A Grade has `AttemptNumber` (`int`, default 1) to support multiple attempts; a unique constraint should be enforced such as `(SessionId, TraineeId, AttemptNumber)` to avoid duplicate entries for the same attempt.
* A Grade may have `IsFinal` (`bit`) to mark whether it is the final grade for that session.
* A Grade may store `GradedBy` (`int`, FK to User) and `GradedAt` (datetime) to record who entered the grade and when.
* A Grade may have `Comments` (`nvarchar(500)`) for teacher feedback.
* A Grade includes audit fields: `CreatedAt`, `UpdatedAt`, and `RowVersion`.
* Business rules: final grade validation (only one `IsFinal = true` per `(SessionId, TraineeId)`), numeric range check, must not be recorded for canceled sessions.
* Example Grade (sentence): *GradeId = 100, SessionId = 10, TraineeId = 22, Value = 85, AttemptNumber = 1, GradedBy = 5, GradedAt = 2025-12-16.*

---

## Optional / Supporting Entities (recommended for a richer, normalized schema)

* **CourseCategory**: A `CourseCategory` has `CategoryId` (PK), `Name` (unique, nvarchar(50)), and `Description`; a Course may reference `CategoryId` instead of a plain Category string to normalize categories.
* **Enrollment** (optional): An `Enrollment` models trainee registration and has `EnrollmentId` (PK), `SessionId` (FK), `TraineeId` (FK), `EnrollmentDate`, `Status` (`Enrolled`, `Withdrawn`, `Waitlisted`, `Completed`), `AttendancePercentage`, and `IsActive`. If you add Enrollment, do not rely solely on Grade existence to indicate that a trainee participated. Example: *EnrollmentId = 200, SessionId = 10, TraineeId = 22, Status = "Enrolled".*
* **CoursePrerequisite** (optional): A join table that stores prerequisites with `Id`, `CourseId` and `PrerequisiteCourseId` (both FKs to Course). A Course may have many prerequisites (many-to-many self relationship).
* **Role** (optional): If roles are dynamic, include `RoleId` (PK) and `Name` (Admin/Instructor/Trainee). Otherwise store role as an enum in the User table.
* **CourseMaterial** (optional): `MaterialId`, `CourseId`, `Title`, `Url`, `Type` (PDF, Video), useful for storing resource links.

---

## Relationships (sentences suitable for an ERD)

* A Course is identified by `CourseId` and may reference a `CategoryId` (optional) to the `CourseCategory` table.
* A Course **is assigned to one Instructor** by `InstructorId`, and an Instructor **may be assigned to many Courses** (one-to-many: `User (Instructor)` → `Course`).
* A Course **has many Sessions**, each Session belongs to exactly one Course (one-to-many: `Course` → `Session`).
* A Session **may be led by one Instructor** (optional `InstructorId` on Session); if not set, use the Course’s Instructor.
* A Session **has many Grades**, each Grade belongs to one Session (one-to-many: `Session` → `Grade`).
* A Trainee (User with Role = Trainee) **has many Grades**, each Grade belongs to one Trainee (one-to-many: `User(Trainee)` → `Grade`).
* If Enrollment is used: a Session **has many Enrollments**, a Trainee **has many Enrollments** and Enrollment links trainees and sessions (many-to-many via Enrollment).
* Course prerequisites are modeled by `CoursePrerequisite` as a many-to-many self-relationship on Course.

---

## Keys, Indexes, and Unique Constraints (sentences)

* Primary keys: `CourseId`, `SessionId`, `UserId`, `GradeId`, etc.
* Unique constraints: `Course.Name` unique, `Course.Code` unique (if used), `User.Email` unique, `(SessionId, TraineeId, AttemptNumber)` unique for grades.
* Indexes for search: add nonclustered indexes on `Course(Name)`, `Course(Category)` or `Course(CategoryId)`, `User(Email)`, `User(FullName)`, and `Session(StartDate)` to speed up searching and sorting.
* Consider full-text index on `Course.Description` if large text search is required.

---

## Referential Actions & Deletion Rules (sentences)

* Prefer **soft-delete** (`IsDeleted`) for Courses, Sessions, Users, and Grades to avoid accidental data loss and to keep historical grades.
* If hard delete is required, deletion of a Course should be restricted if Sessions exist, or cascade-delete Sessions and Grades only after explicit confirmation.
* Deleting a User who is assigned as an Instructor should be blocked until the Instructor is unassigned or reassigned (referential integrity rule).
* Deleting a Session should either prevent deletion when Grades exist, or cascade/delete Grades only when business rules permit.

---

## Validation & Business Rules (summary sentences)

* Use **Data Annotations** for required fields, ranges, string lengths, and email format.
* Implement the custom `NoNumberAttribute` for Course `Name` to block numeric characters.
* Use **Remote Validation** for `Course.Name` and `User.Email` to check uniqueness without page reload (AJAX).
* Enable **client-side unobtrusive validation** so users receive immediate feedback in the browser.
* Enforce date rules: `Session.StartDate` ≥ today on creation; `Session.EndDate` > `StartDate`.
* Enforce grade rules: `Value` must be 0–100; only one final grade per trainee per session (`IsFinal` uniqueness).
* Business checks: prevent creating Session that overlaps with instructor schedule (optional advanced rule), prevent enrolling beyond `Capacity`.

---

## Audit, Concurrency & Security (sentences)

* Include audit fields `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy` on major entities to track changes.
* Use `RowVersion` concurrency tokens to prevent lost updates in multi-user scenarios.
* Store `PasswordHash` securely (or use ASP.NET Identity). Do not store plain passwords.
* Use roles and authorization rules so that only `Admin` can create/delete users, only `Instructor` can grade, etc.

---

## Simple Example Records (one-line sentences for clarity)

* Course example: *CourseId=1, Code="CS101", Name="Intro to Algorithms", Category="Computer Science", Credits=3, InstructorId=5.*
* Session example: *SessionId=10, CourseId=1, Title="Fall 2025", StartDate=2025-10-01, EndDate=2025-12-15, Mode="Offline", Capacity=30.*
* User example: *UserId=5, FirstName="Sara", LastName="Ezzat", Email="[sara@uni.edu](mailto:sara@uni.edu)", Role="Instructor", IsActive=true.*
* Grade example: *GradeId=100, SessionId=10, TraineeId=22, Value=85, AttemptNumber=1, GradedBy=5, GradedAt=2025-12-16.*
* Enrollment example (if used): *EnrollmentId=200, SessionId=10, TraineeId=22, EnrollmentDate=2025-09-20, Status="Enrolled".*

---

# Technologies

* version control: Git, GitHub
* project management tool: Jira
* containerization tool: Docker
* Web: .NET 9, MVC, Razor Pages, 3-Tier Arch, Repository Pattern, Generic Repository, Unit of Work
* Deployment: Monster .NET

CREATE DATABASE EmployeeManagement
GO 

USE EmployeeManagement 
GO 

/* POSITIONS */
CREATE TABLE Positions (
	Id varchar(10) PRIMARY KEY,
	PositionName nvarchar(100) NOT NULL
)

/* ROLES */
CREATE TABLE Roles (
	Id int PRIMARY KEY, 
	RoleName nvarchar(100) NOT NULL
)

/* USERS */
CREATE TABLE Users (
	Id varchar(20) PRIMARY KEY, 
	FullName nvarchar(200) NOT NULL,
	DOB datetime, 
	Gender nvarchar(50) NOT NULL,
	Address nvarchar(200), 
	Email varchar(100) NOT NULL,
	JoinedDate datetime NOT NULL,
	Phone varchar(10) NOT NULL, 
	IdPos varchar(10) NOT NULL, 
	IdRole int NOT NULL
)
ALTER TABLE Users ADD CONSTRAINT FK_IdPos FOREIGN KEY(IdPos) REFERENCES Positions(Id) 
ALTER TABLE Users ADD CONSTRAINT FK_IdRole FOREIGN KEY(IdRole) REFERENCES Roles(Id) 
ALTER TABLE Users ADD Password text NOT NULL
ALTER TABLE Users ADD CONSTRAINT Unique_Email UNIQUE (Email);
ALTER TABLE Users ALTER COLUMN Password nvarchar(MAX) NOT NULL

/* Salary */
CREATE TABLE Salary ( 
	Id int PRIMARY KEY IDENTITY(1, 1), 
	IdEmployee varchar(20),
	Base bigint NOT NULL,
	AllowedOff int NOT NULL, 
	ActualOff int NOT NULL, 
	Bonus bigint, 
	Deduction bigint,
	Final bigint NOT NULL,
	Date datetime NOT NULL
)
ALTER TABLE Salary ADD CONSTRAINT FK_IdEmployee_Salary FOREIGN KEY(IdEmployee) REFERENCES Users(Id) 

/* Forms */ 
CREATE TABLE Forms (
	Id int PRIMARY KEY IDENTITY(1,1), 
	FormName nvarchar(200) NOT NULL
)

/* Files */ 
CREATE TABLE Files (
	Id int PRIMARY KEY IDENTITY(1,1), 
	IdForm int NOT NULL,
	IdEmployee varchar(20) NOT NULL,
	DateCreated datetime NOT NULL, 
	FormName nvarchar(200) NOT NULL,
	FormContent varbinary(MAX) NOT NULL
)
ALTER TABLE Files ADD CONSTRAINT FK_IdForm FOREIGN KEY(IdForm) REFERENCES Forms(Id) 
ALTER TABLE Files ADD CONSTRAINT FK_IdEmployee FOREIGN KEY(IdEmployee) REFERENCES Users(Id) 


/***********************************/
/* INSERT */ 
/* Positions */
CREATE PROCEDURE INSERT_POS
(@Id varchar(10), @PositionName nvarchar(100))
AS
BEGIN
	INSERT INTO Positions VALUES (@Id, @PositionName)
END
GO

/* Users */
/* Count ID */
CREATE FUNCTION LAST_UID()
RETURNS INT
AS
BEGIN
	DECLARE @COUNT INT
	SELECT @COUNT =  (SELECT COUNT(Id) FROM Users)
	RETURN @COUNT
END

/* Get ID */
CREATE FUNCTION GET_UID()
RETURNS VARCHAR(20)
AS
BEGIN
	DECLARE @Id VARCHAR(20)
	RETURN  (SELECT TOP 1 Id FROM Users ORDER BY Id DESC)
END

/* Autoincrement ID */
CREATE FUNCTION AUTO_UID()
RETURNS VARCHAR(20)
AS
BEGIN
	DECLARE @Id VARCHAR(20)
	DECLARE @NId VARCHAR(20)
	SET @Id = dbo.GET_UID()
	DECLARE @COUNT INT 
	SET @COUNT = dbo.LAST_UID()
	IF (@COUNT) = 0
		SET @NId = 'U00001'
	ELSE 
		SET @NId = CASE 
			WHEN @COUNT > 0 AND @COUNT < 9 THEN 'U0000' + CONVERT(CHAR, (CAST(SUBSTRING(@Id, 2, 5) AS INT) + 1 ))
			WHEN @COUNT >= 9 AND @COUNT < 99 THEN 'U000' + CONVERT(CHAR, (CAST(SUBSTRING(@Id, 2, 5) AS INT) + 1 ))
			WHEN @COUNT >= 99 AND @COUNT < 999 THEN 'U00' + CONVERT(CHAR, (CAST(SUBSTRING(@Id, 2, 5) AS INT) + 1 ))
			WHEN @COUNT >= 999 AND @COUNT < 9999 THEN 'U0' + CONVERT(CHAR, (CAST(SUBSTRING(@Id, 2, 5) AS INT) + 1 ))
			WHEN @COUNT >= 9999 AND @COUNT < 99999 THEN 'U' + CONVERT(CHAR, (CAST(SUBSTRING(@Id, 2, 5) AS INT) + 1 ))
		END
	RETURN @NId		
END

/* Insert */
CREATE PROCEDURE INSERT_USER (
	@FullName nvarchar(200),
	@DOB datetime, 
	@Gender nvarchar(50),
	@Address nvarchar(200), 
	@Email varchar(100),
	@JoinedDate datetime,
	@Phone varchar(10), 
	@IdPos varchar(10), 
	@IdRole int,
	@Password nvarchar(MAX))
AS
BEGIN
	DECLARE @Id VARCHAR(20)
	SET @Id = dbo.AUTO_UID()
	INSERT INTO Users VALUES (@Id, @FullName, @DOB, @Gender, @Address, @Email, @JoinedDate, @Phone, @IdPos, @IdRole, @Password)
END
GO

/*
CREATE TRIGGER Trigger_InsertUser
ON Users
AFTER INSERT
AS
BEGIN
  DECLARE @Id VARCHAR(20);
  SET @Id = dbo.AUTO_UID();
  DECLARE @FullName nvarchar(200),
          @DOB datetime,
          @Gender nvarchar(50),
          @Address nvarchar(200),
          @Email varchar(100),
          @JoinedDate datetime,
          @Phone varchar(10),
          @IdPos varchar(10),
          @IdRole int,
          @Password nvarchar(MAX);

  SELECT @FullName = Inserted.FullName,
         @DOB = Inserted.DOB,
         @Gender = Inserted.Gender,
         @Address = Inserted.Address,
         @Email = Inserted.Email,
         @JoinedDate = Inserted.JoinedDate,
         @Phone = Inserted.Phone,
         @IdPos = Inserted.IdPos,
         @IdRole = Inserted.IdRole,
         @Password = Inserted.Password
  FROM INSERTED;

  INSERT INTO Users
  VALUES (@Id, @FullName, @DOB, @Gender, @Address, @Email, @JoinedDate, @Phone, @IdPos, @IdRole, @Password);
END;
GO
*/


/* Salary */
CREATE TRIGGER FinalSalary
ON Salary
AFTER INSERT, UPDATE
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ActualOff int;
	DECLARE @AllowedOff int;
	SELECT @ActualOff = i.ActualOff, @AllowedOff = i.AllowedOff FROM Salary s JOIN INSERTED i ON i.Id = s.Id;

	/*Actual off is <= allowed off*/ 
	IF (@ActualOff <= @AllowedOff) /* Ex: 1 <= 2 --> allowed --> no deduction*/
	BEGIN
		UPDATE Salary
		SET Final = i.Base + i.Bonus - i.Deduction
		FROM Salary s JOIN INSERTED i
		ON i.Id = s.Id;
	END

	/*Actual off is > allowed off*/ 
	ELSE /* Ex: 3 > 2 --> not allowed --> deduction*/
	BEGIN
		UPDATE Salary
		SET Final = i.Base + i.Bonus - i.Deduction - (i.Base / 30 * (@ActualOff - @AllowedOff))
		FROM Salary s JOIN INSERTED i
		ON i.Id = s.Id;
	END
END;

/* UPDATE */ 
/* Positions */
CREATE PROCEDURE UPDATE_POS
(@Id varchar(10), @PositionName nvarchar(100))
AS
BEGIN
	UPDATE Positions SET PositionName = @PositionName WHERE Id = @Id 
END
GO

/* Users */ 
CREATE PROCEDURE UPDATE_USER (
	@Id varchar(20),
	@FullName nvarchar(200),
	@DOB datetime, 
	@Gender nvarchar(50),
	@Address nvarchar(200), 
	@Email varchar(100),
	@JoinedDate datetime,
	@Phone varchar(10), 
	@IdPos varchar(10), 
	@IdRole int,
	@Password text)
AS
BEGIN
	UPDATE Users SET 
	FullName = @FullName,
	DOB = @DOB, 
	Gender = @Gender,
	Address = @Address, 
	Email = @Email,
	JoinedDate = @JoinedDate,
	Phone = @Phone, 
	IdPos = @IdPos, 
	IdRole = @IdRole,
	Password = @Password
	WHERE Id = @Id
END
GO

/* DELETE */ 



/***********************************/
/* INSERT DATA */ 
/* Positions */
INSERT INTO Positions VALUES
('FE', N'Front-end Developer'), 
('BE', N'Back-end Developer'), 
('PM', N'Product Manager'), 
('DE', N'Designer'),
('TE', N'Tester'),
('BA', N'Business Analyst')

/* Roles */
INSERT INTO Roles VALUES
(0, N'Admin'),
(1, N'Employee')

/* Users */
EXEC dbo.INSERT_USER N'Pham Huynh Anh Thu', '2002-07-03', N'Female', N'Ho Chi Minh City', 'phamthushame2002@gmail.com', '2024-05-13', '0946020824','BE', 0, '123456789' 
EXEC dbo.INSERT_USER N'Pham Huynh Yen Vi', '2005-10-06', N'Female', N'Ho Chi Minh City', 'thuolala2323@gmail.com', '2024-05-14', '0847608473','FE', 1, '123456789' 
EXEC dbo.INSERT_USER N'Pham Phuoc Binh', '1976-11-27', N'Male', N'Cao Lanh City', 'nadococaart@gmail.com', '2024-05-15', '0912873642','PM', 1, '123456789' 
EXEC dbo.INSERT_USER N'Le Thanh Thuy', '1979-01-01', N'Female', N'Cao Lanh City', 'blueclinicdemo@gmail.com', '2024-05-16', '0847608473','BA', 1, '123456789' 
EXEC dbo.INSERT_USER N'User A', '2022-07-03', N'Female', N'Cao Lanh City', 'coca@gmail.com', '2024-05-13', '11111111','FE', 1, '123456789' 

/* Salary */
INSERT INTO Salary VALUES
('U00001', 10000000, 2, 0, 1000000, 0, 0, '2024-05-13'),
('U00002', 10000000, 1, 2, 500000, 0, 0, '2024-05-14'),
('U00003', 20000000, 1, 1, 0, 100000, 0, '2024-05-15'),
('U00004', 25000000, 1, 0, 0, 0, 0, '2024-05-16')

/*
DELETE  FROM Salary
SELECT * FROM Users 
DROP TABLE FILES, FORMS, SALARY, USERS, ROLES, POSITIONS
ALTER TABLE Users DROP CONSTRAINT FK_IdPos 
*/

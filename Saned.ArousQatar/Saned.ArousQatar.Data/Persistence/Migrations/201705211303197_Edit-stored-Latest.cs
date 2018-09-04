using System.Data.Entity.Migrations;
using UtiltyManagemnt;

namespace Saned.ArousQatar.Data.Persistence.Migrations
{
   

	public partial class EditstoredLatest : DbMigration
	{
		public override void Up()
		{

		   
/****** Object:  StoredProcedure [dbo].[AdvertisementArchieveCount]    Script Date: 5/21/2017 2:57:55 PM ******/
Sql(@"
ALTER  PROCEDURE [dbo].[AdvertisementArchieveCount]
AS
BEGIN
	
	select count(*) from Advertisments
	where Advertisments.IsArchieved = 1
	
END");
			/****** Object:  StoredProcedure [dbo].[AdvertisementArchieveGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AdvertisementArchieveGetAll]
	@index [int],
	@rowNumber [int],
	@filter [nvarchar](100) = ''
AS
BEGIN
	
						SELECT Advertisments.* ,
						  Categories.Name 'CategoryName',
						  AdvertismentPrices.Period 'AdvertisementPeriod',
						  AdvertismentPrices.Price 'AdvertisementPrice' ,
						  AspNetUsers.Name 'UserName',
						  AspNetUsers.UserName
	
						  FROM Advertisments
	
						   left join Categories on 
							Advertisments.CategoryId = Categories.Id 
						   left join AdvertismentPrices on
							Advertisments.AdvertismentPriceId = AdvertismentPrices.Id
						   left join AspNetUsers on
							AspNetUsers.Id = Advertisments.ApplicationUserId
	
						where
						Advertisments.IsArchieved = 1 and 
						(Advertisments.Name like '%'+@filter +'%' or
						 Advertisments.Description like '%'+@filter +'%' or 
						 Categories.Name like '%'+@filter +'%' or 
						 AdvertismentPrices.Period like '%'+@filter +'%' or
						 AdvertismentPrices.Price like '%'+@filter +'%' or  
						 AspNetUsers.Name like '%'+@filter +'%' or 
						 AspNetUsers.UserName like '%'+@filter +'%')
	
						ORDER BY Advertisments.Id OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY;
					 
END");
			/****** Object:  StoredProcedure [dbo].[AdvertisementGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AdvertisementGetAll]
	@index [int],
	@rowNumber [int],
	@filter [nvarchar](100) = '',
	@IsArchieved [bit] = NULL,
	@CategoryId [int] = NULL,
	@IsPaided [bit] = NULL,
	@UserId [nvarchar](128) = NULL,
	@IsExpired [bit] = NULL
AS
BEGIN




	SELECT Categories.Name 'CategoryName',
	( SELECT top 1 ai.ImageUrl  from dbo.AdvertismentImages ai WHERE ai.AdvertismentId=Advertisments.Id AND ai.IsMainImage=1)  'ImageUrl',
   (SELECT  AdvertismentPrices.Period FROM AdvertismentPrices WHERE AdvertismentPrices.Id	=  Advertisments.AdvertismentPriceId) AS AdvertisementPeriod,
   
	  (SELECT  AdvertismentPrices.Price FROM AdvertismentPrices WHERE AdvertismentPrices.Id	=  Advertisments.AdvertismentPriceId) AS AdvertisementPrice,
	--AdvertismentPrices.Period 'AdvertisementPeriod',
	--AdvertismentPrices.Price 'AdvertisementPrice',
	AspNetUsers.Name 'FullName',
	AspNetUsers.UserName
	, COUNT(Advertisments.Id) OVER() AS 'OverAllCount',
	Advertisments.*
	FROM Advertisments
 --   join AdvertismentImages 
	--on 
	--AdvertismentImages.AdvertismentId = Advertisments.Id
	join Categories
	ON
	Advertisments.CategoryId = Categories.Id  
	 JOIN AspNetUsers    
	ON   AspNetUsers.Id = Advertisments.ApplicationUserId   
	where
	
	Advertisments.IsArchieved = ISNULL(@IsArchieved, Advertisments.IsArchieved)
	
	AND
	(Advertisments.Name LIKE '%' + @filter + '%' OR
	
	Advertisments.[Description] LIKE '%' + @filter + '%' OR
	
	Categories.Name LIKE '%' + @filter + '%' OR
	
	--AdvertismentPrices.[Period] LIKE '%' + @filter + '%' OR
	
	--AdvertismentPrices.Price like '%' + @filter + '%' OR
	
	AspNetUsers.Name LIKE '%' + @filter + '%' or
	
	AspNetUsers.UserName LIKE '%' + @filter + '%')
	
	AND(@CategoryId IS NULL OR Categories.Id = @CategoryId)
	
	AND(@IsPaided IS NULL OR Advertisments.IsPaided = @IsPaided)
	
	AND(@IsExpired IS NULL OR Advertisments.IsExpired = @IsExpired)
	
	AND(@UserId IS NULL OR Advertisments.ApplicationUserId = @UserId)
	ORDER BY Advertisments.CreateDate Desc OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY;
	
END");
			/****** Object:  StoredProcedure [dbo].[AdvertisementGetByUsername]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"

ALTER  PROCEDURE [dbo].[AdvertisementGetByUsername]
	@username [nvarchar](256),
	@index [int],
	@rowNumber [int],
	@filter [nvarchar](100) = '',
	@IsArchieved [bit] = NULL
AS
BEGIN
	
	SELECT
						[Advertisments].Id,
						[Advertisments].Name,
						[Advertisments].Cost,
						(select ap.Period FROM dbo.AdvertismentPrices ap WHERE ap.Id=[Advertisments].AdvertismentPriceId) AS 'AdvertisementPeriod',
		(select ap.Price FROM dbo.AdvertismentPrices ap WHERE ap.Id	=[Advertisments].AdvertismentPriceId) as  'AdvertisementPrice' ,
		Advertisments.AdvertismentPriceId,
		Advertisments.PaidEdPrice,
		Advertisments.IsPaided	,
		dbo.Advertisments.IsExpired,
		Advertisments.IsActive,
		 
	
						Advertisments.StartDate,
						Advertisments.EndDate,
						Advertisments.CreateDate,
						ISNULL([NumberOfViews],0) AS 'NumberOfViews',
	COUNT(Advertisments.Id) OVER() AS 'OverAllCount',
						ISNULL(NumberOfLikes,0) AS 'NumberOfLikes',
						(
							SELECT
							COUNT(Id)
							FROM [dbo].[Comments]
							WHERE [dbo].[Comments].AdvertismentId=[dbo].[Advertisments].Id
						) AS 'Comments',
						(
							SELECT
							TOP 1 [ImageUrl]
							FROM [dbo].[AdvertismentImages]
							WHERE [dbo].[AdvertismentImages].AdvertismentId=[dbo].[Advertisments].Id
							ORDER BY [IsMainImage] DESC
						) AS 'ImageUrl'
	
					FROM [dbo].[Advertisments] join
					AspNetUsers ON
					AspNetUsers.Id = Advertisments.ApplicationUserId

					

					WHERE 
						[dbo].[Advertisments].IsArchieved = ISNULL(@IsArchieved,Advertisments.IsArchieved)
					AND 
						[dbo].[AspNetUsers].UserName = @username
					AND 
	(Advertisments.Name LIKE '%'+@filter +'%' OR Advertisments.[Description] LIKE '%'+@filter +'%')
	
					ORDER BY Advertisments.CreateDate Desc OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY;
	
	
END");
			/****** Object:  StoredProcedure [dbo].[AdvertisementGetSingle]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"

ALTER  PROCEDURE [dbo].[AdvertisementGetSingle]
	@id [int]
AS
BEGIN
	
		 select
						Advertisments.Id , 
						Advertisments.Name,
		AdvertismentPrices.Period 'AdvertisementPeriod',
		AdvertismentPrices.Price 'AdvertisementPrice' ,
		AspNetUsers.Name 'FullName',
		Advertisments.AdvertismentPriceId,Advertisments.PaidEdPrice

		,
						AspNetUsers.PhotoUrl ,
		AspNetUsers.UserName,
		Advertisments.IsPaided	,dbo.Advertisments.IsExpired,Advertisments.IsActive,
		 
	
						Advertisments.StartDate,
						  Advertisments.Cost,
						Advertisments.EndDate,
						Advertisments.CreateDate,
						Advertisments.ApplicationUserId,
						Advertisments.[Description],
						Advertisments.CategoryId,
						AspNetUsers.PhoneNumber,
						ISNULL([NumberOfViews],0) AS 'NumberOfViews',
						ISNULL(NumberOfLikes,0) AS 'NumberOfLikes',
						(
							SELECT
							COUNT(Id)
							FROM [dbo].[Comments]
							WHERE [dbo].[Comments].AdvertismentId=[dbo].[Advertisments].Id
						) AS 'Comments',
						(
							SELECT
							TOP 1 [ImageUrl]
							FROM [dbo].[AdvertismentImages]
							WHERE [dbo].[AdvertismentImages].AdvertismentId=[dbo].[Advertisments].Id
							ORDER BY [IsMainImage] DESC
						) AS 'ImageUrl'
	
							
		FROM Advertisments
							
		left join Categories on 
		Advertisments.CategoryId = Categories.Id 
		left join AdvertismentPrices on
		Advertisments.AdvertismentPriceId = AdvertismentPrices.Id
		left join AspNetUsers on
		AspNetUsers.Id = Advertisments.ApplicationUserId
							
	where Advertisments.Id = @id
					 
END");
			/****** Object:  StoredProcedure [dbo].[AdvertisementImageResetMain]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AdvertisementImageResetMain]
	@id [int]
AS
BEGIN
	
	update AdvertismentImages set IsMainImage = 0 where AdvertismentImages.AdvertismentId = @id  
	
END");
			/****** Object:  StoredProcedure [dbo].[Advertisment_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Advertisment_Delete]
	@Id [int]
AS
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION
	
	DELETE FROM AdvertismentImages WHERE  AdvertismentId = @Id
						DELETE FROM  ChatRequests WHERE AdvertismentId = @Id
						DELETE FROM Comments WHERE AdvertismentId = @Id
						DELETE FROM Complaints WHERE AdvertismentId = @Id
						DELETE FROM Favorites WHERE AdvertismentId = @Id
						DELETE FROM Likes WHERE AdvertismentId = @Id
						DELETE FROM Advertisments WHERE Id = @Id
	
	COMMIT TRANSACTION
	SELECT 1 
	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK;
	SELECT 0
	END CATCH
END");
			/****** Object:  StoredProcedure [dbo].[Advertisment_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Advertisment_Insert]
	@IsActive [bit],
	@Name [nvarchar](max),
	@IsPaided [bit],
	@Description [nvarchar](max),
	@NumberOfViews [int],
	@NumberOfLikes [int],
	@PaidEdPrice [decimal](18, 2),
	@IsArchieved [bit],
	@CreateDate [datetime],
	@StartDate [datetime],
	@EndDate [datetime],
	@IsExpired [bit],
	@Cost [decimal](18, 2),
	@CategoryId [int],
	@ApplicationUserId [nvarchar](128),
	@AdvertismentPriceId [int]
AS
BEGIN
	INSERT [dbo].[Advertisments]([IsActive], [Name], [IsPaided], [Description], [NumberOfViews], [NumberOfLikes], [PaidEdPrice], [IsArchieved], [CreateDate], [StartDate], [EndDate], [IsExpired], [Cost], [CategoryId], [ApplicationUserId], [AdvertismentPriceId])
	VALUES (@IsActive, @Name, @IsPaided, @Description, @NumberOfViews, @NumberOfLikes, @PaidEdPrice, @IsArchieved, @CreateDate, @StartDate, @EndDate, @IsExpired, @Cost, @CategoryId, @ApplicationUserId, @AdvertismentPriceId)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[Advertisments]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[Advertisments] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[Advertisment_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Advertisment_Update]
	@Id [int],
	@IsActive [bit],
	@Name [nvarchar](max),
	@IsPaided [bit],
	@Description [nvarchar](max),
	@NumberOfViews [int],
	@NumberOfLikes [int],
	@PaidEdPrice [decimal](18, 2),
	@IsArchieved [bit],
	@CreateDate [datetime],
	@StartDate [datetime],
	@EndDate [datetime],
	@IsExpired [bit],
	@Cost [decimal](18, 2),
	@CategoryId [int],
	@ApplicationUserId [nvarchar](128),
	@AdvertismentPriceId [int]
AS
BEGIN
	UPDATE [dbo].[Advertisments]
	SET [IsActive] = @IsActive, [Name] = @Name, [IsPaided] = @IsPaided, [Description] = @Description, [NumberOfViews] = @NumberOfViews, [NumberOfLikes] = @NumberOfLikes, [PaidEdPrice] = @PaidEdPrice, [IsArchieved] = @IsArchieved, [CreateDate] = @CreateDate, [StartDate] = @StartDate, [EndDate] = @EndDate, [IsExpired] = @IsExpired, [Cost] = @Cost, [CategoryId] = @CategoryId, [ApplicationUserId] = @ApplicationUserId, [AdvertismentPriceId] = @AdvertismentPriceId
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[AdvertismentImage_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AdvertismentImage_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[AdvertismentImages]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[AdvertismentImage_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AdvertismentImage_Insert]
	@IsMainImage [bit],
	@ImageUrl [nvarchar](500),
	@AdvertismentId [int]
AS
BEGIN
	INSERT [dbo].[AdvertismentImages]([IsMainImage], [ImageUrl], [AdvertismentId])
	VALUES (@IsMainImage, @ImageUrl, @AdvertismentId)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[AdvertismentImages]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[AdvertismentImages] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[AdvertismentImage_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AdvertismentImage_Update]
	@Id [int],
	@IsMainImage [bit],
	@ImageUrl [nvarchar](500),
	@AdvertismentId [int]
AS
BEGIN
	UPDATE [dbo].[AdvertismentImages]
	SET [IsMainImage] = @IsMainImage, [ImageUrl] = @ImageUrl, [AdvertismentId] = @AdvertismentId
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[AdvertismentPrice_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AdvertismentPrice_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[AdvertismentPrices]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[AdvertismentPrice_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AdvertismentPrice_Insert]
	@Period [nvarchar](200),
	@Price [decimal](18, 2),
	@IsArchieved [bit]
AS
BEGIN
	INSERT [dbo].[AdvertismentPrices]([Period], [Price], [IsArchieved])
	VALUES (@Period, @Price, @IsArchieved)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[AdvertismentPrices]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[AdvertismentPrices] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[AdvertismentPrice_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AdvertismentPrice_Update]
	@Id [int],
	@Period [nvarchar](200),
	@Price [decimal](18, 2),
	@IsArchieved [bit]
AS
BEGIN
	UPDATE [dbo].[AdvertismentPrices]
	SET [Period] = @Period, [Price] = @Price, [IsArchieved] = @IsArchieved
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[AdvertismentTransaction_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AdvertismentTransaction_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[AdvertismentTransactions]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[AdvertismentTransaction_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AdvertismentTransaction_Insert]
	@PaymentId [nvarchar](max),
	@CreateDate [datetime],
	@AdvertismentId [int]
AS
BEGIN
	INSERT [dbo].[AdvertismentTransactions]([PaymentId], [CreateDate], [AdvertismentId])
	VALUES (@PaymentId, @CreateDate, @AdvertismentId)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[AdvertismentTransactions]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[AdvertismentTransactions] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[AdvertismentTransaction_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AdvertismentTransaction_Update]
	@Id [int],
	@PaymentId [nvarchar](max),
	@CreateDate [datetime],
	@AdvertismentId [int]
AS
BEGIN
	UPDATE [dbo].[AdvertismentTransactions]
	SET [PaymentId] = @PaymentId, [CreateDate] = @CreateDate, [AdvertismentId] = @AdvertismentId
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[AspNetUsers_Delete_ById]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
Alter  PROCEDURE [dbo].[AspNetUsers_Delete_ById]
	@UserId nvarchar(128)
  
AS
BEGIN
   
   BEGIN TRY
	BEGIN TRANSACTION
	   DELETE  FROM dbo.Favorites	WHERE	dbo.Favorites.ApplicationUserId=@UserId	
	   DELETE  FROM dbo.Complaints	WHERE	dbo.Complaints.ApplicationUserId=@UserId	   
	   DELETE  FROM dbo.Likes	WHERE	dbo.Likes.ApplicationUserId	=@UserId
	   DELETE FROM dbo.NotificationLogs	 WHERE   dbo.NotificationLogs.ApplicationUserId= @UserId		  
	   DELETE FROM	dbo.AspNetUsers	WHERE dbo.AspNetUsers.Id=@UserId
			   
	COMMIT
	SELECT 1;
END TRY
BEGIN CATCH
	ROLLBACK
	SELECT 0;
END CATCH


END");
			/****** Object:  StoredProcedure [dbo].[AspNetUsers_GetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AspNetUsers_GetAll]
AS
BEGIN
	SELECT  *   
	FROM            
	AspNetUsers 
	ORDER BY 
	[dbo].[AspNetUsers].[Id]
END");
			/****** Object:  StoredProcedure [dbo].[AspNetUsers_GetAllFilterd]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AspNetUsers_GetAllFilterd]
	@PageNumber [int],
	@PageSize [int],
	@Filter [nvarchar](250),
	@Role [nvarchar](250)
AS
BEGIN
	SELECT   
	[dbo].[AspNetUsers].Id,
	COUNT([dbo].[AspNetUsers].Id) OVER() AS 'OverAllCount', 
	
	[dbo].[AspNetUsers].[PhotoUrl],   AspNetUsers.UserName,               
											[dbo].[AspNetUsers].[PhoneNumber],   
	AspNetUsers.Name ,
											AspNetUsers.[Email],
	(
	SELECT CAST
	(
	CASE WHEN EXISTS(
	
	SELECT LoginProvider FROM AspNetUserLogins WHERE AspNetUserLogins.UserId= AspNetUsers.Id
	) THEN 1 
	ELSE 0 
	END
	AS BIT)
	) As 'IsSoicalLogin'
	
										   
	FROM
	AspNetUsers
	WHERE
	Id IN
	(
	SELECT UserId From AspNetUserRoles
	INNER JOIN [dbo].[AspNetRoles]
	ON AspNetUserRoles.RoleId=[AspNetRoles].Id
	WHERE[dbo].[AspNetRoles].[Name]= ISNULL(@Role,[dbo].[AspNetRoles].[Name])
	
	)
	
	AND(@Filter is null or Name LIKE '%' + @Filter + '%')
	ORDER BY
	[dbo].[AspNetUsers].[Id]
	DESC OFFSET(@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END");
			/****** Object:  StoredProcedure [dbo].[AspNetUsers_GetSingle]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[AspNetUsers_GetSingle]
	@Id [nvarchar](128)
AS
BEGIN
	
	SELECT *  	                
	FROM            
	AspNetUsers 
	where 
	AspNetUsers.Id= @Id
	ORDER BY 
	[dbo].[AspNetUsers].[Id] 
END");
			/****** Object:  StoredProcedure [dbo].[BankAccount_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[BankAccount_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[BankAccounts]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[BankAccount_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[BankAccount_Insert]
	@BankNumber [nvarchar](200),
	@BankName [nvarchar](100)
AS
BEGIN
	INSERT [dbo].[BankAccounts]([BankNumber], [BankName])
	VALUES (@BankNumber, @BankName)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[BankAccounts]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[BankAccounts] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[BankAccount_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[BankAccount_Update]
	@Id [int],
	@BankNumber [nvarchar](200),
	@BankName [nvarchar](100)
AS
BEGIN
	UPDATE [dbo].[BankAccounts]
	SET [BankNumber] = @BankNumber, [BankName] = @BankName
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[Category_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Category_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[Categories]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[Category_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Category_Insert]
	@Name [nvarchar](150),
	@IsArchieved [bit],
	@IconName [nvarchar](50),
	@ImageUrl [nvarchar](max)
AS
BEGIN
	INSERT [dbo].[Categories]([Name], [IsArchieved], [IconName], [ImageUrl])
	VALUES (@Name, @IsArchieved, @IconName, @ImageUrl)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[Categories]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[Categories] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[Category_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Category_Update]
	@Id [int],
	@Name [nvarchar](150),
	@IsArchieved [bit],
	@IconName [nvarchar](50),
	@ImageUrl [nvarchar](max)
AS
BEGIN
	UPDATE [dbo].[Categories]
	SET [Name] = @Name, [IsArchieved] = @IsArchieved, [IconName] = @IconName, [ImageUrl] = @ImageUrl
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[CategoryGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"

ALTER  PROCEDURE [dbo].[CategoryGetAll]
	@index [int],
	@rowNumber [int],
	@filter [nvarchar](100) = '',
	@IsArchieved [bit] = NULL
AS
BEGIN
	
	SELECT *,COUNT(Categories.Id) OVER() AS 'OverAllCount' FROM Categories 
		Where  Categories.IsArchieved =isnull(@IsArchieved , IsArchieved) and 
	(Categories.Name like '%'+@filter+'%')
		ORDER BY Id DESC OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY
	
END");
			/****** Object:  StoredProcedure [dbo].[CategoryGetSingle]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[CategoryGetSingle]
	@id [int]
AS
BEGIN
	
	select * from Categories 
	where Categories.Id = @id
	
END");
			/****** Object:  StoredProcedure [dbo].[ChatHeader_ById]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ChatHeader_ById]
	@Id [int]
AS
BEGIN
	  select * from [dbo].[ChatHeaders] 
	where [dbo].[ChatHeaders].[Id] = @Id
	
END");
			/****** Object:  StoredProcedure [dbo].[ChatHeader_ListByAdvertismentId]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ChatHeader_ListByAdvertismentId]
	@AdvertismentId [int],
	@PageNumber [int] = 1,
	@PageSize [int] = 8
AS
BEGIN
	 SELECT Id, ( SELECT    UserName
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
	) AS 'UserName'
														 ,
	( SELECT    PhotoUrl
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
	) AS 'PhotoUrl',
							 ( SELECT  Id
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
	) AS 'ReceiverId'
	,
							 ( SELECT  UserName
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
	) AS 'ReceiverUserName'
	,	
							 ( SELECT  Name
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
	) AS 'ReceiverName'
							,
	( SELECT    PhotoUrl
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
	) AS 'ReceiverPhotoUrl',
	
		
	( SELECT    TOP(1) MessageContent
	FROM      dbo.ChatMessages
	WHERE     dbo.ChatMessages.ChatId = dbo.ChatHeaders.Id
														  ORDER BY SentDate DESC
	) AS 'MessageContent'
														,OverallCount = COUNT(1) OVER(),
														 ( SELECT    TOP(1) SentDate
	FROM      dbo.ChatMessages
	WHERE     dbo.ChatMessages.ChatId = dbo.ChatHeaders.Id
														  ORDER BY SentDate DESC
	) AS 'SentDate'
	FROM    dbo.ChatHeaders
	WHERE   RequestId IN ( SELECT   Id
	FROM     dbo.ChatRequests
	WHERE    AdvertismentId = @AdvertismentId )
																		ORDER BY
																CreateDate Desc
															OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY OPTION (RECOMPILE);
END");
			/****** Object:  StoredProcedure [dbo].[ChatHeader_ListByUserId]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ChatHeader_ListByUserId]
	@UserId [nvarchar](128),
	@PageNumber [int] = 1,
	@PageSize [int] = 8,
	@AdvertismentId [int]
AS
BEGIN
	SELECT * ,OverallCount = COUNT(1) OVER() FROM
	(
	SELECT  ChatHeaders.Id ,
	( SELECT    UserName
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
	) AS 'UserName',
	( SELECT    PhotoUrl
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
	) AS 'PhotoUrl' ,
					 ( SELECT  Id
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
	) AS 'ReceiverId'
	,
					 ( SELECT  UserName
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
	) AS 'ReceiverUserName'
					,
		  
					 ( SELECT  Name
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
	) AS 'ReceiverName'
	,	
	
	( SELECT    PhotoUrl
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
	) AS 'ReceiverPhotoUrl',
	( SELECT TOP ( 1 )
	MessageContent
	FROM      dbo.ChatMessages
	WHERE     dbo.ChatMessages.ChatId = dbo.ChatHeaders.Id
	ORDER BY  SentDate DESC
	) AS 'MessageContent' ,
	( SELECT TOP ( 1 )
	SentDate
	FROM      dbo.ChatMessages
	WHERE     dbo.ChatMessages.ChatId = dbo.ChatHeaders.Id
	ORDER BY  SentDate DESC
	) AS 'SentDate'
	FROM    dbo.ChatHeaders inner join ChatRequests on ChatHeaders.RequestId = ChatRequests.Id
	WHERE   UserOneId = @UserId and ChatRequests.AdvertismentId = @AdvertismentId
	
								UNION 		
	SELECT  ChatHeaders.Id ,
	( SELECT    UserName
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
	)AS 'UserName' ,
	( SELECT    PhotoUrl
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
	) AS 'PhotoUrl' ,
					 ( SELECT  Id
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
	) AS 'ReceiverId'
	,
					 ( SELECT  UserName
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
	) AS 'ReceiverUserName'
	,	
	
					 ( SELECT  Name
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
	) AS 'ReceiverName'
					,
	
	( SELECT    PhotoUrl
	FROM      dbo.AspNetUsers
	WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
	) AS 'ReceiverPhotoUrl',
	
	( SELECT TOP ( 1 )
	MessageContent
	FROM      dbo.ChatMessages
	WHERE     dbo.ChatMessages.ChatId = dbo.ChatHeaders.Id
	ORDER BY  SentDate DESC
	) AS 'MessageContent' ,
	( SELECT TOP ( 1 )
	SentDate
	FROM      dbo.ChatMessages
	WHERE     dbo.ChatMessages.ChatId = dbo.ChatHeaders.Id
	ORDER BY  SentDate DESC
	) AS 'SentDate'
	FROM    dbo.ChatHeaders inner join ChatRequests on ChatHeaders.RequestId = ChatRequests.Id
	WHERE   UserTwoId = @UserId and ChatRequests.AdvertismentId = @AdvertismentId
	) s
END");
			/****** Object:  StoredProcedure [dbo].[ChatMessage_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ChatMessage_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[ChatMessages]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[ChatMessage_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ChatMessage_Insert]
	@MessageContent [nvarchar](max),
	@SentDate [datetime],
	@SenderId [nvarchar](128),
	@ChatId [int]
AS
BEGIN
	INSERT [dbo].[ChatMessages]([MessageContent], [SentDate], [SenderId], [ChatId])
	VALUES (@MessageContent, @SentDate, @SenderId, @ChatId)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[ChatMessages]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[ChatMessages] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[ChatMessage_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ChatMessage_Update]
	@Id [int],
	@MessageContent [nvarchar](max),
	@SentDate [datetime],
	@SenderId [nvarchar](128),
	@ChatId [int]
AS
BEGIN
	UPDATE [dbo].[ChatMessages]
	SET [MessageContent] = @MessageContent, [SentDate] = @SentDate, [SenderId] = @SenderId, [ChatId] = @ChatId
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[ChatMessages_SelectList]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ChatMessages_SelectList]
	@PageNumber [int] = 1,
	@PageSize [int] = 8,
	@LastSendDate [datetime],
	@ChatId [int]
AS
BEGIN
		
	SELECT  ChatMessages.Id ,
	MessageContent ,
	SentDate ,
	CONVERT(char(12), SentDate, 114) AS 'FullDate',
	SenderId ,
	ChatId ,
	OverallCount = COUNT(1) OVER ( ),
	UserName
	FROM    dbo.ChatMessages
	INNER JOIN [dbo].[AspNetUsers] ON SenderId = [dbo].[AspNetUsers].Id
	WHERE  ChatId=@ChatId And SentDate >= ISNULL(@LastSendDate, SentDate)
	ORDER BY SentDate DESC
	OFFSET @PageSize * ( @PageNumber - 1 ) ROWS
	FETCH NEXT @PageSize ROWS ONLY
	OPTION  ( RECOMPILE );
	
END");
			/****** Object:  StoredProcedure [dbo].[ChatRequests_Add]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ChatRequests_Add] 
	@AdvertismentId [int],
	@RequestAuthorId [nvarchar](128),
	@RequestDate [datetime]

AS
BEGIN
	  
	-- CHECK EXIT
	DECLARE @Id INT ,
	@RequestId INT ,
	@ChatId INT ,
	@TemChatID INT ,
	@TempId INT ,
	@UserOne NVARCHAR(128) ,
	@UserTwo NVARCHAR(128) ,
	@MessageContent NVARCHAR(MAX);
	BEGIN
	BEGIN TRY
	BEGIN TRANSACTION;
	IF NOT EXISTS ( SELECT  Id
	FROM    ChatRequests
	WHERE   AdvertismentId = @AdvertismentId And [RequestAuthorId]=@RequestAuthorId )
	BEGIN		
	INSERT  INTO ChatRequests
	( AdvertismentId ,
	RequestAuthorId ,
	RequestDate
	)
	VALUES  (@AdvertismentId ,
	@RequestAuthorId ,
	@RequestDate 
	);
	
	SELECT  @Id = Id
	FROM    dbo.ChatRequests
	WHERE   @@ROWCOUNT > 0
	AND Id = SCOPE_IDENTITY();
	SET @RequestId = ( SELECT   t0.Id
	FROM     dbo.ChatRequests AS t0
	WHERE    @@ROWCOUNT > 0
	AND t0.Id = @Id
	);
	
	SET @UserTwo  = @RequestAuthorId;
	SET @UserOne = (SELECT ApplicationUserId
	FROM   [dbo].[Advertisments]
	WHERE  Id = @AdvertismentId
	);
	INSERT  INTO dbo.ChatHeaders
	( RequestId ,
	UserOneId ,
	UserTwoId ,
	CreateDate
	)
	VALUES  ( @RequestId ,
	@UserOne ,
	@UserTwo ,
	@RequestDate
	);
	
	SELECT  @TemChatID = Id
	FROM    dbo.ChatHeaders
	WHERE   @@ROWCOUNT > 0
	AND Id = SCOPE_IDENTITY();
	SET @ChatId = ( SELECT  t0.Id
	FROM    dbo.ChatHeaders AS t0
	WHERE   @@ROWCOUNT > 0
	AND t0.Id = @TemChatID
	);
	
	INSERT  INTO dbo.ChatMessages
	( MessageContent ,
	SentDate ,
	SenderId ,
	ChatId
	)
	VALUES  ( @MessageContent ,
	@RequestDate ,
	@RequestAuthorId ,
	@ChatId
	);
	
	SELECT  @ChatId;
	END;
	ELSE
	BEGIN
	SET @ChatId = ( SELECT  Id
	FROM    dbo.ChatHeaders
	WHERE   RequestId = ( SELECT
	Id
	FROM
	dbo.ChatRequests
	WHERE
	AdvertismentId = @AdvertismentId And [RequestAuthorId]=  @RequestAuthorId
	)
	);
	SELECT  @ChatId;
	END;
	COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
	ROLLBACK;
	
	END CATCH;
	END;
	
END");
			/****** Object:  StoredProcedure [dbo].[ChatRequests_ById]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ChatRequests_ById]
	@Id [int]
AS
BEGIN
	 select * from [dbo].[ChatRequests]
	where [dbo].[ChatRequests].[Id] = @Id
	
END");
			/****** Object:  StoredProcedure [dbo].[Comment_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Comment_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[Comments]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[Comment_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Comment_Insert]
	@Message [nvarchar](max),
	@CreateDate [datetime],
	@AdvertismentId [int],
	@ApplicationUserId [nvarchar](128),
	@CommentParentId [int]
AS
BEGIN
	INSERT [dbo].[Comments]([Message], [CreateDate], [AdvertismentId], [ApplicationUserId], [CommentParentId])
	VALUES (@Message, @CreateDate, @AdvertismentId, @ApplicationUserId, @CommentParentId)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[Comments]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[Comments] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[Comment_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Comment_Update]
	@Id [int],
	@Message [nvarchar](max),
	@CreateDate [datetime],
	@AdvertismentId [int],
	@ApplicationUserId [nvarchar](128),
	@CommentParentId [int]
AS
BEGIN
	UPDATE [dbo].[Comments]
	SET [Message] = @Message, [CreateDate] = @CreateDate, [AdvertismentId] = @AdvertismentId, [ApplicationUserId] = @ApplicationUserId, [CommentParentId] = @CommentParentId
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[Complaint_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Complaint_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[Complaints]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[Complaint_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Complaint_Insert]
	@Message [nvarchar](max),
	@IsArchieved [bit],
	@ApplicationUserId [nvarchar](128),
	@AdvertismentId [int],
	@ComplainedId [nvarchar](128)
AS
BEGIN
	INSERT [dbo].[Complaints]([Message], [IsArchieved], [ApplicationUserId], [AdvertismentId], [ComplainedId])
	VALUES (@Message, @IsArchieved, @ApplicationUserId, @AdvertismentId, @ComplainedId)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[Complaints]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[Complaints] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[Complaint_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Complaint_Update]
	@Id [int],
	@Message [nvarchar](max),
	@IsArchieved [bit],
	@ApplicationUserId [nvarchar](128),
	@AdvertismentId [int],
	@ComplainedId [nvarchar](128)
AS
BEGIN
	UPDATE [dbo].[Complaints]
	SET [Message] = @Message, [IsArchieved] = @IsArchieved, [ApplicationUserId] = @ApplicationUserId, [AdvertismentId] = @AdvertismentId, [ComplainedId] = @ComplainedId
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[ContactInformation_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ContactInformation_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[ContactInformations]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[ContactInformation_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ContactInformation_Insert]
	@Contact [nvarchar](200),
	@ContactTypeId [int],
	@IconName [nvarchar](70)
AS
BEGIN
	INSERT [dbo].[ContactInformations]([Contact], [ContactTypeId], [IconName])
	VALUES (@Contact, @ContactTypeId, @IconName)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[ContactInformations]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[ContactInformations] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[ContactInformation_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ContactInformation_Update]
	@Id [int],
	@Contact [nvarchar](200),
	@ContactTypeId [int],
	@IconName [nvarchar](70)
AS
BEGIN
	UPDATE [dbo].[ContactInformations]
	SET [Contact] = @Contact, [ContactTypeId] = @ContactTypeId, [IconName] = @IconName
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[ContactType_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ContactType_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[ContactTypes]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[ContactType_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ContactType_Insert]
	@Type [nvarchar](50)
AS
BEGIN
	INSERT [dbo].[ContactTypes]([Type])
	VALUES (@Type)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[ContactTypes]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[ContactTypes] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[ContactType_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ContactType_Update]
	@Id [int],
	@Type [nvarchar](50)
AS
BEGIN
	UPDATE [dbo].[ContactTypes]
	SET [Type] = @Type
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[ContactUsMessage_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ContactUsMessage_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[ContactUsMessages]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[ContactUsMessage_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ContactUsMessage_Insert]
	@Name [nvarchar](max),
	@Phone [nvarchar](max),
	@Email [nvarchar](max),
	@Message [nvarchar](max),
	@IsArchieved [bit],
	@CreatedDate [datetime]
AS
BEGIN
	INSERT [dbo].[ContactUsMessages]([Name], [Phone], [Email], [Message], [IsArchieved], [CreatedDate])
	VALUES (@Name, @Phone, @Email, @Message, @IsArchieved, @CreatedDate)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[ContactUsMessages]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[ContactUsMessages] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[ContactUsMessage_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[ContactUsMessage_Update]
	@Id [int],
	@Name [nvarchar](max),
	@Phone [nvarchar](max),
	@Email [nvarchar](max),
	@Message [nvarchar](max),
	@IsArchieved [bit],
	@CreatedDate [datetime]
AS
BEGIN
	UPDATE [dbo].[ContactUsMessages]
	SET [Name] = @Name, [Phone] = @Phone, [Email] = @Email, [Message] = @Message, [IsArchieved] = @IsArchieved, [CreatedDate] = @CreatedDate
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[Error_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Error_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[Errors]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[Error_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Error_Insert]
	@Message [nvarchar](max),
	@StackTrace [nvarchar](max),
	@DateCreated [datetime]
AS
BEGIN
	INSERT [dbo].[Errors]([Message], [StackTrace], [DateCreated])
	VALUES (@Message, @StackTrace, @DateCreated)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[Errors]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[Errors] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[Error_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Error_Update]
	@Id [int],
	@Message [nvarchar](max),
	@StackTrace [nvarchar](max),
	@DateCreated [datetime]
AS
BEGIN
	UPDATE [dbo].[Errors]
	SET [Message] = @Message, [StackTrace] = @StackTrace, [DateCreated] = @DateCreated
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[Favorite_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Favorite_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[Favorites]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[Favorite_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Favorite_Insert]
	@ApplicationUserId [nvarchar](128),
	@AdvertismentId [int]
AS
BEGIN
	INSERT [dbo].[Favorites]([ApplicationUserId], [AdvertismentId])
	VALUES (@ApplicationUserId, @AdvertismentId)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[Favorites]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[Favorites] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[Favorite_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Favorite_Update]
	@Id [int],
	@ApplicationUserId [nvarchar](128),
	@AdvertismentId [int]
AS
BEGIN
	UPDATE [dbo].[Favorites]
	SET [ApplicationUserId] = @ApplicationUserId, [AdvertismentId] = @AdvertismentId
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[Like_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Like_Delete]
	@Id [int]
AS
BEGIN
	DELETE [dbo].[Likes]
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[Like_Insert]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Like_Insert]
	@ApplicationUserId [nvarchar](128),
	@AdvertismentId [int]
AS
BEGIN
	INSERT [dbo].[Likes]([ApplicationUserId], [AdvertismentId])
	VALUES (@ApplicationUserId, @AdvertismentId)
	
	DECLARE @Id int
	SELECT @Id = [Id]
	FROM [dbo].[Likes]
	WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
	
	SELECT t0.[Id]
	FROM [dbo].[Likes] AS t0
	WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END");
			/****** Object:  StoredProcedure [dbo].[Like_Update]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[Like_Update]
	@Id [int],
	@ApplicationUserId [nvarchar](128),
	@AdvertismentId [int]
AS
BEGIN
	UPDATE [dbo].[Likes]
	SET [ApplicationUserId] = @ApplicationUserId, [AdvertismentId] = @AdvertismentId
	WHERE ([Id] = @Id)
END");
			/****** Object:  StoredProcedure [dbo].[SPGetAllCategory]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[SPGetAllCategory]
AS
BEGIN
	
							SELECT * from Categories 
							where Categories.IsArchieved != 1
							
END");
			/****** Object:  StoredProcedure [dbo].[SPGetSingleCategory]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE [dbo].[SPGetSingleCategory]
	@Id [int]
AS
BEGIN
	
							SELECT * from Categories 
							where Categories.Id = @Id
							
END");
			/****** Object:  StoredProcedure  [AdvertisementDeleteGetSingle]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [AdvertisementDeleteGetSingle]
	@id [int]
AS
BEGIN
	
	select * from Advertisments where Advertisments.Id = @id
	
END");
			/****** Object:  StoredProcedure  [AdvertisementGetAllByCategoryId]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"

Alter PROCEDURE  [AdvertisementGetAllByCategoryId]
	@index [int],
	@rowNumber [int],
	@filter [nvarchar](100) = '',
	@IsArchieved [bit] = NULL,
	@CategoryId [int] = NULL,
	@IsActive [bit] = NULL
AS
BEGIN
	
	 UPDATE dbo.Advertisments
	SET dbo.Advertisments.IsExpired = 1 WHERE
	 [Advertisments].EndDate <= GetDate()


	SELECT
			[Id],
			[Name],
			[Cost],
	COUNT(Advertisments.Id) OVER() AS 'OverAllCount',
			ISNULL([NumberOfViews],0) AS 'NumberOfViews',
			ISNULL(NumberOfLikes,0) AS 'NumberOfLikes',
			(
				SELECT
				COUNT(Id)
				FROM [dbo].[Comments]
				WHERE [dbo].[Comments].AdvertismentId=[dbo].[Advertisments].Id
			) AS 'Comments',
			(
				SELECT
				TOP 1 [ImageUrl]
				FROM [dbo].[AdvertismentImages]
				WHERE [dbo].[AdvertismentImages].AdvertismentId=[dbo].[Advertisments].Id
				ORDER BY [IsMainImage] DESC
			) AS 'ImageUrl',
			IsPaided,IsExpired
	
		FROM [dbo].[Advertisments] 
		WHERE 
		  
		[dbo].[Advertisments].CategoryId=ISNULL(@CategoryId,[dbo].[Advertisments].CategoryId) 
		AND
		(  @IsArchieved IS NULL OR    Advertisments.IsArchieved IS null  or  Advertisments.IsArchieved=@IsArchieved)
		AND 
	(@filter is null or (Advertisments.Name LIKE '%'+@filter +'%' OR Advertisments.[Description] LIKE '%'+@filter +'%'))
	and (@IsActive IS NULL OR IsActive = @IsActive)
		ORDER BY   dbo.Advertisments.IsExpired asc,dbo.Advertisments.IsPaided Desc OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY;
	
END");
			/****** Object:  StoredProcedure  [AdvertisementGetByUserId]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"

Alter  PROCEDURE  [AdvertisementGetByUserId]
	@UserId [nvarchar](128),
	@PageNumber [int] = 1,
	@PageSize [int] = 8,
	@Filter [nvarchar](250) = NULL,
	@IsArchieved [bit] = NULL,
	@IsActive [bit] = NULL
AS
BEGIN
	  SELECT
						[Advertisments].Id,
						[Advertisments].Name,
						[Advertisments].Cost,
		(select ap.Period FROM dbo.AdvertismentPrices ap WHERE ap.Id=[Advertisments].AdvertismentPriceId) AS 'AdvertisementPeriod',
		(select ap.Price FROM dbo.AdvertismentPrices ap WHERE ap.Id	=[Advertisments].AdvertismentPriceId) as  'AdvertisementPrice' ,
		Advertisments.AdvertismentPriceId,
		Advertisments.PaidEdPrice,
		Advertisments.IsPaided	,
		dbo.Advertisments.IsExpired,
		Advertisments.IsActive,
		 
	
						Advertisments.StartDate,
						Advertisments.EndDate,
						Advertisments.CreateDate,
						ISNULL([NumberOfViews],0) AS 'NumberOfViews',
							COUNT(Advertisments.Id) OVER() AS 'OverAllCount',
						ISNULL(NumberOfLikes,0) AS 'NumberOfLikes',
						(
							SELECT
							COUNT(Id)
							FROM [dbo].[Comments]
							WHERE [dbo].[Comments].AdvertismentId=[dbo].[Advertisments].Id
						) AS 'Comments',
						(
							SELECT
							TOP 1 [ImageUrl]
							FROM [dbo].[AdvertismentImages]
							WHERE [dbo].[AdvertismentImages].AdvertismentId=[dbo].[Advertisments].Id
							ORDER BY [IsMainImage] DESC
						) AS 'ImageUrl'
	
					FROM [dbo].[Advertisments] 
	
					WHERE 
						[dbo].[Advertisments].IsArchieved = ISNULL(@IsArchieved,Advertisments.IsArchieved) AND 
						[dbo].[Advertisments].[ApplicationUserId] = @UserId AND 
						(@IsActive IS NULL OR IsActive = @IsActive) AND 
							(@Filter IS NULL OR Advertisments.Name LIKE '%'+@Filter +'%' OR Advertisments.[Description] LIKE '%'+@Filter +'%')
					ORDER BY dbo.Advertisments.IsExpired asc,dbo.Advertisments.IsPaided Desc OFFSET @PageSize * ( @PageNumber - 1 ) ROWS FETCH NEXT @PageSize ROWS ONLY;
	
END");
			/****** Object:  StoredProcedure  [AdvertisementPrice_GetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [AdvertisementPrice_GetAll]
AS
BEGIN
	SELECT
			AdvertismentPrices.Id ,
			AdvertismentPrices.[Period],
			AdvertismentPrices.[Price],
			COUNT(AdvertismentPrices.Id) OVER() AS 'OverAllCount'  
			FROM
			AdvertismentPrices 
	Where 
			AdvertismentPrices.IsArchieved <> 1
	ORDER BY AdvertismentPrices.Id
			
END");
			/****** Object:  StoredProcedure  [AdvertisementPriceArchieveCount]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [AdvertisementPriceArchieveCount]
AS
BEGIN
	
	select Count(AdvertismentPrices.Id) from AdvertismentPrices where IsArchieved = 1
	
END");
			/****** Object:  StoredProcedure  [AdvertisementPriceArchieveGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [AdvertisementPriceArchieveGetAll]
	@index [int],
	@rowNumber [int],
	@filter [nvarchar](100) = ''
AS
BEGIN
	
	SELECT * FROM AdvertismentPrices 
		Where AdvertismentPrices.IsArchieved = 1 and 
	(AdvertismentPrices.Period like '%'+@filter+'%' or 
	AdvertismentPrices.Price like '%'+@filter+'%')
		ORDER BY Id OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY
	
END");
			/****** Object:  StoredProcedure  [AdvertisementPriceGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [AdvertisementPriceGetAll]
	@index [int],
	@rowNumber [int],
	@filter [nvarchar](100) = '',
	@IsArchieved [bit] = NULL
AS
BEGIN
	
	SELECT
			AdvertismentPrices.Id ,
			AdvertismentPrices.[Period],
			AdvertismentPrices.[Price],
			COUNT(AdvertismentPrices.Id) OVER() AS 'OverAllCount'  
		FROM
			AdvertismentPrices 
	Where 
			AdvertismentPrices.IsArchieved = ISNULL(@IsArchieved , AdvertismentPrices.IsArchieved) 
		and 
			(AdvertismentPrices.Period like '%'+@filter+'%' or 
			 AdvertismentPrices.Price like '%'+@filter+'%')
	ORDER BY
			 Id OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY
	
END");
			/****** Object:  StoredProcedure  [AdvertisementPriceGetSingle]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [AdvertisementPriceGetSingle]
	@id [int]
AS
BEGIN
	
	select * from AdvertismentPrices where AdvertismentPrices.Id = @id
	
END");
			/****** Object:  StoredProcedure  [AdvertisementPriceIsDuplicateDaysCount]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [AdvertisementPriceIsDuplicateDaysCount]
	@DaysCount [int],
	@AdvertismentPriceId [int] = NULL
AS
BEGIN
	SELECT COUNT(Id) FROM AdvertismentPrices
	WHERE CAST(PeriOd AS INT) = @DaysCount
	AND(@AdvertismentPriceId IS NULL OR Id != @AdvertismentPriceId) 
END");
			/****** Object:  StoredProcedure  [AdvertisementPriceIsRelatedWithAdvertisement]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [AdvertisementPriceIsRelatedWithAdvertisement]
	@id [int]
AS
BEGIN
	
	select count(*) from Advertisments
	where Advertisments.AdvertismentPriceId = @id
	
END");
			/****** Object:  StoredProcedure  [Advertisment_Delete]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [Advertisment_Delete]
	@Id [int]
AS
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION
	
	DELETE FROM AdvertismentImages WHERE  AdvertismentId = @Id
					 DELETE FROM  ChatRequests WHERE AdvertismentId = @Id
					 DELETE FROM Comments WHERE AdvertismentId = @Id
					 DELETE FROM Complaints WHERE AdvertismentId = @Id
					 DELETE FROM Favorites WHERE AdvertismentId = @Id
					 DELETE FROM Likes WHERE AdvertismentId = @Id
					 DELETE FROM Advertisments WHERE Id = @Id
	
	COMMIT TRANSACTION
	SELECT 1 
	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK;
	SELECT 0
	END CATCH
END");
			/****** Object:  StoredProcedure  [AdvertismentImagesByAdsId]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [AdvertismentImagesByAdsId]
	@AdId [int]
AS
BEGIN
	SELECT*
	FROM [dbo].[AdvertismentImages]
	WHERE [dbo].[AdvertismentImages].AdvertismentId=@AdId
	ORDER BY [IsMainImage] DESC
END");
			/****** Object:  StoredProcedure  [Advertisments_SelectAllPaging]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"

ALTER  PROCEDURE  [Advertisments_SelectAllPaging]
	@PageNumber [int] = 1,
	@PageSize [int] = NULL,
	@IsArchieved [bit] = NULL,
	@IsActive [bit] = NULL 


AS
BEGIN
	
	
	
	UPDATE dbo.Advertisments
	SET dbo.Advertisments.IsExpired = 1 WHERE
	 [Advertisments].EndDate <= GetDate()


	SELECT 
	Id,Cost, OverallCount = COUNT(1) OVER ( ),
	(SELECT TOP(1)
	[ImageUrl] 
	FROM
	[AdvertismentImages]
	WHERE
	[AdvertismentImages].[AdvertismentId]=[Advertisments].Id
	) AS 'ImageUrl'
	FROM [Advertisments]
	WHERE 
   (@IsArchieved is null or IsNull(IsArchieved,0)=@IsArchieved) AND 
   --(@IsActive is null or IsActive = @IsActive)  AND 
   --IsArchieved=@IsArchieved and
   (@IsActive is null or IsActive = @IsActive) and
	 [Advertisments].IsPaided=1 AND  [Advertisments].[IsExpired]=0
	 
	 and (DATEDIFF(day,[Advertisments].EndDate,GetDate())<=0)  
	 and (DATEDIFF(day,[Advertisments].StartDate ,GetDate())>=0)

	--and    GetDate() BETWEEN	[Advertisments].StartDate and  [Advertisments].EndDate
	ORDER BY  NEWID()
	--LIMIT @PageSize
	-- [IsPaided] DESC,
	--[CreateDate] DESC
	OFFSET @PageSize * ( @PageNumber - 1 ) ROWS
	FETCH NEXT @PageSize ROWS ONLY
	OPTION  ( RECOMPILE );
	
END");
			/****** Object:  StoredProcedure  [AspNetUsers_Select_Info]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [AspNetUsers_Select_Info]
	@UserName [nvarchar](256)
AS
BEGIN
	SELECT 
	Id,
	Name,
	Email,
	PhotoUrl,
	(SELECT Count(Id) FROM Advertisments WHERE AspNetUsers.Id=Advertisments.ApplicationUserId ) AS 'AdsCount' ,
	PhoneNumber
	FROM AspNetUsers
	WHERE UserName=@UserName
END");
			/****** Object:  StoredProcedure  [AspNetUsers_Update_ImageUrl]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [AspNetUsers_Update_ImageUrl]
	@Id [nvarchar](128),
	@PhotoUrl [nvarchar](max)
AS
BEGIN
	 Update AspNetUsers SET PhotoUrl=@PhotoUrl WHERE Id=@Id
END");
			/****** Object:  StoredProcedure  [AspNetUsers_Update_Info]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [AspNetUsers_Update_Info]
	@Id [nvarchar](128),
	@Name [nvarchar](150),
	 @Phone [nvarchar](max)
AS
BEGIN
	 Update AspNetUsers
	  SET Name= isnull(@Name,Name),
	   PhoneNumber=isnull(@Phone,PhoneNumber)	 
	  WHERE Id=@Id
END");
			/****** Object:  StoredProcedure  [BankAccountGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [BankAccountGetAll]
	@index [int],
	@rowNumber [int],
	@filter [nvarchar](100) = ''
AS
BEGIN
	
	SELECT * FROM BankAccounts 
		Where  
	(BankAccounts.BankNumber like '%'+@filter+'%' or 
	BankAccounts.BankName like '%'+@filter+'%')
		ORDER BY Id OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY
	
END");
			/****** Object:  StoredProcedure  [BankAccountGetSingle]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [BankAccountGetSingle]
	@id [int]
AS
BEGIN
	
	select * from BankAccounts 
	where BankAccounts.Id = @id
	
END");
			/****** Object:  StoredProcedure  [Categories_SelectAllPaging]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [Categories_SelectAllPaging]
	@PageNumber [int] = 1,
	@PageSize [int] = NULL,
	@Filter [nvarchar](250) = NULL,
	@IsArchieved [bit] = NULL
AS
BEGIN
	
	
	SELECT 
	Id,Name,ImageUrl,
	OverallCount = COUNT(1) OVER ( )
	FROM [dbo].[Categories]
	WHERE 
	IsArchieved=ISNULL(@IsArchieved,IsArchieved)
	
	AND
	(
	@Filter IS NULL
	OR Name  LIKE '%' + @Filter + '%')
	
	ORDER BY Id ASC
	OFFSET @PageSize * ( @PageNumber - 1 ) ROWS
	FETCH NEXT @PageSize ROWS ONLY
	OPTION  ( RECOMPILE );
	
END");
			/****** Object:  StoredProcedure  [CategoryArchieveCount]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [CategoryArchieveCount]
AS
BEGIN
	
	select count(*) from Categories where 
	Categories.IsArchieved = 1
	
END");
			/****** Object:  StoredProcedure  [CategoryArchieveGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [CategoryArchieveGetAll]
	@index [int],
	@rowNumber [int],
	@filter [nvarchar](100) = ''
AS
BEGIN
	
	SELECT * FROM Categories 
		Where  Categories.IsArchieved = 1 and 
	(Categories.Name like '%'+@filter+'%')
		ORDER BY Id OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY
	
END");
			/****** Object:  StoredProcedure  [CategoryCount]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [CategoryCount]
AS
BEGIN
	
	select count (*)from Categories
	where Categories.IsArchieved = 0
	
END");
			/****** Object:  StoredProcedure  [CategoryGetName]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [CategoryGetName]
AS
BEGIN
	
						SELECT Categories.Id , Categories.Name , Categories.IconName FROM Categories 
					
END");
			/****** Object:  StoredProcedure  [CategoryIsRelatedWithAdvertisements]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [CategoryIsRelatedWithAdvertisements]
	@id [int]
AS
BEGIN
	
	select count(*) from Advertisments
	where Advertisments.CategoryId = @id
	
END");
			/****** Object:  StoredProcedure  [CommentDeleteAllReplays]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [CommentDeleteAllReplays]
	@id [int]
AS
BEGIN
	
	Delete Comments 
						where CommentParentId = @id
	
END");
			/****** Object:  StoredProcedure  [CommentGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [CommentGetAll]
	@Id [int],
	@PageNumber [int] = 1,
	@PageSize [int] = 8,
	@Filter [nvarchar](250) = NULL
AS
BEGIN
	 select Comments.* , AspNetUsers.Name 'UserFirstName', AspNetUsers.PhotoUrl as 'PhotoUrl',  COUNT(Comments.Id) OVER() AS 'OverAllCount'
	from Comments 
	inner join AspNetUsers
	on AspNetUsers.Id = Comments.ApplicationUserId
	where Comments.AdvertismentId = @Id and Comments.CommentParentId is null
						  and (@Filter is null or AspNetUsers.Name LIKE '%'+@Filter +'%' or Comments.[Message] LIKE '%'+@Filter +'%') 
	ORDER BY Comments.CreateDate DESC OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
	
END");
			/****** Object:  StoredProcedure  [CommentGetSingle]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [CommentGetSingle]
	@id [int]
AS
BEGIN
	
	select Comments.* , AspNetUsers.Name 'UserFirstName'
	from Comments
	inner join AspNetUsers
	on AspNetUsers.Id = Comments.ApplicationUserId
	where Comments.Id = @id
	
END");
			/****** Object:  StoredProcedure  [CommentGetSingleWithoutUser]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [CommentGetSingleWithoutUser]
	@id [int]
AS
BEGIN
	
	select * from Comments where Comments.Id = @id
	
END");
			/****** Object:  StoredProcedure  [CommentReplyGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [CommentReplyGetAll]
	@id [int]
AS
BEGIN
	
	select Comments.* , AspNetUsers.Name 'UserFirstName'
	from Comments 
	inner join AspNetUsers
	on AspNetUsers.Id = Comments.ApplicationUserId
	where Comments.CommentParentId = @id
	
END");
			/****** Object:  StoredProcedure  [Comments_GetPagedComments]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [Comments_GetPagedComments]
	@PageNumber [int],
	@PageSize [int],
	@Filter [nvarchar](250)
AS
BEGIN
	SELECT Comments.* , Advertisments.Name AS AdvertismentName, AspNetUsers.Name AS UserFirstName FROM Comments
	INNER JOIN Advertisments ON Advertisments.Id = Comments.AdvertismentId
	INNER JOIN AspNetUsers ON AspNetUsers.Id = Comments.ApplicationUserId
	WHERE (@Filter is null or (AspNetUsers.Name LIKE '%'+@Filter +'%' or Comments.[Message] LIKE '%'+@Filter +'%')) 
	ORDER BY AdvertismentId OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END");
			/****** Object:  StoredProcedure  [ComplaintAdvertisementArchieveGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [ComplaintAdvertisementArchieveGetAll]
AS
BEGIN
	
	select complain.*, users.UserName 'CamplaintUser' , Advertisments.Name 'AdvertisementName'
	from Complaints  complain
	left join AspNetUsers users    
	on 
	ApplicationUserId = users.Id
	left join Advertisments
	on 
	AdvertismentId = Advertisments.Id
	
	where complain.ComplainedId is  null and complain.IsArchieved = 1
	
END");
			/****** Object:  StoredProcedure  [ComplaintAdvertisementGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [ComplaintAdvertisementGetAll]
	@index [int],
	@rowNumber [int],
	@filter [nvarchar](100)
AS
BEGIN
   



	select complain.*, 
	users.UserName 'CamplaintUser' ,
	Advertisments.Name 'AdvertisementName',
	 users.Email,
	 users.PhoneNumber,
	 
	 (SELECT anu.UserName FROM dbo.AspNetUsers anu WHERE anu.Id=Advertisments.ApplicationUserId) AS ComplainedUser,
	 (SELECT anu.Name FROM dbo.AspNetUsers anu WHERE anu.Id=Advertisments.ApplicationUserId) AS ComplainedName,
	  (SELECT anu.Email FROM dbo.AspNetUsers anu WHERE anu.Id=Advertisments.ApplicationUserId) AS ComplainedEmail,
	 (SELECT anu.PhoneNumber FROM dbo.AspNetUsers anu WHERE anu.Id=Advertisments.ApplicationUserId) AS ComplainedPhoneNumber,

	 COUNT(Advertisments.Id) OVER() AS 'OverAllCount'
	from Complaints complain
	left
	join AspNetUsers users
	on
	ApplicationUserId = users.Id
	left
	join Advertisments
	on
	AdvertismentId = Advertisments.Id
	where complain.ComplainedId is  null and complain.IsArchieved = 0
	and(complain.[Message] LIKE '%' + @filter + '%' or
	users.username   LIKE '%' + @filter + '%' or
	Advertisments.name LIKE '%' + @filter + '%')
	ORDER BY complain.Id Desc OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY; 
END");
			/****** Object:  StoredProcedure  [ComplaintGetSingle]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [ComplaintGetSingle]
	@id [int]
AS
BEGIN
	
	select * from Complaints where Complaints.Id = @id
	
END");
			/****** Object:  StoredProcedure  [ComplaintUserArchieveGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [ComplaintUserArchieveGetAll]
AS
BEGIN
	
	select complain.*, users.UserName 'CamplaintUser' , complained.UserName 'ComplainedUser'
	from Complaints  complain
	left join AspNetUsers users    
	on 
	ApplicationUserId = users.Id
	left join AspNetUsers complained 
	on 
	ComplainedId = complained.Id
	
	where complain.ComplainedId is not null and complain.IsArchieved = 1
	
END");
			/****** Object:  StoredProcedure  [ComplaintUserGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [ComplaintUserGetAll]
AS
BEGIN
	
	select complain.*,
	 users.UserName 'CamplaintUser' 
	, complained.UserName 'ComplainedUser'
	--,
	--users.Email ,users.PhoneNumber
	from Complaints  complain
	left join AspNetUsers users    
	on 
	ApplicationUserId = users.Id
	left join AspNetUsers complained 
	on 
	ComplainedId = complained.Id
	
	where complain.ComplainedId is not null and complain.IsArchieved = 0
	
END");
			/****** Object:  StoredProcedure  [ContactInformationGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"

ALTER  PROCEDURE  [ContactInformationGetAll]  --null,null,null
	@PageNumber [int] ,
	@PageSize [int] ,
	@Filter [nvarchar](250) = NULL
AS
if(@PageNumber =0 and @PageSize=0)

BEGIN
	 SELECT ContactInformations.* ,[ContactTypes].[Type] as 'ContactTypeName' , COUNT(ContactInformations.Id) OVER() AS 'OverAllCount'
	FROM ContactInformations
						left join 
						[dbo].[ContactTypes]
						on ContactInformations.[ContactTypeId]=[ContactTypes].Id
	where 
						@Filter is null or 
	ContactInformations.Contact like '%'+@Filter+'%' 
	ORDER BY Id DESC  
end

else
BEGIN
	 SELECT ContactInformations.* ,[ContactTypes].[Type] as 'ContactTypeName' , COUNT(ContactInformations.Id) OVER() AS 'OverAllCount'
	FROM ContactInformations
						left join 
						[dbo].[ContactTypes]
						on ContactInformations.[ContactTypeId]=[ContactTypes].Id
	where 
						@Filter is null or 
	ContactInformations.Contact like '%'+@Filter+'%' 
	ORDER BY Id DESC OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
	
END");
			/****** Object:  StoredProcedure  [ContactInformationGetSingle]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [ContactInformationGetSingle]
	@id [int]
AS
BEGIN
	
	select * from ContactInformations where ContactInformations.Id = @id
	
END");
			/****** Object:  StoredProcedure  [ContactTypeAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [ContactTypeAll]
AS
BEGIN
	   
	SELECT *
	FROM 
					[dbo].[ContactTypes]
	ORDER BY Id DESC 
END");
			/****** Object:  StoredProcedure  [ContactTypeGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [ContactTypeGetAll]
	@index [int],
	@rowNumber [int],
	@filter [nvarchar](100) = ''
AS
BEGIN
	
	SELECT * FROM ContactTypes 
		ORDER BY Id OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY
	
END");
			/****** Object:  StoredProcedure  [ContactTypeGetSingle]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [ContactTypeGetSingle]
	@id [int]
AS
BEGIN
	
	select * from ContactTypes where ContactTypes.Id = @id
	
END");
			/****** Object:  StoredProcedure  [ContactTypeIsRelatedToContactInformation]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [ContactTypeIsRelatedToContactInformation]
	@id [int]
AS
BEGIN
	
	select count(*) from ContactInformations where ContactInformations.ContactTypeId = @id
	
END");
			/****** Object:  StoredProcedure  [ContactUsMessage_GetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"

Alter  PROCEDURE  [ContactUsMessage_GetAll]
	@PageNumber [int]=1,
	@PageSize [int]=10,
	@Filter [nvarchar](100) = null,
	@IsArchieved [bit] = NULL
AS
BEGIN
	
   SELECT *,COUNT(cm.Id) OVER() AS 'OverAllCount' 
	
	FROM  [dbo].[ContactUsMessages] cm

   Where  
   cm.IsArchieved =isnull(@IsArchieved , IsArchieved)
		and 
  (@Filter IS  null or ( cm.Message like '%'+@Filter+'%')  )
  and 
  (@Filter IS  null or ( cm.Name like '%'+@Filter+'%')  )
		ORDER BY Id DESC 
		OFFSET @PageSize * ( @PageNumber - 1 ) ROWS
FETCH NEXT @PageSize ROWS ONLY
OPTION  ( RECOMPILE );

END");
			/****** Object:  StoredProcedure  [FavoriteGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"


Alter  PROCEDURE  [FavoriteGetAll]
	@username [nvarchar](256),
	@index [int],
	@rowNumber [int]
AS
BEGIN
	
	select  Favorites.[Id] , 
			Advertisments.Name , 
			Advertisments.Cost,
			Advertisments.Id as AdvertismentId,
			Advertisments.IsPaided,Advertisments.IsExpired,
			COUNT(Favorites.Id) OVER() AS 'OverAllCount',
			ISNULL([NumberOfViews],0) AS 'NumberOfViews',
			ISNULL(NumberOfLikes,0) AS 'NumberOfLikes',
			
			(
				SELECT
				COUNT(Id)
				FROM [dbo].[Comments]
				WHERE [dbo].[Comments].AdvertismentId=[dbo].[Advertisments].Id
			) AS 'Comments',
			(
				SELECT
				TOP 1 [ImageUrl]
				FROM [dbo].[AdvertismentImages]
				WHERE [dbo].[AdvertismentImages].AdvertismentId=[dbo].[Advertisments].Id
				ORDER BY [IsMainImage] DESC
			) AS 'ImageUrl'
	
	from Favorites
	inner join Advertisments on 
	Advertisments.Id = Favorites.AdvertismentId
	
		inner join AspNetUsers on
	AspNetUsers.Id = Favorites.ApplicationUserId 
	
	where AspNetUsers.UserName = @username
	
	ORDER BY Advertisments.IsExpired asc,Advertisments.IsPaided Desc OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY;
	
END
");
			/****** Object:  StoredProcedure  [FavoriteGetOne]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [FavoriteGetOne]
	@id [int]
AS
BEGIN
	
	select * from Favorites  where Favorites.Id = @id
	
END");
			/****** Object:  StoredProcedure  [FavoriteGetSingle]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [FavoriteGetSingle]
	@id [int]
AS
BEGIN
	
	select Favorites.* , Advertisments.Name 'AdvertisementName' 
	, AspNetUsers.Name 'UserFirstName' 
	from Favorites
	inner join Advertisments on 
	Advertisments.Id = Favorites.AdvertismentId
	inner join AspNetUsers on
	AspNetUsers.Id = Favorites.ApplicationUserId 
	
	where Favorites.Id = @id 
	
END");
			/****** Object:  StoredProcedure  [LikeGetAll]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [LikeGetAll]
	@id [int]
AS
BEGIN
	
	select Likes.* , AspNetUsers.Name 'UserFirstName' 
	from Likes join  AspNetUsers  on
	AspNetUsers.Id = Likes.ApplicationUserId
	where AdvertismentId = @id
	
END");
			/****** Object:  StoredProcedure  [LikeGetSinlge]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER  PROCEDURE  [LikeGetSinlge]
	@id [int]
AS
BEGIN
	
	select * from Likes where Likes.Id = @id
	
END");
			/****** Object:  StoredProcedure  [Notification_GetList_ByUser]    Script Date: 5/21/2017 2:57:55 PM ******/
			Sql(@"
ALTER PROCEDURE  [Notification_GetList_ByUser] 
@Lang nvarchar(40),
@PageSize int,
@PageNumber int,
@UserId nvarchar(128)
as
select

		CASE

		WHEN  @Lang = 'ar' THEN ISNULL(d.Message, d.EnglishMessage)

		WHEN  @Lang = 'en' THEN ISNULL(d.EnglishMessage, d.Message)

		END AS Msg
 from PushNotifications as d
 join NotificationsLogs on d.Id = NotificationLogs.NotificationMessageId where NotificationLogs.ApplicationUserId = @UserId
 ORDER BY d.CreationDate DESC

	OFFSET @PageSize * (@PageNumber - 1) ROWS

	FETCH NEXT @PageSize ROWS ONLY

	OPTION(RECOMPILE)

");
/****** Object:  StoredProcedure  [Notifications_Mark_AsRead]    Script Date: 5/21/2017 2:57:55 PM ******/
Sql(@"
			ALTER  PROCEDURE[Notifications_Mark_AsRead]
			
				@NotificationIds nvarchar(max)
			

			AS
			
				SET NOCOUNT ON;
			BEGIN TRY
	BEGIN TRANSACTION


		   UPDATE[dbo].[PushNotifications] set[dbo].[PushNotifications].[Notified] = 1

			  WHERE Id IN(
					SELECT CAST(Item AS INTEGER)

					FROM[SplitString](@NotificationIds, ',')
			  )
	 SELECT  @@ROWCOUNT
	COMMIT TRANSACTION



	END TRY

	BEGIN CATCH

	IF @@TRANCOUNT > 0 ROLLBACK;
			SELECT 0


	END CATCH
");
/****** Object:  StoredProcedure  [Notifications_Select_ListByUserId]    Script Date: 5/21/2017 2:57:55 PM ******/
Sql(@"

ALTER  PROCEDURE  [Notifications_Select_ListByUserId]
	@UserId [nvarchar](128),
	@Lang [nvarchar](2),
	@PageNumber [int] = 1,
	@PageSize [int] = 10
AS
BEGIN
   bEGIN TRY
   
  SELECT pn.Id  ,  pn.ChatRequestId , pn.[Notified],cr.AdvertismentId,a.Name AS AdvertisementName,
	CASE 
	WHEN @UserId=ch.UserOneId THEN ch.UserTwoId
	WHEN @UserId=ch.UserTwoId THEN ch.UserOneId
	 end as 'Sender',

	 (SELECT UserName from dbo.AspNetUsers anu
	 WHERE anu.Id!=@UserId AND ( anu.Id=ch.UserTwoId OR anu.Id=ch.UserOneId)  ) AS UserName
	 ,

	  (SELECT PhotoUrl from dbo.AspNetUsers anu
	 WHERE anu.Id!=@UserId AND ( anu.Id=ch.UserTwoId OR anu.Id=ch.UserOneId)  ) AS PhotoUrl

	 ,
	  (SELECT Name from dbo.AspNetUsers anu
	 WHERE anu.Id!=@UserId AND ( anu.Id=ch.UserTwoId OR anu.Id=ch.UserOneId)  ) AS Name
	 ,
	case
	when @Lang = 'ar' then   pn.[Message] 
	when @Lang = 'en' then  pn.EnglishMessage	
	end as 'Message',
	OverallCount = COUNT(1) OVER() ,
	NotifiedCount =(SELECT COUNT(pn.Id )  FROM
				dbo.NotificationLogs nl	
				join 
				dbo.PushNotifications pn 
				ON nl.NotificationMessageId=pn.Id
				WHERE nl.ApplicationUserId=@UserId  
				and  pn.[Notified]=0)
	FROM
	dbo.NotificationLogs nl	
	join 
	dbo.PushNotifications pn 
	ON nl.NotificationMessageId=pn.Id
	JOIN dbo.ChatRequests cr	
	ON pn.ChatRequestId = cr.Id
	 JOIN dbo.ChatHeaders ch	
	ON	cr.Id = ch.RequestId	
	JOIN dbo.Advertisments a
	ON cr.AdvertismentId = a.Id

	WHERE nl.ApplicationUserId=@UserId
	ORDER BY
	pn.[CreationDate]  DESC	
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY OPTION (RECOMPILE);
END TRY
BEGIN CATCH
	SELECT 
	--ERROR_NUMBER() AS Message
	 --,ERROR_SEVERITY() AS ErrorSeverity
	 --,ERROR_STATE() AS ErrorState
	 --,ERROR_PROCEDURE() AS ErrorProcedure,
	 ERROR_LINE() AS OverallCount
	 ,ERROR_MESSAGE() AS 'Message';
END CATCH
   
   
END");

		}




		public override void Down()
		{
		}
	}
}

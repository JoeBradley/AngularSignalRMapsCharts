INSERT INTO [RealTimeReporting].dbo.[EventLog]
           ([LogGUID]
           ,[LogTypeKey]
           ,[LogConfigID]
           ,[LogUserID]
           ,[LogUserName]
           ,[LogPortalID]
           ,[LogPortalName]
           ,[LogCreateDate]
           ,[LogServerName]
           ,[LogProperties]
           ,[LogNotificationPending])
     VALUES
           (
		   NEWID(),
		   'HOST_ALERT',
		   95,
		   NULL,
		   'host',
		   0,
		   NULL, 
		   GETDATE(),
		   'Cass',
		   '<LogProperties><LogProperty><PropertyName>Install Package:</PropertyName><PropertyValue>CCFileExplorer</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Starting Installation</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Starting Installation - CCFileExplorer</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Starting Installation - Script</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Begin Sql execution</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - Providers\DataProviders\SqlDataProvider\00.00.01.SqlDataProvider</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - Providers\DataProviders\SqlDataProvider\00.00.01.SqlDataProvider</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Executing 00.00.01.SqlDataProvider</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Start Sql execution: 00.00.01.SqlDataProvider file</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>End Sql execution: 00.00.01.SqlDataProvider file</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - Providers\DataProviders\SqlDataProvider\Uninstall.SqlDataProvider</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - Providers\DataProviders\SqlDataProvider\Uninstall.SqlDataProvider</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Finished Sql execution</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Component installed successfully - Script</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Starting Installation - ResourceFile</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Expanding Resource file</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - Edit.ascx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - Edit.ascx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - License.txt</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - License.txt</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - module.css</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - module.css</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - ReleaseNotes.txt</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - ReleaseNotes.txt</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - Settings.ascx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - Settings.ascx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - View.ascx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - View.ascx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - App_LocalResources\Edit.ascx.resx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - App_LocalResources/Edit.ascx.resx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - App_LocalResources\Settings.ascx.resx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - App_LocalResources/Settings.ascx.resx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - App_LocalResources\View.ascx.resx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - App_LocalResources/View.ascx.resx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - Documentation\Documentation.css</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - Documentation/Documentation.css</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - Documentation\Documentation.html</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - Documentation/Documentation.html</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Resource Files created</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Component installed successfully - ResourceFile</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Starting Installation - Module</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Module registered successfully - CCFileExplorer</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Component installed successfully - Module</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Starting Installation - Assembly</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Assembly registered - bin\CCFileExplorer.dll</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - bin\CCFileExplorer.dll</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - bin\CCFileExplorer.dll</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Component installed successfully - Assembly</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Installation committed</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Installation successful. - CCFileExplorer</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Deleted temporary install folder</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Installation successful.</PropertyValue></LogProperty></LogProperties>',
		   NULL);



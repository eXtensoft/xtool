
 e.[Id]
      ,e.[CreatedAt]
	  ,DATENAME(month,e.[CreatedAt]) as [Month]
	  ,DATENAME(day,e.[CreatedAt]) as [Day]
      ,e.[ApplicationKey]
      ,e.[Zone]
      ,e.[AppContextInstance]
      ,e.[MessageId]
      ,e.[Category]
      ,e.[Severity]
      ,e.[Message]
	  ,r.[ApiRequestId]
	  ,s.[Id] as [SessionId]
      --,[XmlData]
FROM [<schema>].ApiRequest r LEFT OUTER JOIN
                         [<schema-session>].Session s ON r.BearerToken = s.BearerToken RIGHT OUTER JOIN
                         [<schema-error>].Error e ON r.MessageId = e.MessageId
WHERE (e.[CreatedAt] >= @From and e.[CreatedAt] <= @To)
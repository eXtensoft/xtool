  SELECT a.FacetGroup, a.FacetKey, a.FacetVAlue,a.FacetGroup,a.InstanceCount
  FROM
  (
	SELECT DISTINCT TOP (100) PERCENT CONVERT(nvarchar(36),[BasicToken]) as [FacetValue], 'BasicToken' AS [FacetKey],'Session' AS [FacetGroup], count([<schema>].[Session].[Id]) as [InstanceCount]
	FROM [<schema>].[Session] (nolock)
	WHERE [CreatedAt] >= @From AND [CreatedAt] <= @To
	GROUP BY CONVERT(nvarchar(36),[BasicToken])

	UNION SELECT DISTINCT TOP (100) PERCENT CONVERT(nvarchar(12),[TenantId]) as [FacetValue], 'Tenant' AS [FacetKey],'Session' AS [FacetGroup], count([<schema>].[Session].[Id]) as [InstanceCount]
	FROM [<schema>].[Session] (nolock)
	WHERE [CreatedAt] >= @From AND [CreatedAt] <= @To
	GROUP BY CONVERT(nvarchar(12),[TenantId])

	UNION SELECT DISTINCT TOP (100) PERCENT DATENAME(dw,[CreatedAt]) as [FacetValue], 'DayOfWeek' AS [FacetKey],'Session' AS [FacetGroup], count([<schema>].[Session].[Id]) as [InstanceCount]
	FROM [<schema>].[Session] (nolock)
	WHERE [CreatedAt] >= @From AND [CreatedAt] <= @To
	GROUP BY DATENAME(dw,[CreatedAt])

	UNION SELECT DISTINCT TOP (100) PERCENT DATENAME(hh,[CreatedAt]) as [FacetValue], 'HourOfDay' AS [FacetKey],'Session' AS [FacetGroup], count([<schema>].[Session].[Id]) as [InstanceCount]
	FROM [<schema>].[Session] (nolock)
	WHERE [CreatedAt] >= @From AND [CreatedAt] <= @To
	GROUP BY DATENAME(hh,[CreatedAt])
  )a ORDER BY a.FacetKey,a.FacetValue
SELECT a.FacetGroup,a.FacetKey,a.FacetValue,a.FacetGroup,a.InstanceCount
from
(
SELECT DISTINCT TOP (100) PERCENT Severity AS [FacetValue], 'Severity' AS [FacetKey], 'Error' AS [FacetGroup], count([<schema>].[Error].Id) as [InstanceCount]
FROM [<schema>].[Error] (nolock)
WHERE [CreatedAt] >= @From AND [CreatedAt] <= @To
GROUP BY [Severity]

UNION SELECT DISTINCT TOP (100) PERCENT [Zone] AS [FacetValue], 'Zone' AS [FacetKey], 'Error' AS [FacetGroup], count([<schema>].[Error].Id) as [InstanceCount]
FROM [<schema>].[Error] (nolock)
WHERE [CreatedAt] >= @From AND [CreatedAt] <= @To
GROUP BY [Zone]

UNION SELECT DISTINCT TOP (100) PERCENT [ApplicationKey] AS [FacetValue], 'ApplicationKey' AS [FacetKey], 'Error' AS [FacetGroup], count([<schema>].[Error].Id) as [InstanceCount]
FROM [<schema>].[Error] (nolock)
WHERE [CreatedAt] >= @From AND [CreatedAt] <= @To
GROUP BY [ApplicationKey]

UNION SELECT DISTINCT TOP (100) PERCENT [AppContextInstance] AS [FacetValue], 'AppContextInstance' AS [FacetKey], 'Error' AS [FacetGroup], count([<schema>].[Error].Id) as [InstanceCount]
FROM [<schema>].[Error] (nolock)
WHERE [CreatedAt] >= @From AND [CreatedAt] <= @To
GROUP BY [AppContextInstance]

UNION SELECT DISTINCT TOP (100) PERCENT [Category] AS [FacetValue], 'Category' AS [FacetKey], 'Error' AS [FacetGroup], count([<schema>].[Error].Id) as [InstanceCount]
FROM [<schema>].[Error] (nolock)
WHERE [CreatedAt] >= @From AND [CreatedAt] <= @To
GROUP BY [Category]
)a
ORDER BY a.FacetKey,a.FacetValue

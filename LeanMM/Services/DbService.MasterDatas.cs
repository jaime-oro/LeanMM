using LeanMM.Models;

namespace LeanMM.Services
{
    public partial class DbService
    {
        public async Task<List<MMMasterData>> MD_GetByNameResultsAsync(string SearchTerm)
        {
            var query = @"
                SELECT TOP (100)
                    md.[NAME],
                    md.[TYPE],
                    md.[OSM_VERSION_ID],
                    md.[INFOS],
                    md.[DESCRIPTION],
                    md.[DESCRIPTION2],
                    md.[DESCRIPCION3],
                    CASE 
                        WHEN EXISTS (
                            SELECT 1 
                            FROM [medmgr].[WM_ELE_LINKS] AS links
                            INNER JOIN [medmgr].[MASTERDATA] AS child_md 
                                ON links.[CHILD_ELEMENT] = child_md.[INFOS]
                            WHERE links.[PARENT_ELEMENT] = md.[INFOS]
                        ) THEN CAST(1 AS BIT)
                        ELSE CAST(0 AS BIT)
                    END AS [HasChildren]
                FROM 
                    [medmgr].[MASTERDATA] AS md
                WHERE 
                    md.[NAME] LIKE REPLACE(REPLACE(@SearchTerm, '*', '%'), '?', '_')
                ORDER BY 
                    md.[NAME] DESC;";

            return await QueryAsync<MMMasterData>(query, new { SearchTerm = SearchTerm });
        }

        public async Task<MMMasterData?> MD_GetByInfosAsync(string Infos)
        {
            var query = @"
                SELECT TOP 1
                    md.[NAME],
                    md.[TYPE],
                    md.[OSM_VERSION_ID],
                    md.[INFOS],
                    md.[DESCRIPTION],
                    md.[DESCRIPTION2],
                    md.[DESCRIPCION3],
                    CASE 
                        WHEN EXISTS (
                            SELECT 1 
                            FROM [medmgr].[WM_ELE_LINKS] AS links
                            INNER JOIN [medmgr].[MASTERDATA] AS child_md 
                                ON links.[CHILD_ELEMENT] = child_md.[INFOS]
                            WHERE links.[PARENT_ELEMENT] = md.[INFOS]
                        ) THEN CAST(1 AS BIT)
                        ELSE CAST(0 AS BIT)
                    END AS [HasChildren]
                FROM 
                    [medmgr].[MASTERDATA] AS md
                WHERE 
                    md.[INFOS] = @Infos;";

            return await GetFirstOrDefaultAsync<MMMasterData>(query, new { Infos = Infos });
        }

        public async Task<List<MMMasterData>> MD_GetMDChildsByInfosAsync(string? Infos, int Level)
        {
            if (string.IsNullOrEmpty(Infos))
            {
                return new List<MMMasterData>();
            }

            var query = @"
                SELECT 
                    child_md.[NAME],
                    child_md.[TYPE],
                    child_md.[OSM_VERSION_ID],
                    child_md.[INFOS],
                    child_md.[DESCRIPTION],
                    child_md.[DESCRIPTION2],
                    child_md.[DESCRIPCION3],
                    COALESCE(NULLIF(links.[OVERWRITES], ''), links.[QUANTITY_DMS]) AS [Quantity],
                    parent_md.[INFOS] AS [ParentInfo],
                    @ParentInfos AS [ParentInfos],
                    @level AS [Level],
                    CASE 
                        WHEN EXISTS (
                            SELECT 1 
                            FROM [medmgr].[WM_ELE_LINKS] AS grand_links
                            INNER JOIN [medmgr].[MASTERDATA] AS grand_child_md 
                                ON grand_links.[CHILD_ELEMENT] = grand_child_md.[INFOS]
                            WHERE grand_links.[PARENT_ELEMENT] = child_md.[INFOS]
                        ) THEN CAST(1 AS BIT)
                        ELSE CAST(0 AS BIT)
                    END AS [HasChildren]
                FROM 
                    [medmgr].[MASTERDATA] AS parent_md
                INNER JOIN 
                    [medmgr].[WM_ELE_LINKS] AS links 
                    ON parent_md.[INFOS] = links.[PARENT_ELEMENT]
                INNER JOIN 
                    [medmgr].[MASTERDATA] AS child_md 
                    ON links.[CHILD_ELEMENT] = child_md.[INFOS]
                WHERE 
                    parent_md.[INFOS] = @ParentInfos
                ORDER BY 
                    links.[POS];";

            return await QueryAsync<MMMasterData>(query, new { ParentInfos = Infos, level = Level });
        }
    }
}

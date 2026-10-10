using LeanMM.Models;

namespace LeanMM.Services
{
    public partial class DbService
    {
        public async Task<List<MMElement>> EL_GetChildsByInfosAsync(string? Infos, int Level = 0)
        {
            if (string.IsNullOrEmpty(Infos)) return new List<MMElement>();

            var query = @"
                SELECT 
                    elem.[UNIQUE_ID_STRING],
                    elem.[CLASS_NAME],
                    elem.[INFOS],
                    elem.[ELID],
                    elem.[ALID],
                    COALESCE(NULLIF(link.[OVERWRITES], ''), link.[QUANTITY_DMS]) as [Quantity],

                    CAST(CASE 
                        WHEN EXISTS (
                            SELECT 1 
                            FROM [medmgr].[WM_ELE_LINKS] childCheck 
                            WHERE childCheck.[PARENT_ELEMENT] = elem.[INFOS]
                        ) THEN 1 
                        ELSE 0 
                    END AS BIT) AS [HasChildren]

                FROM [medmgr].[WM_ELE_LINKS] link 
                INNER JOIN [medmgr].[WM_ELEMENTS] elem 
                    ON link.[CHILD_ELEMENT] = elem.[INFOS]
                WHERE link.[PARENT_ELEMENT] = @Infos
                    AND elem.[CLASS_TYPE] = '$DOC'
                    AND elem.[CLASS_NAME] IN ('MODEL_3D', 'default_doc', 'DRAWING_2D', 'DRAWING_2D_ACAD')
                ORDER BY link.[POS];";

            var elements = await QueryAsync<MMElement>(query, new { Infos = Infos });

            var groupedElments = elements.GroupBy(e => new { e.Name, e.CLASS_NAME });
            foreach (var group in groupedElments)
            {
                int maxVersion = group.Max(e => e.Version);
                foreach (var element in group)
                {
                    element.IsLatestVersion = (element.Version == maxVersion);
                    element.Level = Level;
                }
            }

            return elements;
        }

        public async Task<List<MMElement>> EL_GetAllChildsByInfosAsync(string? Infos, int Level = 0)
        {
            if (string.IsNullOrEmpty(Infos)) return new List<MMElement>();

            var query = @"
                SELECT 
                    elem.[UNIQUE_ID_STRING],
                    elem.[CLASS_NAME],
                    elem.[INFOS],
                    elem.[ELID],
                    elem.[ALID]
                FROM [medmgr].[WM_ELE_LINKS] link 
                INNER JOIN [medmgr].[WM_ELEMENTS] elem 
                    ON link.[CHILD_ELEMENT] = elem.[INFOS]
                WHERE link.[PARENT_ELEMENT] = @Infos
                ORDER BY link.[POS];";

            var elements = await QueryAsync<MMElement>(query, new { Infos = Infos });

            var groupedElments = elements.GroupBy(e => new { e.Name, e.CLASS_NAME });
            foreach (var group in groupedElments)
            {
                int maxVersion = group.Max(e => e.Version);
                foreach (var element in group)
                {
                    element.IsLatestVersion = (element.Version == maxVersion);
                    element.Level = Level;
                }
            }

            return elements;
        }

        public async Task<MMElement?> EL_GetByInfosAsync(string? Infos)
        {
            if (string.IsNullOrEmpty(Infos)) return null;
            var query = @"
                SELECT 
                    [UNIQUE_ID_STRING],
                    [CLASS_NAME],
                    [INFOS],
                    [ELID],
                    [ALID]
                FROM [medmgr].[WM_ELEMENTS]
                WHERE [INFOS] = @Infos;";
            return await GetFirstOrDefaultAsync<MMElement>(query, new { Infos = Infos });
        }

        public async Task<MMModel3D> M3D_GetByInfosAsync(string? Infos)
        {
            if (string.IsNullOrEmpty(Infos)) return new MMModel3D();

            MMModel3D m3d = new();
            m3d!.MMElement = await EL_GetByInfosAsync(Infos);
            m3d.Files = await F_GetByInfosAsync(Infos);
            //m3d.File = await F_GetByInfosAsync(m3d.MMElement?.ELID);

            return m3d;
        }
    }
}

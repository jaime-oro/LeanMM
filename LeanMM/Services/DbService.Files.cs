using LeanMM.Models;

namespace LeanMM.Services
{
    public partial class DbService
    {
        public async Task<MMFile?> F_GetFirstByInfosAsync(string? Infos)
        {
            if (string.IsNullOrEmpty(Infos)) return null;

            var query = @"
                SELECT 
                    [FID],
                    [AREA_NAME],
                    [FILE_NAME],
                    [PHYS_NAME],
                    [FILE_TYPE],
                    [FILE_SIZE],
                    [PROGRAM_NAME],
                    [ELID],
                    DATEADD(s, 1493975122, '1970-01-01') AS [STORED_DATE],
                    [STORED_BY]
                FROM [medmgr].[WM_FILES]
                WHERE [ELID] = @Infos;";
            return await GetFirstOrDefaultAsync<MMFile>(query, new { Infos = Infos });
        }

        public async Task<List<MMFile>> F_GetByInfosAsync(string? Infos)
        {
            if (string.IsNullOrEmpty(Infos)) return new List<MMFile>();

            var query = @"
                SELECT 
                    [FID],
                    [AREA_NAME],
                    [FILE_NAME],
                    [PHYS_NAME],
                    [FILE_TYPE],
                    [FILE_SIZE],
                    [PROGRAM_NAME],
                    [ELID],
                    DATEADD(s, 1493975122, '1970-01-01') AS [STORED_DATE],
                    [STORED_BY]
                FROM [medmgr].[WM_FILES]
                WHERE [ELID] = @Infos;";
            return await QueryAsync<MMFile>(query, new { Infos = Infos });
        }
    }
}

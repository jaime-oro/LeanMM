using LeanMM.Models;

namespace LeanMM.Services
{
    public partial class DbService
    {
        private async Task<int> CheckTablesExist()
        {
            const string sql = @"
                IF OBJECT_ID(N'dbo.LMM_TASKS', N'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.LMM_TASKS (
                        ID INT IDENTITY(1,1) PRIMARY KEY,
                        USER_NAME NVARCHAR(100) NOT NULL,
                        NAME NVARCHAR(255) NOT NULL,
                        DATE_ADDED DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                        DATE_FINISHED DATETIME2 NULL,
                        FINISHED BIT NOT NULL DEFAULT 0,
                        CANCELLED BIT NOT NULL DEFAULT 0
                    );
                END;

                IF OBJECT_ID(N'dbo.LMM_JOBS', N'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.LMM_JOBS (
                        ID INT IDENTITY(1,1) PRIMARY KEY,
                        TASK_ID INT NOT NULL,
                        PHYS_NAME NVARCHAR(500) NOT NULL,
                        FILE_NAME NVARCHAR(255) NOT NULL,
                        FINISHED BIT NOT NULL DEFAULT 0
                    );
                END;";

            return await ExecuteAsync(sql);
        }

        public async Task<List<LMMTask>> LMMTask_GetByUserName(string userName)
        {
            await CheckTablesExist();
            const string sql = @"
                SELECT * FROM dbo.LMM_TASKS WHERE USER_NAME = @userName ORDER BY ID DESC";

            var tasks = await QueryAsync<LMMTask>(sql, new { userName });
            foreach (var task in tasks)
            {
                task.Jobs = await LMMJob_GetByTaskIdAsync(task.ID ?? 0);
            }
            return tasks;
        }

        public async Task<int> LMMTask_Insert(LMMTask task)
        {
            await CheckTablesExist();
            const string sql = @"
                INSERT INTO dbo.LMM_TASKS (USER_NAME, NAME, DATE_ADDED, DATE_FINISHED, FINISHED, CANCELLED)
                VALUES (@USER, @NAME, @DATE_ADDED, @DATE_FINISHED, @FINISHED, @CANCELLED);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            task.ID = await ExecuteScalarAsync<int>(sql, task);

            foreach (var job in task.Jobs ?? new List<LMMJob>())
            {
                job.TASK_ID = task.ID ?? 0;
                await LMMJob_Insert(job);
            }

            return task.ID ?? 0;
        }

        public async Task<List<LMMJob>> LMMJob_GetByTaskIdAsync(int taskId)
        {
            const string sql = @"
                SELECT * FROM dbo.LMM_JOBS WHERE TASK_ID = @taskId";


            return await QueryAsync<LMMJob>(sql, new { taskId });
        }

        public async Task<int> LMMJob_Insert(LMMJob job)
        {
            const string sql = @"
                INSERT INTO dbo.LMM_JOBS (TASK_ID, PHYS_NAME, FILE_NAME, FINISHED)
                VALUES (@TASK_ID, @PHYS_NAME, @FILE_NAME, @FINISHED);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            return await ExecuteAsync(sql, job);
        }

    }
}
